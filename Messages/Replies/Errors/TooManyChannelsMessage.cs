using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Sent to a user when they have joined the maximum number of allowed channels and they try to join another channel.
  /// </summary>
  [Serializable]
  public class TooManyChannelsMessage : ErrorMessage, IChannelTargetedMessage {
    private string channel = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="TooManyChannelsMessage" /> class.
    /// </summary>
    public TooManyChannelsMessage()
      : base(405) {
    }

    /// <summary>
    ///   The channel to which entry was denied.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
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
      parameters.Add("You have joined too many channels");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        Channel = parameters[1];
      } else {
        Channel = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnTooManyChannels(new IrcMessageEventArgs<TooManyChannelsMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
