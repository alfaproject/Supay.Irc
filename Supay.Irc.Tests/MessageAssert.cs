using Microsoft.VisualStudio.TestTools.UnitTesting;
using Supay.Irc.Messages;

namespace Supay.Irc.Tests
{
  public static class MessageAssert
  {
    public static T TypeAndRoundTrip<T>(string raw) where T : class 
    {
      var msg = IrcMessageFactory.Parse(raw) as T;
      Assert.IsNotNull(msg);
      Assert.AreEqual(raw, msg.ToString());
      return msg;
    }
  }
}
