using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Signifies the end of the MOTD sent by the server.
  /// </summary>
  [Serializable]
  public class MotdEndReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="MotdEndReplyMessage" /> class.
    /// </summary>
    public MotdEndReplyMessage()
      : base(376) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add("End of /MOTD command");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnMotdEndReply(new IrcMessageEventArgs<MotdEndReplyMessage>(this));
    }
  }
}
