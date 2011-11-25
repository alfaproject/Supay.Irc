using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is sent when the client has been elevated to network operator status.
  /// </summary>
  [Serializable]
  public class OperReplyMessage : NumericMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="OperReplyMessage" /> class
    /// </summary>
    public OperReplyMessage()
      : base(381)
    {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add("You are now an IRC operator");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnOperReply(new IrcMessageEventArgs<OperReplyMessage>(this));
    }
  }
}
