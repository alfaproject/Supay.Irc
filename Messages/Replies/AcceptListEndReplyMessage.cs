using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   An Accept/CallerId system message marking the end of the responses to an
  ///   <see chref = "AcceptListRequestMessage" />.
  /// </summary>
  [Serializable]
  public class AcceptListEndReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="AcceptListEndReplyMessage" />.
    /// </summary>
    public AcceptListEndReplyMessage()
      : base(282) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add("End of /ACCEPT list.");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnAcceptListEndReply(new IrcMessageEventArgs<AcceptListEndReplyMessage>(this));
    }
  }
}
