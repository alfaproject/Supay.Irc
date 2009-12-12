using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The base class for all ctcp request messages.
  /// </summary>
  [Serializable]
  public abstract class CtcpRequestMessage : CtcpMessage {
    /// <summary>
    /// Gets the irc command used to send the ctcp command to another user.
    /// </summary>
    /// <remarks>
    /// A request message uses the PRIVMSG command for transport.
    /// </remarks>
    protected override String TransportCommand {
      get {
        return "PRIVMSG";
      }
    }

    /// <summary>
    /// Gets the data payload of the Ctcp request.
    /// </summary>
    protected override String ExtendedData {
      get {
        return "";
      }
    }

  }

}