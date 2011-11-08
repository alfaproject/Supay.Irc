using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> sent when a user tries to change his nick while on a channel in which he is banned.
  /// </summary>
  /// <remarks>
  ///   This is error code is also defined as "Resource Unavailable", but this message variant is more common.
  /// </remarks>
  [Serializable]
  public class CannotChangeNickWhileBannedMessage : ErrorMessage, IChannelTargetedMessage {
    private string channel;

    /// <summary>
    ///   Creates a new instances of the <see cref="TooManyLinesMessage" /> class.
    /// </summary>
    public CannotChangeNickWhileBannedMessage()
      : base(437) {
    }

    /// <summary>
    ///   Gets or sets the channel in which the user is banned.
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
      parameters.Add("Cannot change nickname while banned on channel");
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
      conduit.OnCannotChangeNickWhileBanned(new IrcMessageEventArgs<CannotChangeNickWhileBannedMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
