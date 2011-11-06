using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which is invite
  ///   only.
  /// </summary>
  /// <remarks>
  ///   A channel can be set invite only with a <see cref="ChannelModeMessage" /> containing an
  ///   <see cref="InviteOnlyMode" />.
  /// </remarks>
  [Serializable]
  public class ChannelBlockedMessage : ErrorMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelBlockedMessage" /> class.
    /// </summary>
    public ChannelBlockedMessage()
      : base(485) {
    }

    /// <summary>
    ///   Gets or sets the channel which is blocked
    /// </summary>
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel;

    /// <summary>
    ///   Gets or sets the reason the channel is blocked
    /// </summary>
    public string Reason {
      get {
        return reason;
      }
      set {
        reason = value;
      }
    }

    private string reason;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(string.Format(CultureInfo.InvariantCulture, "Cannot join channel ({0})", Reason));
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Channel = string.Empty;
      Reason = string.Empty;

      if (parameters.Count > 2) {
        Channel = parameters[1];
        Reason = MessageUtil.StringBetweenStrings(parameters[2], "Cannot join channel (", ")");
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelBlocked(new IrcMessageEventArgs<ChannelBlockedMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }

    #endregion
  }
}
