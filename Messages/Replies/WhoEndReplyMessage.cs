using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Signals the end of the <see cref="WhoReplyMessage"/> list.
  /// </summary>
  [Serializable]
  public class WhoEndReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="WhoEndReplyMessage"/> class.
    /// </summary>
    public WhoEndReplyMessage()
      : base() {
      this.InternalNumeric = 315;
    }

    /// <summary>
    /// Gets or sets the nick of the user in the Who reply list.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }
    private string nick = "";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("End of /WHO list");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        this.Nick = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWhoEndReply(new IrcMessageEventArgs<WhoEndReplyMessage>(this));
    }

  }

}