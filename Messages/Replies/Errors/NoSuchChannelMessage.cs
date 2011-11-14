using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Used to indicate the given channel name is invalid.
  /// </summary>
  [Serializable]
  public class NoSuchChannelMessage : ErrorMessage, IChannelTargetedMessage
  {
    private string _channel = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="NoSuchChannelMessage" /> class.
    /// </summary>
    public NoSuchChannelMessage()
      : base(403)
    {
    }

    /// <summary>
    ///   Gets or sets the Channel which was empty or didn't exist.
    /// </summary>
    public virtual string Channel
    {
      get
      {
        return this._channel;
      }
      set
      {
        this._channel = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Channel);
      parameters.Add("No such channel");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Channel = parameters.Count > 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNoSuchChannel(new IrcMessageEventArgs<NoSuchChannelMessage>(this));
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
