using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message is a notice message which is scoped to the current channel.
  /// </summary>
  /// <remarks>
  /// This is a non-standard message.
  /// This command exists because many servers limit the number of standard notice messages
  /// you can send in a time frame. However, they will let channel operators send this notice message
  /// as often as they want to people who are in that channel.
  /// </remarks>
  [Serializable]
  public class ChannelScopedNoticeMessage : CommandMessage, IChannelTargetedMessage {

    #region Constructors

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelScopedNoticeMessage"/> class.
    /// </summary>
    public ChannelScopedNoticeMessage()
      : base() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelScopedNoticeMessage"/> class with the given text string.
    /// </summary>
    public ChannelScopedNoticeMessage(string text)
      : base() {
      this.Text = text;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelScopedNoticeMessage"/> class with the given text string and target channel or user.
    /// </summary>
    public ChannelScopedNoticeMessage(string text, string target)
      : this(text) {
      this.Target = target;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "CNOTICE";
      }
    }

    /// <summary>
    /// Gets or sets the actual text of this message.
    /// </summary>
    public virtual string Text {
      get {
        return this.text;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        this.text = value;
      }
    }
    private string text = string.Empty;

    /// <summary>
    /// Gets or sets the target of this message.
    /// </summary>
    public virtual string Target {
      get {
        return this.target;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        target = value;
      }
    }
    private string target = string.Empty;

    /// <summary>
    /// Gets or sets the channel which this message is scoped to.
    /// </summary>
    public virtual string Channel {
      get {
        return this.channel;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        channel = value;
      }
    }
    private string channel = string.Empty;

    #endregion

    #region Methods

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Target);
      parameters.Add(Channel);
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      if (parameters.Count == 3) {
        this.Target = parameters[0];
        this.Channel = parameters[1];
        this.Text = parameters[2];
      } else {
        this.Target = string.Empty;
        this.Channel = string.Empty;
        this.Text = string.Empty;
      }
    }

    /// <summary>
    /// Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
      if (string.IsNullOrEmpty(this.Target)) {
        throw new InvalidMessageException(Properties.Resources.TargetcannotBeEmpty);
      }
      if (string.IsNullOrEmpty(this.Channel)) {
        throw new InvalidMessageException(Properties.Resources.ChannelCannotBeEmpty);
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelScopedNotice(new IrcMessageEventArgs<ChannelScopedNoticeMessage>(this));
    }

    #endregion

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