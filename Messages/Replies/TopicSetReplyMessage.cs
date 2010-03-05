using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The reply sent when the server acknowledges that a channel's topic has been changed.
  /// </summary>
  [Serializable]
  public class TopicSetReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="TopicSetReplyMessage"/> class.
    /// </summary>
    public TopicSetReplyMessage()
      : base() {
      this.InternalNumeric = 333;
    }

    /// <summary>
    /// Gets or sets the channel with the changed topic.
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
    /// Gets or sets the user which changed the topic.
    /// </summary>
    public virtual Supay.Irc.User User {
      get {
        return user;
      }
      set {
        user = value;
      }
    }
    private Supay.Irc.User user = new User();

    /// <summary>
    /// Gets or sets the time at which the topic was changed.
    /// </summary>
    public virtual DateTime TimeSet {
      get {
        return timeSet;
      }
      set {
        timeSet = value;
      }
    }
    private DateTime timeSet = DateTime.Now;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(User.IrcMask);
      parameters.Add(MessageUtil.ConvertToUnixTime(TimeSet).ToString(CultureInfo.InvariantCulture));
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Channel = parameters[1];
      this.User = new User(parameters[2]);
      this.TimeSet = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnTopicSetReply(new IrcMessageEventArgs<TopicSetReplyMessage>(this));
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