using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   With the WhisperMessage, clients can send messages to people within the context of a channel.
  /// </summary>
  [Serializable]
  public class WhisperMessage : CommandMessage, IChannelTargetedMessage {
    private readonly List<string> targets = new List<string>();
    private string channel = string.Empty;
    private string text = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "WHISPER";
      }
    }

    /// <summary>
    ///   Gets or sets the channel being targeted.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <summary>
    ///   Gets the target of this <see cref="TextMessage" />.
    /// </summary>
    public virtual List<string> Targets {
      get {
        return targets;
      }
    }

    /// <summary>
    ///   Gets or sets the actual text of this <see cref="TextMessage" />.
    /// </summary>
    /// <remarks>
    ///   This property holds the core purpose of IRC itself... sending text communication to others.
    /// </remarks>
    public virtual string Text {
      get {
        return text;
      }
      set {
        text = value;
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
      parameters.Add(Channel);
      parameters.Add(MessageUtil.CreateList(Targets, ","));
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Targets.Clear();
      if (parameters.Count > 2) {
        Channel = parameters[0];
        Targets.AddRange(parameters[1].Split(','));
        Text = parameters[2];
      } else {
        Channel = string.Empty;
        Text = string.Empty;
      }
    }

    /// <summary>
    ///   Validates this message's properties according to the given <see cref="ServerSupport" />.
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      Channel = MessageUtil.EnsureValidChannelName(Channel, serverSupport);
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhisper(new IrcMessageEventArgs<WhisperMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
