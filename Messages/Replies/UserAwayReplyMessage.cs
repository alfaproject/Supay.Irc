using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// The message is received by a client from a server 
  /// when they attempt to send a message to a user who
  /// is marked as away using the <see cref="AwayMessage"/>.
  /// </summary>
  [Serializable]
  public class UserAwayMessage : NumericMessage {
    /// <summary>
    /// Creates a new instance of the <see cref="UserAwayMessage"/>.
    /// </summary>
    public UserAwayMessage()
      : base(301) {
    }

    /// <summary>
    /// Gets or sets the user's away message.
    /// </summary>
    public virtual string Text {
      get {
        return text;
      }
      set {
        text = value;
      }
    }

    /// <summary>
    /// Gets or sets the nick of the user who is away.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    private string text = string.Empty;
    private string nick = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 2) {
        this.Nick = parameters[1];
        this.Text = parameters[2];
      } else {
        this.Nick = string.Empty;
        this.Text = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUserAway(new IrcMessageEventArgs<UserAwayMessage>(this));
    }
  }
}
