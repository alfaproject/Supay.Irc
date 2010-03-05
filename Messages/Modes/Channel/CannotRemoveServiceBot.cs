using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when a user tries to kill, kick, or deop a bot which provides channel services.
  /// </summary>
  [Serializable]
  public class CannotRemoveServiceBotMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="CannotRemoveServiceBotMessage"/> class.
    /// </summary>
    public CannotRemoveServiceBotMessage()
      : base() {
      this.InternalNumeric = 484;
    }

    /// <summary>
    /// Gets or sets the nick of the bot
    /// </summary>
    public string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }
    private string nick;

    /// <summary>
    /// Gets or sets the channel on which the bot resides
    /// </summary>
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private string channel;


    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add(Channel);
      parameters.Add("Cannot kill, kick or de-op channel service");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Nick = "";
      this.Channel = "";
      if (parameters.Count > 2) {
        this.Nick = parameters[1];
        this.Channel = parameters[2];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnCannotRemoveServiceBot(new IrcMessageEventArgs<CannotRemoveServiceBotMessage>(this));
    }

  }

}