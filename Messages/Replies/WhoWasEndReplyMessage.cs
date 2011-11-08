using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Signals the end of a reply to a <see cref="WhoWasMessage" />.
  /// </summary>
  [Serializable]
  public class WhoWasEndReplyMessage : NumericMessage {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoWasEndReplyMessage" /> class.
    /// </summary>
    public WhoWasEndReplyMessage()
      : base(369) {
    }

    /// <summary>
    ///   Gets or sets the nick of the user being examined.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("End of WHOWAS");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Nick = parameters.Count == 3 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoWasEndReply(new IrcMessageEventArgs<WhoWasEndReplyMessage>(this));
    }
  }
}
