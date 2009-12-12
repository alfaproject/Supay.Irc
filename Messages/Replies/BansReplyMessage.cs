using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Contains a Channel and BanId as one of possible many replies to a ban list request.
  /// </summary>
  [Serializable]
  public class BansReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="BansReplyMessage"/> class.
    /// </summary>
    public BansReplyMessage()
      : base() {
      this.InternalNumeric = 367;
    }

    /// <summary>
    /// Gets or sets the channel the ban list refers to.
    /// </summary>
    public virtual String Channel {
      get {
        return this.channel;
      }
      set {
        this.channel = value;
      }
    }

    /// <summary>
    /// Gets or sets the ban referenced.
    /// </summary>
    public virtual String BanId {
      get {
        return banId;
      }
      set {
        banId = value;
      }
    }

    private String channel = "";
    private String banId = "";

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
      writer.AddParameter(this.BanId);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 2) {
        this.Channel = parameters[1];
        this.BanId = parameters[2];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnBansReply(new IrcMessageEventArgs<BansReplyMessage>(this));
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
    }

    #endregion
  }

}