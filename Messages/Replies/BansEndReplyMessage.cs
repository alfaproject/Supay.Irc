using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Signifies the end of the reply listing Bans on a channel. </summary>
  [Serializable]
  public class BansEndReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    ///   Creates a new instances of the <see cref="BansEndReplyMessage"/> class. </summary>
    public BansEndReplyMessage()
      : base(368) {
      Channel = string.Empty;
    }

    /// <summary>
    ///   Gets or sets the channel the ban list refers to. </summary>
    public string Channel {
      get;
      set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("End of channel ban list");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        Channel = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    /// current <see cref="IrcMessage"/> subclass. </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnBansEndReply(new IrcMessageEventArgs<BansEndReplyMessage>(this));
    }

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    public virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }

    #endregion

  } //class BansEndReplyMessage
} //namespace Supay.Irc.Messages