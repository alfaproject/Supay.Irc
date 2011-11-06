using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Marks the end of the replies to the <see cref="ListMessage" /> query.
  /// </summary>
  [Serializable]
  public class ListEndReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="ListEndReplyMessage" />.
    /// </summary>
    public ListEndReplyMessage()
      : base(323) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("End of /LIST");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnListEndReply(new IrcMessageEventArgs<ListEndReplyMessage>(this));
    }
  }
}
