using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A single reply to the <see cref="ListMessage" /> query.
  /// </summary>
  [Serializable]
  public class ListReplyMessage : NumericMessage, IChannelTargetedMessage {
    private string channel = string.Empty;
    private int memberCount = -1;
    private string topic = string.Empty;

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
      parameters.Add(MemberCount.ToString(CultureInfo.InvariantCulture));
      parameters.Add(Topic);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 4) {
        Channel = parameters[1];
        MemberCount = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
        Topic = parameters[3];
      } else {
        Channel = string.Empty;
        MemberCount = -1;
        Topic = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnListReply(new IrcMessageEventArgs<ListReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
