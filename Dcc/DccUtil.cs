using System;
using System.Collections.Specialized;

namespace Supay.Irc.Dcc {

  /// <summary>
  /// Holds a few utilities for dcc message parsing.
  /// </summary>
  public sealed class DccUtil {

    /// <summary>
    /// Do not use under penalty of death
    /// </summary>
    private DccUtil() {
    }

    /// <summary>
    /// Gets the Dcc Command of the message, such as CHAT or SEND.
    /// </summary>
    public static String GetCommand(String rawMessage) {
      return GetParameters(rawMessage)[0];
    }

    /// <summary>
    /// Gets the Dcc Argument of the message, such as the filename of a SEND.
    /// </summary>
    public static String GetArgument(String rawMessage) {
      return GetParameters(rawMessage)[1];
    }

    /// <summary>
    /// Gets the address of the connection instantiator in 64-bits format as a String.
    /// </summary>
    public static String GetAddress(String rawMessage) {
      return GetParameters(rawMessage)[2];
    }

    /// <summary>
    /// Gets the port of the connection instantiator.
    /// </summary>
    public static String GetPort(String rawMessage) {
      return GetParameters(rawMessage)[3];
    }

    /// <summary>
    /// Gets the inner parameters of a dcc data area.
    /// </summary>
    public static StringCollection GetParameters(String rawMessage) {
      String extendedData = Supay.Irc.Messages.CtcpUtil.GetExtendedData(rawMessage);
      return Supay.Irc.Messages.MessageUtil.Tokenize(extendedData, 0);
    }

  }

}