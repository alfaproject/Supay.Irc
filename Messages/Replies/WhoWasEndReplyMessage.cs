using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Signals the end of a reply to a <see cref="WhoWasMessage"/>.
  /// </summary>
  [Serializable]
  public class WhoWasEndReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="WhoWasEndReplyMessage"/> class.
    /// </summary>
    public WhoWasEndReplyMessage()
      : base() {
      this.InternalNumeric = 369;
    }

    /// <summary>
    /// Gets or sets the nick of the user being examined.
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
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("End of WHOWAS");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        this.Nick = parameters[1];
      } else {
        this.Nick = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWhoWasEndReply(new IrcMessageEventArgs<WhoWasEndReplyMessage>(this));
    }

  }

}