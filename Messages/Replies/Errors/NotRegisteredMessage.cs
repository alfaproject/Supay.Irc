using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Returned by the server to indicate that the client must be registered before the server will allow it to be parsed in detail.
  /// </summary>
  [Serializable]
  public class NotRegisteredMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NotRegisteredMessage"/> class.
    /// </summary>
    public NotRegisteredMessage()
      : base() {
      this.InternalNumeric = 451;
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("You have not registered");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNotRegistered(new IrcMessageEventArgs<NotRegisteredMessage>(this));
    }

  }

}