using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Signals the end of a <see cref="WhoIsMessage" /> reply.
  /// </summary>
  [Serializable]
  public class WhoIsEndReplyMessage : NumericMessage {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoIsEndReplyMessage" /> class.
    /// </summary>
    public WhoIsEndReplyMessage()
      : base(318) {
    }

    /// <summary>
    ///   Gets or sets the nick for the user examined.
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
      parameters.Add("End of /WHOIS list");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        Nick = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoIsEndReply(new IrcMessageEventArgs<WhoIsEndReplyMessage>(this));
    }
  }
}
