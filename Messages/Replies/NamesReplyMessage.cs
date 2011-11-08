using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A single reply to the <see cref="NamesMessage" /> query.
  /// </summary>
  [Serializable]
  public class NamesReplyMessage : NumericMessage, IChannelTargetedMessage {
    #region ChannelVisibility enum

    /// <summary>
    ///   The list of channel visibility settings for the <see cref="NamesReplyMessage" />.
    /// </summary>
    public enum ChannelVisibility {
      /// <summary>
      ///   The channel is in <see cref="Supay.Irc.Messages.Modes.SecretMode" />
      /// </summary>
      Secret,

      /// <summary>
      ///   The channel is in <see cref="Supay.Irc.Messages.Modes.PrivateMode" />
      /// </summary>
      Private,

      /// <summary>
      ///   The channel has no hidden modes applied.
      /// </summary>
      Public
    }

    #endregion

    private string channel = string.Empty;
    private ChannelVisibility visibility = ChannelVisibility.Public;

    /// <summary>
    ///   Creates a new instance of the <see cref="NamesReplyMessage" /> class.
    /// </summary>
    public NamesReplyMessage()
      : base(353) {
      Nicks = new Dictionary<string, ChannelStatus>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Gets or sets the visibility of the channel specified in the reply.
    /// </summary>
    public virtual ChannelVisibility Visibility {
      get {
        return visibility;
      }
      set {
        visibility = value;
      }
    }

    /// <summary>
    ///   Gets or sets the name of the channel specified in the reply.
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
    ///   Gets the collection of nicks in the channel.
    /// </summary>
    public Dictionary<string, ChannelStatus> Nicks {
      get;
      private set;
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
      switch (Visibility) {
        case ChannelVisibility.Public:
          parameters.Add("=");
          break;
        case ChannelVisibility.Private:
          parameters.Add("*");
          break;
        case ChannelVisibility.Secret:
          parameters.Add("@");
          break;
      }
      parameters.Add(Channel);
      parameters.Add(MessageUtil.CreateList(Nicks.Keys, " ", nick => Nicks[nick].Symbol + nick));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);

      Visibility = ChannelVisibility.Public;
      Channel = string.Empty;
      Nicks.Clear();

      if (parameters.Count >= 3) {
        switch (parameters[1]) {
          case "=":
            Visibility = ChannelVisibility.Public;
            break;
          case "*":
            Visibility = ChannelVisibility.Private;
            break;
          case "@":
            Visibility = ChannelVisibility.Secret;
            break;
        }
        Channel = parameters[2];
        if (parameters.Count > 3) {
          string[] msgNicks = parameters[3].Split(' ');
          foreach (string nick in msgNicks) {
            ChannelStatus status = ChannelStatus.None;
            string parsedNick = nick;

            if (parsedNick.Length > 1) {
              string firstLetter = parsedNick.Substring(0, 1);
              if (ChannelStatus.IsDefined(firstLetter)) {
                status = ChannelStatus.GetInstance(firstLetter);
                parsedNick = parsedNick.Substring(1);
              }
            }
            Nicks.Add(parsedNick, status);
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnNamesReply(new IrcMessageEventArgs<NamesReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
