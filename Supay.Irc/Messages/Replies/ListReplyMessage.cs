using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A single reply to the <see cref="ListMessage" /> query.
  /// </summary>
  [Serializable]
  public class ListReplyMessage : NumericMessage, IChannelTargetedMessage
  {
    private string channel = string.Empty;
    private int memberCount = -1;
    private string topic = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ListReplyMessage" /> class.
    /// </summary>
    public ListReplyMessage()
      : base(322)
    {
    }

    /// <summary>
    ///   Gets or sets the channel for this reply.
    /// </summary>
    public virtual string Channel
    {
      get
      {
        return this.channel;
      }
      set
      {
        this.channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of people in the channel.
    /// </summary>
    public virtual int MemberCount
    {
      get
      {
        return this.memberCount;
      }
      set
      {
        this.memberCount = value;
      }
    }

    /// <summary>
    ///   Gets or sets the topic of the channel.
    /// </summary>
    public virtual string Topic
    {
      get
      {
        return this.topic;
      }
      set
      {
        this.topic = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add(this.MemberCount.ToString(CultureInfo.InvariantCulture));
        parameters.Add(this.Topic);
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      if (parameters.Count == 4)
      {
        this.Channel = parameters[1];
        this.MemberCount = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
        this.Topic = parameters[3];
      }
      else
      {
        this.Channel = string.Empty;
        this.MemberCount = -1;
        this.Topic = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnListReply(new IrcMessageEventArgs<ListReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
    }
  }
}
