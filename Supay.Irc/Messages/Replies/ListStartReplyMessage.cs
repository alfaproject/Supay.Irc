using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Marks the start of the replies to the <see cref="ListMessage" /> query.
  /// </summary>
  [Serializable]
  public class ListStartReplyMessage : NumericMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="ListStartReplyMessage" /> class.
    /// </summary>
    public ListStartReplyMessage()
      : base(321)
    {
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add("Channel");
        parameters.Add("Users Name");
        return parameters;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnListStartReply(new IrcMessageEventArgs<ListStartReplyMessage>(this));
    }
  }
}
