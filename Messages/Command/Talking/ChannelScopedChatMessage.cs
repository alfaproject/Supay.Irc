using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   This message is a chat message which is scoped to the current channel. </summary>
  /// <remarks>
  ///   This is a non-standard message. This command exists because many servers limit the number
  ///   of standard chat messages you can send in a time frame. However, they will let channel
  ///   operators send this chat message as often as they want to people who are in that channel. </remarks>
  [Serializable]
  public class ChannelScopedChatMessage : CommandMessage, IChannelTargetedMessage {

    private string _text;
    private string _target;
    private string _channel;

    #region Constructors

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage"/> class. </summary>
    public ChannelScopedChatMessage()
      : this(string.Empty) {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage"/> class with the given
    ///   text string. </summary>
    public ChannelScopedChatMessage(string text)
      : this(text, string.Empty) {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelScopedChatMessage"/> class with the given
    ///   text string and target channel or user. </summary>
    public ChannelScopedChatMessage(string text, string target) {
      Text = text;
      Target = target;
      _channel = string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the IRC command associated with this message. </summary>
    protected override string Command {
      get {
        return "CPRIVMSG";
      }
    }

    /// <summary>
    ///   Gets or sets the actual text of this message. </summary>
    public string Text {
      get {
        return _text;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        _text = value;
      }
    }

    /// <summary>
    ///   Gets or sets the target of this message. </summary>
    public string Target {
      get {
        return _target;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        _target = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel which this message is scoped to. </summary>
    public virtual string Channel {
      get {
        return _channel;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        _channel = value;
      }
    }

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
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Target = parameters[0];
      Channel = parameters[1];
      Text = parameters[2];
    }

    /// <summary>
    ///   Validates this message against the given server support. </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      MessageUtil.EnsureValidChannelName(Channel, serverSupport);
      if (string.IsNullOrEmpty(Target)) {
        throw new InvalidMessageException(Properties.Resources.TargetcannotBeEmpty);
      }
      if (string.IsNullOrEmpty(Channel)) {
        throw new InvalidMessageException(Properties.Resources.ChannelCannotBeEmpty);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    ///   current <see cref="IrcMessage"/> subclass. </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelScopedChat(new IrcMessageEventArgs<ChannelScopedChatMessage>(this));
    }

    #endregion

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    public virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }

    #endregion

  } //class ChannelScopedChatMessage
} //namespace Supay.Irc.Messages