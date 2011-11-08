using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Reply to a <see cref="WhoIsMessage" />, stating the channels a user is in.
  /// </summary>
  [Serializable]
  public class WhoIsChannelsReplyMessage : NumericMessage, IChannelTargetedMessage {
    private readonly List<string> channels = new List<string>();
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoIsChannelsReplyMessage" /> class.
    /// </summary>
    public WhoIsChannelsReplyMessage()
      : base(319) {
    }

    /// <summary>
    ///   Gets or sets the Nick of the user being
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
    ///   Gets the collection of channels the user is a member of.
    /// </summary>
    public virtual List<string> Channels {
      get {
        return channels;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      if (Channels.Count != 0) {
        parameters.Add(MessageUtil.CreateList(Channels, " "));
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Nick = string.Empty;
      Channels.Clear();

      if (parameters.Count == 3) {
        Nick = parameters[1];
        Channels.AddRange(parameters[2].Split(' '));
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoIsChannelsReply(new IrcMessageEventArgs<WhoIsChannelsReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.ContainsIgnoreCaseMatch(Channels, channelName);
    }
  }
}
