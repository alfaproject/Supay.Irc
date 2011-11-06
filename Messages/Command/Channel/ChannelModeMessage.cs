using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The ChannelModeMessage allows channels to have their mode changed.
  /// </summary>
  /// <remarks>
  ///   Modes include such things as channel user limits and passwords, as well as the bans list and settings ops.
  ///   This message wraps the MODE command.
  /// </remarks>
  [Serializable]
  public class ChannelModeMessage : CommandMessage, IChannelTargetedMessage {
    private readonly List<string> modeArguments = new List<string>();
    private string channel = string.Empty;
    private string modeChanges = string.Empty;

    /// <summary>
    ///   Creates a new instance of the ChannelModeMessage class.
    /// </summary>
    public ChannelModeMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the ChannelModeMessage class and applies the given parameters.
    /// </summary>
    /// <param name="channel">The name of the channel being affected.</param>
    /// <param name="modeChanges">The mode changes being applied.</param>
    /// <param name="modeArguments">The arguments ( parameters ) for the <see cref="ChannelModeMessage.ModeChanges" /> property.</param>
    public ChannelModeMessage(string channel, string modeChanges, params string[] modeArguments) {
      this.channel = channel;
      this.modeChanges = modeChanges;
      this.modeArguments.AddRange(modeArguments);
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "MODE";
      }
    }

    /// <summary>
    ///   Gets or sets the name of the channel being affected.
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
    ///   Gets or sets the mode changes being applied.
    /// </summary>
    /// <remarks>
    ///   An example ModeChanges might look like "+ool".
    ///   This means adding the channel op mode for two users, and setting a limit on the user count.
    /// </remarks>
    public virtual string ModeChanges {
      get {
        return modeChanges;
      }
      set {
        modeChanges = value;
      }
    }

    /// <summary>
    ///   Gets the collection of arguments ( parameters ) for the <see cref="ChannelModeMessage.ModeChanges" /> property.
    /// </summary>
    /// <remarks>
    ///   Some modes require a parameter, such as +o requires the mask of the person to be given ops.
    /// </remarks>
    public virtual List<string> ModeArguments {
      get {
        return modeArguments;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      Collection<string> p = MessageUtil.GetParameters(unparsedMessage);
      if (p.Count >= 1) {
        return MessageUtil.HasValidChannelPrefix(p[0]);
      } else {
        return false;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Channel = parameters[0];

      if (parameters.Count > 1) {
        ModeChanges = parameters[1];
      } else {
        ModeChanges = string.Empty;
      }

      ModeArguments.Clear();
      if (parameters.Count > 2) {
        for (int i = 2; i < parameters.Count; i++) {
          ModeArguments.Add(parameters[i]);
        }
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      if (!string.IsNullOrEmpty(ModeChanges)) {
        parameters.Add(ModeChanges);
        foreach (string modeArgument in ModeArguments) {
          parameters.Add(modeArgument);
        }
      }
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelMode(new IrcMessageEventArgs<ChannelModeMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
