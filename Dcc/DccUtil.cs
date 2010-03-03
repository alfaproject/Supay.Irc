using System.Collections.Specialized;
using Supay.Irc.Messages;

namespace Supay.Irc.Dcc {

  /// <summary>
  ///   Holds a few utilities for DCC message parsing. </summary>
  public static class DccUtil {

    /// <summary>
    ///   Gets the DCC Command of the message, such as CHAT or SEND. </summary>
    public static string GetCommand(string rawMessage) {
      return GetParameters(rawMessage)[0];
    }

    /// <summary>
    ///   Gets the DCC Argument of the message, such as the filename of a SEND. </summary>
    public static string GetArgument(string rawMessage) {
      return GetParameters(rawMessage)[1];
    }

    /// <summary>
    ///   Gets the address of the connection instantiator in 64-bits format as a string. </summary>
    public static string GetAddress(string rawMessage) {
      return GetParameters(rawMessage)[2];
    }

    /// <summary>
    ///   Gets the port of the connection instantiator. </summary>
    public static string GetPort(string rawMessage) {
      return GetParameters(rawMessage)[3];
    }

    /// <summary>
    ///   Gets the inner parameters of a DCC data area. </summary>
    public static StringCollection GetParameters(string rawMessage) {
      string extendedData = CtcpUtil.GetExtendedData(rawMessage);
      return MessageUtil.Tokenize(extendedData, 0);
    }

  } //class DccUtil
} //namespace Supay.Irc.Dcc