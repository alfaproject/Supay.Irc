using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The <see cref="ErrorMessage"/> received when attempting to join a channel on which the user is banned.
  /// </summary>
  [Serializable]
  public class YouAreBannedFromChannelMessage : ErrorMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="YouAreBannedFromChannelMessage"/> class.
    /// </summary>
    public YouAreBannedFromChannelMessage()
      : base(474) {
    }

    /// <summary>
    /// Gets or sets the channel on which the user is banned
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
      parameters.Add("Cannot join channel (+b)");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Channel = string.Empty;
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
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }

}