using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// Marks the start of the replies to the <see cref="ListMessage"/> query.
  /// </summary>
  [Serializable]
  public class ListStartReplyMessage : NumericMessage {
    /// <summary>
    /// Creates a new instance of the <see cref="ListStartReplyMessage"/> class.
    /// </summary>
    public ListStartReplyMessage()
      : base(321) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("Channel");
      parameters.Add("Users Name");
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnListStartReply(new IrcMessageEventArgs<ListStartReplyMessage>(this));
    }
  }
}
