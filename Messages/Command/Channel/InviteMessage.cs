using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The InviteMessage is used to invite users to a channel.
  /// </summary>
  /// <remarks>
  ///   This message wraps the INVITE command.
  /// </remarks>
  [Serializable]
  public class InviteMessage : CommandMessage, IChannelTargetedMessage {
    private string channel = string.Empty;
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="InviteMessage" /> class.
    /// </summary>
    public InviteMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="InviteMessage" /> class with the given channel and nick.
    /// </summary>
    /// <param name="channel">The channel the person is being invited into.</param>
    /// <param name="nick">The nick of the user invited</param>
    public InviteMessage(string channel, string nick) {
      this.channel = channel;
      this.nick = nick;
    }

    /// <summary>
    ///   Gets or sets the channel the person is being invited into.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the nick of the user invited
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "INVITE";
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      Channel = MessageUtil.EnsureValidChannelName(Channel, serverSupport);
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Nick);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 2) {
        Channel = parameters[0];
        Nick = parameters[1];
      } else {
        Channel = string.Empty;
        Nick = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnInvite(new IrcMessageEventArgs<InviteMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
