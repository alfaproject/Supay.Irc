using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when attempting to join a channel which invite-only.
  /// </summary>
  /// <remarks>
  /// A channel can be set invite-only with a ChannelModeMessage containing a InviteOnlyMode.
  /// </remarks>
  [Serializable]
  public class ChannelIsInviteOnlyMessage : ErrorMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="ChannelIsInviteOnlyMessage"/> class.
    /// </summary>
    public ChannelIsInviteOnlyMessage()
      : base() {
      this.InternalNumeric = 473;
    }

    /// <summary>
    /// Gets or sets the channel which is invite-only
    /// </summary>
    public String Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private String channel;

    /// <exclude />
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
      writer.AddParameter("Cannot join channel (+i)");
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Channel = "";
      if (parameters.Count > 2) {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelIsInviteOnly(new IrcMessageEventArgs<ChannelIsInviteOnlyMessage>(this));
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    /// Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.IsIgnoreCaseMatch(this.Channel, channelName);
    }

    #endregion
  }

}