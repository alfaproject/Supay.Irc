using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The reply sent when the server acknowledges that a channel's topic has been changed.
  /// </summary>
  [Serializable]
  public class TopicSetReplyMessage : NumericMessage, IChannelTargetedMessage
  {
    private string channel = string.Empty;
    private DateTime timeSet = DateTime.Now;
    private User user = new User();

    /// <summary>
    ///   Creates a new instance of the <see cref="TopicSetReplyMessage" /> class.
    /// </summary>
    public TopicSetReplyMessage()
      : base(333)
    {
    }

    /// <summary>
    ///   Gets or sets the channel with the changed topic.
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
    ///   Gets or sets the user which changed the topic.
    /// </summary>
    public virtual User User
    {
      get
      {
        return this.user;
      }
      set
      {
        this.user = value;
      }
    }

    /// <summary>
    ///   Gets or sets the time at which the topic was changed.
    /// </summary>
    public virtual DateTime TimeSet
    {
      get
      {
        return this.timeSet;
      }
      set
      {
        this.timeSet = value;
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
        parameters.Add(this.User.IrcMask);
        parameters.Add(MessageUtil.ConvertToUnixTime(this.TimeSet).ToString(CultureInfo.InvariantCulture));
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Channel = parameters[1];
      this.User = new User(parameters[2]);
      this.TimeSet = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture));
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnTopicSetReply(new IrcMessageEventArgs<TopicSetReplyMessage>(this));
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
