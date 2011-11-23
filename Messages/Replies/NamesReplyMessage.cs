using System;
using System.Linq;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A single reply to the <see cref="NamesMessage" /> query.
  /// </summary>
  [Serializable]
  public class NamesReplyMessage : NumericMessage, IChannelTargetedMessage
  {
    #region ChannelVisibility enum

    /// <summary>
    ///   The list of channel visibility settings for the <see cref="NamesReplyMessage" />.
    /// </summary>
    public enum ChannelVisibility
    {
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
      : base(353)
    {
      this.Nicks = new Dictionary<string, ChannelStatus>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Gets or sets the visibility of the channel specified in the reply.
    /// </summary>
    public virtual ChannelVisibility Visibility
    {
      get
      {
        return this.visibility;
      }
      set
      {
        this.visibility = value;
      }
    }

    /// <summary>
    ///   Gets or sets the name of the channel specified in the reply.
    /// </summary>
    public virtual string Channel
    {
      get
      {
        return this.channel;
      }
      set
      {
        this.channel = value;
      }
    }

    /// <summary>
    ///   Gets the collection of nicks in the channel.
    /// </summary>
    public Dictionary<string, ChannelStatus> Nicks
    {
      get;
      private set;
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      switch (this.Visibility)
      {
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
      parameters.Add(this.Channel);
      parameters.Add(string.Join(" ", this.Nicks.Select(user => user.Value.Symbol + user.Key)));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Visibility = ChannelVisibility.Public;
      this.Channel = string.Empty;
      this.Nicks.Clear();

      if (parameters.Count >= 3)
      {
        switch (parameters[1])
        {
          case "=":
            this.Visibility = ChannelVisibility.Public;
            break;
          case "*":
            this.Visibility = ChannelVisibility.Private;
            break;
          case "@":
            this.Visibility = ChannelVisibility.Secret;
            break;
        }
        this.Channel = parameters[2];
        if (parameters.Count > 3)
        {
          var msgNicks = parameters[3].Split(' ');
          foreach (string nick in msgNicks)
          {
            ChannelStatus status = ChannelStatus.None;
            string parsedNick = nick;

            if (parsedNick.Length > 1)
            {
              string firstLetter = parsedNick.Substring(0, 1);
              if (ChannelStatus.IsDefined(firstLetter))
              {
                status = ChannelStatus.GetInstance(firstLetter);
                parsedNick = parsedNick.Substring(1);
              }
            }
            this.Nicks.Add(parsedNick, status);
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNamesReply(new IrcMessageEventArgs<NamesReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.EqualsI(channelName);
    }
  }
}
