using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The KickMessage can be used to forcibly remove a user from a channel.
  ///   It 'kicks them out' of the channel.
  /// </summary>
  /// <remarks>
  ///   Only a channel operator may kick another user out of a channel.
  ///   This message wraps the KICK message.
  /// </remarks>
  [Serializable]
  public class KickMessage : CommandMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="KickMessage" /> class.
    /// </summary>
    public KickMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="KickMessage" /> class with the given channel and nick.
    /// </summary>
    /// <param name="channel">The name of the channel affected.</param>
    /// <param name="nick">The nick of the user being kicked out.</param>
    public KickMessage(string channel, string nick) {
      channels.Add(channel);
      nicks.Add(nick);
    }

    private List<string> channels = new List<string>();
    private List<string> nicks = new List<string>();
    private string reason = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "KICK";
      }
    }

    /// <summary>
    ///   Gets the channels affected.
    /// </summary>
    public virtual List<string> Channels {
      get {
        return channels;
      }
    }

    /// <summary>
    ///   Gets the nicks of the users being kicked.
    /// </summary>
    public virtual List<string> Nicks {
      get {
        return nicks;
      }
    }

    /// <summary>
    ///   Gets or sets the reason for the kick
    /// </summary>
    public virtual string Reason {
      get {
        return reason;
      }
      set {
        reason = value;
      }
    }

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport == null) {
        return;
      }
      if (Reason.Length > serverSupport.MaxKickCommentLength) {
        Reason = Reason.Substring(0, serverSupport.MaxKickCommentLength);
      }
      for (int i = 0; i < Channels.Count; i++) {
        Channels[i] = MessageUtil.EnsureValidChannelName(Channels[i], serverSupport);
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Channels, ","));
      parameters.Add(MessageUtil.CreateList(Nicks, ","));
      if (!string.IsNullOrEmpty(Reason)) {
        parameters.Add(Reason);
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Channels.Clear();
      Nicks.Clear();
      Reason = string.Empty;
      if (parameters.Count >= 2) {
        Channels.AddRange(parameters[0].Split(','));
        Nicks.AddRange(parameters[1].Split(','));
        if (parameters.Count >= 3) {
          Reason = parameters[2];
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnKick(new IrcMessageEventArgs<KickMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.ContainsIgnoreCaseMatch(Channels, channelName);
    }

    #endregion
  }
}
