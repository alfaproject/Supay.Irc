using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when attempting to join a channel which requires a
  ///   registered nick, and the user does not have one.
  /// </summary>
  [Serializable]
  public class ChannelRequiresRegisteredNickMessage : ErrorMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="ChannelRequiresRegisteredNickMessage" /> class.
    /// </summary>
    public ChannelRequiresRegisteredNickMessage()
      : base(477) {
    }

    /// <summary>
    ///   Gets or sets the channel which has a key
    /// </summary>
    public string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("You need a registered nick to join that channel.");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Channel = string.Empty;
      if (parameters.Count > 2) {
        Channel = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelRequiresRegisteredNick(new IrcMessageEventArgs<ChannelRequiresRegisteredNickMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }

    #endregion
  }
}
