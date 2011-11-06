using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a user tries to kill, kick, or de-op a bot which provides channel services.
  /// </summary>
  [Serializable]
  public class CannotRemoveServiceBotMessage : ErrorMessage {
    private string channel;
    private string nick;

    /// <summary>
    ///   Creates a new instances of the <see cref="CannotRemoveServiceBotMessage" /> class.
    /// </summary>
    public CannotRemoveServiceBotMessage()
      : base(484) {
    }

    /// <summary>
    ///   Gets or sets the nick of the bot
    /// </summary>
    public string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel on which the bot resides
    /// </summary>
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add(Channel);
      parameters.Add("Cannot kill, kick or de-op channel service");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Nick = string.Empty;
      Channel = string.Empty;
      if (parameters.Count > 2) {
        Nick = parameters[1];
        Channel = parameters[2];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnCannotRemoveServiceBot(new IrcMessageEventArgs<CannotRemoveServiceBotMessage>(this));
    }
  }
}
