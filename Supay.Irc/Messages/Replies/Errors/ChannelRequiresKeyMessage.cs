using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which has a key
  ///   set, and the user has not provided it.
  /// </summary>
  /// <remarks>
  ///   A channel can require a key with the <see cref="ChannelModeMessage" /> with a
  ///   <see cref="KeyMode" />. The key must be set on the <see cref="JoinMessage" /> to join such
  ///   channels.
  /// </remarks>
  [Serializable]
  public class ChannelRequiresKeyMessage : ErrorMessage, IChannelTargetedMessage
  {
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelRequiresKeyMessage" /> class.
    /// </summary>
    public ChannelRequiresKeyMessage()
      : base(475)
    {
    }

    /// <summary>
    ///   Gets or sets the channel which has a key
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
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add("Cannot join channel (+k)");
        return parameters;
      }
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
      conduit.OnChannelRequiresKey(new IrcMessageEventArgs<ChannelRequiresKeyMessage>(this));
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
