using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when attempting to join a channel on which the user is banned.
  /// </summary>
  [Serializable]
  public class YouAreBannedFromChannelMessage : ErrorMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="YouAreBannedFromChannelMessage"/> class.
    /// </summary>
    public YouAreBannedFromChannelMessage()
      : base() {
      this.InternalNumeric = 474;
    }

    /// <summary>
    /// Gets or sets the channel on which the user is banned
    /// </summary>
    public String Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private String channel;

    /// <exclude />
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
      writer.AddParameter("Cannot join channel (+b)");
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
      conduit.OnYouAreBannedFromChannel(new IrcMessageEventArgs<YouAreBannedFromChannelMessage>(this));
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    /// Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.IsIgnoreCaseMatch(this.Channel, channelName);
      ;
    }

    #endregion
  }

}