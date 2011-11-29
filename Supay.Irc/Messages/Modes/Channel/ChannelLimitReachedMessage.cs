using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which has reached
  ///   its limit of users.
  /// </summary>
  /// <remarks>
  ///   A channel can set it's user limit with a <see cref="ChannelModeMessage" /> containing a
  ///   <see cref="LimitMode" />. Once that many users are in the channel, any other users
  ///   attempting to join will get this reply. On some networks, an invite allows an user to
  ///   bypass the limit.
  /// </remarks>
  [Serializable]
  public class ChannelLimitReachedMessage : ErrorMessage
  {
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelLimitReachedMessage" /> class.
    /// </summary>
    public ChannelLimitReachedMessage()
      : base(471)
    {
    }

    /// <summary>
    ///   Gets or sets the channel which has reached its limit
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

    /// <exclude />
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add("Cannot join channel (+l)");
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
      conduit.OnChannelLimitReached(new IrcMessageEventArgs<ChannelLimitReachedMessage>(this));
    }
  }
}
