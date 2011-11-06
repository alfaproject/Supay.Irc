using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   One of the possible replies to a <see cref="WhoIsMessage" /> message.
  /// </summary>
  [Serializable]
  public class WhoIsRegisteredNickReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="WhoIsRegisteredNickReplyMessage" /> class.
    /// </summary>
    public WhoIsRegisteredNickReplyMessage()
      : base(307) {
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

    private string nick = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("has identified for this nick");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        Nick = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoIsRegisteredNickReply(new IrcMessageEventArgs<WhoIsRegisteredNickReplyMessage>(this));
    }
  }
}
