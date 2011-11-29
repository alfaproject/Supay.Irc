using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a user tries to perform a channel-specific operation on a user, 
  ///   and the user isn't in the channel.
  /// </summary>
  /// <remarks>
  ///   You will often get this if you attempt to kick a user but someone else kicks them before you do. 
  ///   If the user does not actually exist at all, 401 will be returned instead.
  /// </remarks>
  [Serializable]
  public class NotOnChannelMessage : ErrorMessage, IChannelTargetedMessage
  {
    private string channel;
    private string nick;

    /// <summary>
    ///   Creates a new instances of the <see cref="NotOnChannelMessage" /> class.
    /// </summary>
    public NotOnChannelMessage()
      : base(441)
    {
    }

    /// <summary>
    ///   Gets or sets the nick of the user being targeted
    /// </summary>
    public string Nick
    {
      get
      {
        return this.nick;
      }
      set
      {
        this.nick = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel being targeted
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
        parameters.Add(this.Nick);
        parameters.Add(this.Channel);
        parameters.Add("They aren't on that channel");
        return parameters;
      }
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Nick = string.Empty;
      this.Channel = string.Empty;
      if (parameters.Count > 2)
      {
        this.Nick = parameters[1];
        this.Channel = parameters[2];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNotOnChannel(new IrcMessageEventArgs<NotOnChannelMessage>(this));
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
