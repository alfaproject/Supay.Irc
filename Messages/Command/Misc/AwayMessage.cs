using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   With the AwayMessage, clients can set an automatic reply string for any <see cref="ChatMessage" />s directed at them (not to a channel they are on).
  /// </summary>
  /// <remarks>
  ///   The automatic reply is sent by the server to client sending the <see cref="ChatMessage" />.
  ///   The only replying server is the one to which the sending client is connected to.
  /// </remarks>
  [Serializable]
  public class AwayMessage : CommandMessage {
    /// <summary>
    ///   Creates a new instance of the AwayMessage class.
    /// </summary>
    public AwayMessage()
      : base() {
    }

    /// <summary>
    ///   Creates a new instance of the AwayMessage class with the given reason.
    /// </summary>
    public AwayMessage(string reason)
      : base() {
      this.reason = reason;
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "AWAY";
      }
    }

    /// <summary>
    ///   Gets or sets the reason for being away.
    /// </summary>
    public virtual string Reason {
      get {
        return reason;
      }
      set {
        reason = value;
      }
    }

    private string reason = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(string.IsNullOrEmpty(Reason) ? "away" : Reason);
      return parameters;
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      return (base.CanParse(unparsedMessage) && MessageUtil.GetParameters(unparsedMessage).Count > 0);
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 0) {
        Reason = parameters[0];
      } else {
        Reason = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnAway(new IrcMessageEventArgs<AwayMessage>(this));
    }
  }
}
