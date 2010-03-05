using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when a UserModeMessage was sent with a UserMode which the server didn't recognize.
  /// </summary>
  [Serializable]
  public class UnknownUserModeMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="UnknownUserModeMessage"/> class.
    /// </summary>
    public UnknownUserModeMessage()
      : base() {
      this.InternalNumeric = 501;
    }

    /// <exclude />
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("Unknown MODE flag");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUnknownUserMode(new IrcMessageEventArgs<UnknownUserModeMessage>(this));
    }

  }

}