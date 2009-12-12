using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// An Accept/CallerId system message marking the end of the responses to an AcceptListRequestMessage.
  /// </summary>
  [Serializable]
  public class AcceptListEndReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="AcceptListEndReplyMessage"/>.
    /// </summary>
    public AcceptListEndReplyMessage()
      : base() {
      this.InternalNumeric = 282;
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("End of /ACCEPT list.");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnAcceptListEndReply(new IrcMessageEventArgs<AcceptListEndReplyMessage>(this));
    }

  }

}