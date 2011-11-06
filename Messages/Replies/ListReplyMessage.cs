using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A single reply to the <see cref="ListMessage" /> query.
  /// </summary>
  [Serializable]
  public class ListReplyMessage : NumericMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="ListReplyMessage" /> class.
    /// </summary>
    public ListReplyMessage()
      : base(322) {
    }

    /// <summary>
    ///   Gets or sets the channel for this reply.
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
    ///   Gets or sets the number of people in the channel.
    /// </summary>
    public virtual int MemberCount {
      get {
        return memberCount;
      }
      set {
        memberCount = value;
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

    private string topic = string.Empty;
    private int memberCount = -1;
    private string channel = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(MemberCount.ToString(CultureInfo.InvariantCulture));
      parameters.Add(Topic);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 4) {
        this.Channel = parameters[1];
        this.MemberCount = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
        this.Topic = parameters[3];
      } else {
        this.Channel = string.Empty;
        this.MemberCount = -1;
        this.Topic = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnListReply(new IrcMessageEventArgs<ListReplyMessage>(this));
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
