using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This is the reply to an empty <see cref="ChannelModeMessage" />.
  /// </summary>
  [Serializable]
  public class ChannelModeIsReplyMessage : NumericMessage, IChannelTargetedMessage
  {
    private readonly IList<string> modeArguments = new List<string>();
    private string channel = string.Empty;
    private string modes = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelModeIsReplyMessage" /> class.
    /// </summary>
    public ChannelModeIsReplyMessage()
      : base(324)
    {
    }

    /// <summary>
    ///   Gets or sets the channel referred to.
    /// </summary>
    public virtual string Channel
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
    ///   Gets or sets the modes in effect.
    /// </summary>
    /// <remarks>
    ///   An example Modes might look like "+ml".
    /// </remarks>
    public virtual string Modes
    {
      get
      {
        return this.modes;
      }
      set
      {
        this.modes = value;
      }
    }

    /// <summary>
    ///   Gets the collection of arguments ( parameters ) for the <see cref="ChannelModeIsReplyMessage.Modes" /> property.
    /// </summary>
    /// <remarks>
    ///   Some modes require a parameter, such as +l ( user limit ) requires the number being limited to.
    /// </remarks>
    public virtual IList<string> ModeArguments
    {
      get
      {
        return this.modeArguments;
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
      parameters.Add(this.Modes);
      if (this.ModeArguments.Count != 0)
      {
        parameters.Add(string.Join(" ", this.ModeArguments));
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.ModeArguments.Clear();
      if (parameters.Count > 2)
      {
        this.Channel = parameters[1];
        this.Modes = parameters[2];
        for (int i = 3; i < parameters.Count; i++)
        {
          this.ModeArguments.Add(parameters[i]);
        }
      }
      else
      {
        this.Channel = string.Empty;
        this.Modes = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelModeIsReply(new IrcMessageEventArgs<ChannelModeIsReplyMessage>(this));
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
