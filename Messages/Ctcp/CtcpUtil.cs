using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Utility class for Ctcp messages.
  /// </summary>
  /// <remarks>
  /// This most likely doesn't need to be used from lib-user code.
  /// </remarks>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Util"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ctcp")]
  public static class CtcpUtil {

    /// <summary>
    /// The character used to indicate the start and end of an extended data section in a CTCP message.
    /// </summary>
    public static readonly Char ExtendedDataMarker = '\x0001';

    /// <summary>
    /// Escapes the given text for use in a ctcp message.
    /// </summary>
    public static String Escape(String text) {
      String escaper = '\x0014'.ToString();
      String NUL = '\x0000'.ToString();
      String result = text;
      result = System.Text.RegularExpressions.Regex.Replace(result, escaper, escaper + escaper);
      result = System.Text.RegularExpressions.Regex.Replace(result, NUL, escaper + "0");
      result = System.Text.RegularExpressions.Regex.Replace(result, "\r", escaper + "r");
      result = System.Text.RegularExpressions.Regex.Replace(result, "\n", escaper + "n");
      return result;
    }

    /// <summary>
    /// Unescapes the given text for use outside a ctcp message.
    /// </summary>
    public static String Unescape(String text) {
      String escaper = '\x0014'.ToString();
      String NUL = '\x0000'.ToString();
      String result = text;

      result = System.Text.RegularExpressions.Regex.Replace(result, escaper + "0", NUL);
      result = System.Text.RegularExpressions.Regex.Replace(result, escaper + "r", "\r");
      result = System.Text.RegularExpressions.Regex.Replace(result, escaper + "n", "\n");
      result = System.Text.RegularExpressions.Regex.Replace(result, escaper + escaper, escaper);

      return result;
    }

    /// <summary>
    /// Extracts the TransportCommand from the given message string.
    /// </summary>
    /// <remarks>
    /// A Ctcp TransportCommand is the irc command used to send the message to another user,
    /// and is not to be confused with the Ctcp Command.
    /// It is always either PRIVMSG or NOTICE for a valid ctcp command.
    /// </remarks>
    public static String GetTransportCommand(String rawMessage) {
      return MessageUtil.GetCommand(rawMessage);
    }

    /// <summary>
    /// Extracts the actual Ctcp command from the given message string.
    /// </summary>
    public static String GetInternalCommand(String rawMessage) {
      String ctcpMessage = MessageUtil.GetLastParameter(rawMessage);
      if (ctcpMessage.IndexOf(" ", StringComparison.Ordinal) > 0) {
        return ctcpMessage.Substring(1, ctcpMessage.IndexOf(" ", StringComparison.Ordinal) - 1);
      } else {
        return ctcpMessage.Substring(1, ctcpMessage.Length - 2);
      }
    }

    /// <summary>
    /// Extracts the extended data section of a Ctcp message.
    /// </summary>
    public static String GetExtendedData(String rawMessage) {
      String ctcpMessage = MessageUtil.GetLastParameter(rawMessage);
      String extendedData = "";
      if (ctcpMessage.IndexOf(" ", StringComparison.Ordinal) > 0) {
        extendedData = ctcpMessage.Substring(ctcpMessage.IndexOf(" ", StringComparison.Ordinal) + 1);
        extendedData = extendedData.Substring(0, extendedData.Length - 1);
      }
      return CtcpUtil.Unescape(extendedData);
    }

    /// <summary>
    /// Determines if the given message is a Ctcp message.
    /// </summary>
    public static bool IsCtcpMessage(String rawMessage) {
      StringCollection p = MessageUtil.GetParameters(rawMessage);
      if (p.Count != 2) {
        return false;
      }
      String payLoad = p[1];
      if (!(payLoad.StartsWith(CtcpUtil.ExtendedDataMarker.ToString(), StringComparison.Ordinal) && payLoad.EndsWith(CtcpUtil.ExtendedDataMarker.ToString(), StringComparison.Ordinal))) {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Determines if the given ctcp message is a request message.
    /// </summary>
    public static bool IsRequestMessage(String rawMessage) {
      return CtcpUtil.GetTransportCommand(rawMessage) == "PRIVMSG";
    }

    /// <summary>
    /// Determines if the given ctcp message is a reply message.
    /// </summary>
    public static bool IsReplyMessage(String rawMessage) {
      return CtcpUtil.GetTransportCommand(rawMessage) == "NOTICE";
    }
  }

}