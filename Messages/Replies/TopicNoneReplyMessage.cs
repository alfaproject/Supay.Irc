using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The reply to a <see cref="TopicMessage"/> which requests the topic, 
  /// and there is none set.
  /// </summary>
  [Serializable]
  public class TopicNoneReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="TopicNoneReplyMessage"/> class.
    /// </summary>
    public TopicNoneReplyMessage()
      : base() {
      this.InternalNumeric = 331;
    }

    /// <summary>
    /// The name of the channel which has no topic set.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel = "";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("No topic is set");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 2) {
        this.Channel = parameters[1];
      } else {
        this.Channel = "";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnTopicNoneReply(new IrcMessageEventArgs<TopicNoneReplyMessage>(this));
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