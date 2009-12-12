using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when a user adds too many users to his Accept list.
  /// </summary>
  [Serializable]
  public class AcceptListFullMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="BanListFullMessage"/> class.
    /// </summary>
    public AcceptListFullMessage()
      : base() {
      this.InternalNumeric = 456;
    }

    /// <exclude />
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("Accept list is full");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnAcceptListFull(new IrcMessageEventArgs<AcceptListFullMessage>(this));
    }

  }

}