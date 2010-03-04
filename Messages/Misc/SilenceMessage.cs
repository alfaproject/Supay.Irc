using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// With the SilenceMessage, clients can tell a server to never send messages to them from a given user. This, effectively, is a serverside ignore command.
  /// </summary>
  [Serializable]
  public class SilenceMessage : CommandMessage {

    /// <summary>
    /// Creates a new instance of the SilenceMessage class.
    /// </summary>
    public SilenceMessage()
      : base() {
    }

    /// <summary>
    /// Creates a new instance of the SilenceMessage class with the <see cref="User"/>.
    /// </summary>
    public SilenceMessage(User silencedUser)
      : base() {
      this.silencedUser = silencedUser;
    }

    /// <summary>
    /// Creates a new instance of the SilenceMessage class with the given mask.
    /// </summary>
    public SilenceMessage(string userMask)
      : this(new User(userMask)) {
    }

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "SILENCE";
      }
    }

    /// <summary>
    /// Gets or sets the user being silenced.
    /// </summary>
    public virtual User SilencedUser {
      get {
        return this.silencedUser;
      }
      set {
        this.silencedUser = value;
      }
    }
    private User silencedUser = new User();

    /// <summary>
    /// Gets or sets the action being applied to the silenced user on the list.
    /// </summary>
    public virtual ModeAction Action {
      get {
        return _action;
      }
      set {
        _action = value;
      }
    }
    private ModeAction _action = ModeAction.Add;

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      // SILENCE [{{+|-}<user>@<host>}]
      base.AddParametersToFormat(writer);
      if (this.SilencedUser != null && this.SilencedUser.ToString().Length != 0) {
        writer.AddParameter(this.Action.ToString() + this.SilencedUser.ToString());
      }
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 0) {
        string target = parameters[0];
        switch (target[0]) {
          case '+':
            Action = ModeAction.Add;
            target = target.Substring(1);
            break;
          case '-':
            Action = ModeAction.Remove;
            target = target.Substring(1);
            break;
          default:
            Action = ModeAction.Add;
            break;
        }
        this.SilencedUser = new User(target);
      } else {
        this.SilencedUser = new User();
        this.Action = ModeAction.Add;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnSilence(new IrcMessageEventArgs<SilenceMessage>(this));
    }

  }

}