using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when attempting to join a channel which requires a registered nick, 
  /// and the user does not have one.
  /// </summary>
  /// <remarks>
  /// A channel can require a key with the ChannelModeMessage with a KeyMode.
  /// The key must be set on the JoinMessage to join such channels.
  /// </remarks>
  [Serializable]
  public class ChannelRequiresRegisteredNickMessage : ErrorMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="ChannelRequiresRegisteredNickMessage"/> class.
    /// </summary>
    public ChannelRequiresRegisteredNickMessage()
      : base() {
      this.InternalNumeric = 477;
    }

    /// <summary>
    /// Gets or sets the channel which has a key
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
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
      writer.AddParameter("You need a registered nick to join that channel.");
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Channel = "";
      if (parameters.Count > 2) {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelRequiresRegisteredNick(new IrcMessageEventArgs<ChannelRequiresRegisteredNickMessage>(this));
    }


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