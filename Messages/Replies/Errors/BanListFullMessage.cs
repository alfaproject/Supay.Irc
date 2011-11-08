using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a user tries to perform a channel-specific operation on a user, 
  ///   and the user isn't in the channel.
  /// </summary>
  /// <remarks>
  ///   Although all networks have a limit on the total number of bans allowed, 
  ///   not all networks will tell you when the list is full. 
  ///   (they will simply ignore extra bans.)
  /// </remarks>
  [Serializable]
  public class BanListFullMessage : ErrorMessage, IChannelTargetedMessage {
    private Mask banMask;
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="BanListFullMessage" /> class.
    /// </summary>
    public BanListFullMessage()
      : base(478) {
    }

    /// <summary>
    ///   Gets or sets the mask of the user being banned.
    /// </summary>
    public Mask BanMask {
      get {
        return banMask;
      }
      set {
        banMask = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel being targeted
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
      parameters.Add(BanMask.ToString());
      parameters.Add("Channel ban/ignore list is full");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 2) {
        Channel = parameters[1];
        BanMask = new Mask(parameters[2]);
      } else {
        Channel = string.Empty;
        BanMask = new User();
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnBanListFull(new IrcMessageEventArgs<BanListFullMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
