using System;
using System.Collections.Generic;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is a chat message which is scoped to the current channel.
  /// </summary>
  /// <remarks>
  ///   This is a non-standard message. This command exists because many servers limit the number
  ///   of standard chat messages you can send in a time frame. However, they will let channel
  ///   operators send this chat message as often as they want to people who are in that channel.
  /// </remarks>
  [Serializable]
  public class ChannelScopedChatMessage : CommandMessage, IChannelTargetedMessage
  {
    private string channel;
    private string target;
    private string text;

    #region Constructors

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage" /> class.
    /// </summary>
    public ChannelScopedChatMessage()
      : this(string.Empty)
    {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage" /> class with the given
    ///   text string.
    /// </summary>
    public ChannelScopedChatMessage(string text)
      : this(text, string.Empty)
    {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage" /> class with the given
    ///   text string and target channel or user.
    /// </summary>
    public ChannelScopedChatMessage(string text, string target)
    {
      this.Text = text;
      this.Target = target;
      this.channel = string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "CPRIVMSG";
      }
    }

    /// <summary>
    ///   Gets or sets the actual text of this message.
    /// </summary>
    public string Text
    {
      get
      {
        return this.text;
      }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }
        this.text = value;
      }
    }

    /// <summary>
    ///   Gets or sets the target of this message.
    /// </summary>
    public string Target
    {
      get
      {
        return this.target;
      }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }
        this.target = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel which this message is scoped to.
    /// </summary>
    public virtual string Channel
    {
      get
      {
        return this.channel;
      }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }
        this.channel = value;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Target);
      parameters.Add(this.Channel);
      parameters.Add(this.Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Target = parameters[0];
      this.Channel = parameters[1];
      this.Text = parameters[2];
    }

    /// <summary>
    ///   Validates this message against the given server support.
    /// </summary>
    public override void Validate(ServerSupport serverSupport)
    {
      base.Validate(serverSupport);
      MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
      if (string.IsNullOrEmpty(this.Target))
      {
        throw new InvalidMessageException(Resources.TargetcannotBeEmpty);
      }
      if (string.IsNullOrEmpty(this.Channel))
      {
        throw new InvalidMessageException(Resources.ChannelCannotBeEmpty);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelScopedChat(new IrcMessageEventArgs<ChannelScopedChatMessage>(this));
    }

    #endregion

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    public virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }
}
