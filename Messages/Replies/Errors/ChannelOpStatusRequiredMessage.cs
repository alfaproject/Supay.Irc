using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a user tries to perform a command which
  ///   requires channel-operator status.
  /// </summary>
  /// <remarks>
  ///   Channel-operator status is set with the <see cref="OperatorMode" /> of the
  ///   <see cref="ChannelModeMessage" />.
  /// </remarks>
  [Serializable]
  public class ChannelOperatorStatusRequiredMessage : ErrorMessage, IChannelTargetedMessage {
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelOperatorStatusRequiredMessage" /> class.
    /// </summary>
    public ChannelOperatorStatusRequiredMessage()
      : base(482) {
    }

    /// <summary>
    ///   Gets or sets the channel on which the command requires <see cref="OperatorMode" /> status.
    /// </summary>
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <exclude />
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("You're not channel operator");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Channel = string.Empty;
      if (parameters.Count > 1) {
        Channel = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelOperatorStatusRequired(new IrcMessageEventArgs<ChannelOperatorStatusRequiredMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
