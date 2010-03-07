using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The message received informing the user of a channel's creation time.
  /// </summary>
  [Serializable]
  public class ChannelCreationTimeMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelCreationTimeMessage"/> class.
    /// </summary>
    public ChannelCreationTimeMessage()
      : base(329) {
    }

    /// <summary>
    /// Gets or sets the channel referred to.
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
    /// Gets or sets the time which the channel was created.
    /// </summary>
    public virtual DateTime TimeCreated {
      get {
        return timeCreated;
      }
      set {
        timeCreated = value;
      }
    }

    private string channel = string.Empty;
    private DateTime timeCreated = DateTime.MinValue;


    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(MessageUtil.ConvertToUnixTime(TimeCreated).ToString(CultureInfo.InvariantCulture));
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Channel = string.Empty;
      this.TimeCreated = DateTime.MinValue;

      if (parameters.Count > 2) {
        this.Channel = parameters[1];
        DateTime? unixTime = MessageUtil.ConvertFromUnixTime(parameters[2]);
        if (unixTime.HasValue) {
          this.TimeCreated = unixTime.Value;
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelCreationTime(new IrcMessageEventArgs<ChannelCreationTimeMessage>(this));
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