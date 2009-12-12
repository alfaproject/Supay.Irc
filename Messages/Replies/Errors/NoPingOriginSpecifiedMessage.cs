using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Sent to a user who sends a PingMessage which doesn't have a valid origin.
  /// </summary>
  [Serializable]
  public class NoPingOriginSpecifiedMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NoPingOriginSpecifiedMessage"/> class.
    /// </summary>
    public NoPingOriginSpecifiedMessage()
      : base() {
      this.InternalNumeric = 409;
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("No origin specified");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNoPingOriginSpecified(new IrcMessageEventArgs<NoPingOriginSpecifiedMessage>(this));
    }

  }

}