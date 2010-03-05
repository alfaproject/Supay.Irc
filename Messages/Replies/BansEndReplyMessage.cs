using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Signifies the end of the reply listing Bans on a channel.
  /// </summary>
  [Serializable]
  public class BansEndReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="BansEndReplyMessage"/> class.
    /// </summary>
    public BansEndReplyMessage()
      : base() {
      this.InternalNumeric = 368;
    }

    /// <summary>
    /// Gets or sets the channel the ban list refers to.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel = "";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("End of channel ban list");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnBansEndReply(new IrcMessageEventArgs<BansEndReplyMessage>(this));
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