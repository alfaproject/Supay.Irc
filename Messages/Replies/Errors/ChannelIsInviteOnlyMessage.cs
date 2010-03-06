using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   The <see cref="ErrorMessage"/> received when attempting to join a channel which is invite
  ///   only. </summary>
  /// <remarks>
  ///   A channel can be set invite-only with a <see cref="ChannelModeMessage"/> containing an
  ///   <see cref="InviteOnlyMode"/>. </remarks>
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
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private string channel;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("Cannot join channel (+i)");
      return parameters;
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
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }

}