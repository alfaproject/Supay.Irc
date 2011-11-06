using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Sent to a user who is either (a) not on a channel which is mode +n or (b) not a channel op (or mode +v) on a channel which has mode +m set or where the user is	banned and is trying to send a PRIVMSG message to	that channel.
  /// </summary>
  [Serializable]
  public class CannotSendToChannelMessage : ErrorMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="CannotSendToChannelMessage" /> class.
    /// </summary>
    public CannotSendToChannelMessage()
      : base(404) {
    }

    /// <summary>
    ///   The channel to which the message can't be sent.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("Cannot send to channel");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      } else {
        this.Channel = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnCannotSendToChannel(new IrcMessageEventArgs<CannotSendToChannelMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }
}
