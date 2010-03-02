using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Sent to a user who is either (a) not on a channel which is mode +n or (b) not a chanop (or mode +v) on a channel which has mode +m set or where the user is	banned and is trying to send a PRIVMSG message to	that channel.
  /// </summary>
  [Serializable]
  public class CannotSendToChannelMessage : ErrorMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="CannotSendToChannelMessage"/> class.
    /// </summary>
    public CannotSendToChannelMessage()
      : base() {
      this.InternalNumeric = 404;
    }

    /// <summary>
    /// The channel to which the message can't be sent.
    /// </summary>
    public virtual String Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private String channel = "";

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
      writer.AddParameter("Cannot send to channel");
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      } else {
        this.Channel = "";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnCannotSendToChannel(new IrcMessageEventArgs<CannotSendToChannelMessage>(this));
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }

}