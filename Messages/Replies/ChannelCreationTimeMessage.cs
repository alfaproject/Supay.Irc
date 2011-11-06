using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The message received informing the user of a channel's creation time.
  /// </summary>
  [Serializable]
  public class ChannelCreationTimeMessage : NumericMessage, IChannelTargetedMessage {
    private string channel = string.Empty;
    private DateTime timeCreated = DateTime.MinValue;

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelCreationTimeMessage" /> class.
    /// </summary>
    public ChannelCreationTimeMessage()
      : base(329) {
    }

    /// <summary>
    ///   Gets or sets the channel referred to.
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
    ///   Gets or sets the time which the channel was created.
    /// </summary>
    public virtual DateTime TimeCreated {
      get {
        return timeCreated;
      }
      set {
        timeCreated = value;
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
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(MessageUtil.ConvertToUnixTime(TimeCreated).ToString(CultureInfo.InvariantCulture));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Channel = string.Empty;
      TimeCreated = DateTime.MinValue;

      if (parameters.Count > 2) {
        Channel = parameters[1];
        DateTime? unixTime = MessageUtil.ConvertFromUnixTime(parameters[2]);
        if (unixTime.HasValue) {
          TimeCreated = unixTime.Value;
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelCreationTime(new IrcMessageEventArgs<ChannelCreationTimeMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
