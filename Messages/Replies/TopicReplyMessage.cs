using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The reply for the <see cref="TopicMessage" />.
  /// </summary>
  [Serializable]
  public class TopicReplyMessage : NumericMessage, IChannelTargetedMessage {
    private string channel = string.Empty;
    private string topic = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="TopicReplyMessage" /> class.
    /// </summary>
    public TopicReplyMessage()
      : base(332) {
    }

    /// <summary>
    ///   Gets or sets the channel referenced.
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
    ///   Gets or sets the topic of the channel.
    /// </summary>
    public virtual string Topic {
      get {
        return topic;
      }
      set {
        topic = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Topic);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 2) {
        Channel = parameters[1];
        Topic = parameters[2];
      } else {
        Channel = string.Empty;
        Topic = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnTopicReply(new IrcMessageEventArgs<TopicReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
