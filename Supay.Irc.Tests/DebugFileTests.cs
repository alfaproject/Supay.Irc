using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  [TestClass]
  public class DebugFileTests
  {
    [TestMethod]
    public void DebugFileTest()
    {
      var thisAssembly = typeof(DebugFileTests).Assembly;

      var logFileName = thisAssembly.GetManifestResourceNames().FirstOrDefault(resourceName => resourceName.Contains("irclog.txt"));
      Assert.IsNotNull(logFileName);

      var logStream = thisAssembly.GetManifestResourceStream(logFileName);
      Assert.IsNotNull(logStream);

      using (TextReader reader = new StreamReader(logStream))
      {
        while (reader.Peek() >= 0)
        {
          var line = reader.ReadLine();
          if (string.IsNullOrWhiteSpace(line))
          {
            continue;
          }
          line = line.Trim();

          if (line.StartsWith("-> ", StringComparison.Ordinal) || line.StartsWith("<- ", StringComparison.Ordinal))
          {
            line = (line.StartsWith("-> ", StringComparison.Ordinal) ? ":" : "") + line.Substring(3);
          }

          IrcMessage msg = IrcMessageFactory.Parse(line);
          Type msgType = msg.GetType();
          Assert.IsNotInstanceOfType(msg, typeof(GenericErrorMessage), "Message '{0}' parsed to generic message type '{1}'", line, msgType.Name);
          Assert.IsNotInstanceOfType(msg, typeof(GenericMessage), "Message '{0}' parsed to generic message type '{1}'", line, msgType.Name);
          Assert.IsNotInstanceOfType(msg, typeof(GenericNumericMessage), "Message '{0}' parsed to generic message type '{1}'", line, msgType.Name);
          Assert.IsNotInstanceOfType(msg, typeof(GenericCtcpReplyMessage), "Message '{0}' parsed to generic message type '{1}'", line, msgType.Name);
          if (msg is GenericCtcpRequestMessage)
          {
            var ctcp = msg as GenericCtcpRequestMessage;
            if (ctcp.Command != "SLOTS" && ctcp.Command != "MP3")
            {
              Assert.Fail("Message '{0}' parsed to GenericCtcpRequestMessage", line);
            }
          }
        }
      }
    }
  }
}
