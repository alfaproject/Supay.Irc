using System;
using System.Collections.Generic;
using System.Globalization;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which is invite
  ///   only.
  /// </summary>
  /// <remarks>
  ///   A channel can be set invite only with a <see cref="ChannelModeMessage" /> containing an
  ///   <see cref="InviteOnlyMode" />.
  /// </remarks>
  [Serializable]
  public class ChannelBlockedMessage : ErrorMessage, IChannelTargetedMessage
  {
    private string channel;
    private string reason;

    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelBlockedMessage" /> class.
    /// </summary>
    public ChannelBlockedMessage()
      : base(485)
    {
    }

    /// <summary>
    ///   Gets or sets the channel which is blocked
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

    /// <summary>
    ///   Gets or sets the reason the channel is blocked
    /// </summary>
    public string Reason
    {
      get
      {
        return this.reason;
      }
      set
      {
        this.reason = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <exclude />
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add(string.Format(CultureInfo.InvariantCulture, "Cannot join channel ({0})", this.Reason));
        return parameters;
      }
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Channel = string.Empty;
      this.Reason = string.Empty;

      if (parameters.Count > 2)
      {
        this.Channel = parameters[1];
        this.Reason = MessageUtil.StringBetweenStrings(parameters[2], "Cannot join channel (", ")");
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelBlocked(new IrcMessageEventArgs<ChannelBlockedMessage>(this));
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
