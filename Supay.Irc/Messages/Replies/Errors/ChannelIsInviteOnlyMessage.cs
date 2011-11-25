using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which is invite
  ///   only.
  /// </summary>
  /// <remarks>
  ///   A channel can be set invite-only with a <see cref="ChannelModeMessage" /> containing an
  ///   <see cref="InviteOnlyMode" />.
  /// </remarks>
  [Serializable]
  public class ChannelIsInviteOnlyMessage : ErrorMessage, IChannelTargetedMessage
  {
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelIsInviteOnlyMessage" /> class.
    /// </summary>
    public ChannelIsInviteOnlyMessage()
      : base(473)
    {
    }

    /// <summary>
    ///   Gets or sets the channel which is invite-only
    /// </summary>
    public string Channel
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

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <exclude />
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Channel);
      parameters.Add("Cannot join channel (+i)");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Channel = string.Empty;
      if (parameters.Count > 2)
      {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelIsInviteOnly(new IrcMessageEventArgs<ChannelIsInviteOnlyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.EqualsI(channelName);
    }
  }
}
