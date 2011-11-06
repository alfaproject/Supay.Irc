using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="TopicMessage" /> is used to change or view the topic of a channel.
  /// </summary>
  [Serializable]
  public class TopicMessage : CommandMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="TopicMessage" /> class.
    /// </summary>
    public TopicMessage()
      : base() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="TopicMessage" /> class for the given channel and topic.
    /// </summary>
    /// <param name="channel">The channel to affect.</param>
    /// <param name="topic">The new topic to set.</param>
    public TopicMessage(string channel, string topic)
      : base() {
      this.channel = channel;
      this.topic = topic;
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "TOPIC";
      }
    }

    /// <summary>
    ///   Gets or sets the channel affected
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the new Topic to apply
    /// </summary>
    /// <remarks>
    ///   If Topic is blank, the server will send a <see cref="TopicReplyMessage" /> and probably a <see cref="TopicSetReplyMessage" />,
    ///   telling you what the current topic is, who set it, and when.
    /// </remarks>
    public virtual string Topic {
      get {
        return topic;
      }
      set {
        topic = value;
      }
    }

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      if (!string.IsNullOrEmpty(Topic)) {
        parameters.Add(Topic);
      }
      return parameters;
    }

    /// <summary>
    ///   Parse the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Channel = string.Empty;
      this.Topic = string.Empty;
      if (parameters.Count >= 1) {
        this.Channel = parameters[0];
        if (parameters.Count >= 2) {
          this.Topic = parameters[1];
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnTopic(new IrcMessageEventArgs<TopicMessage>(this));
    }

    private string channel = string.Empty;
    private string topic = string.Empty;

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
