using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A reply to a <see cref="WhoMessage" /> query.
  /// </summary>
  [Serializable]
  public class WhoReplyMessage : NumericMessage, IChannelTargetedMessage
  {
    private string channel = string.Empty;
    private int hopCount = -1;
    private ChannelStatus status = ChannelStatus.None;
    private User user = new User();

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoReplyMessage" /> class.
    /// </summary>
    public WhoReplyMessage()
      : base(352)
    {
    }

    /// <summary>
    ///   Gets or sets the channel associated with the user.
    /// </summary>
    /// <remarks>
    ///   In the case of a non-channel based <see cref="WhoMessage" />, Channel will contain the
    ///   most recent channel which the user joined and is still on.
    /// </remarks>
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
    ///   Gets or sets the user being examined.
    /// </summary>
    public virtual User User
    {
      get
      {
        return this.user;
      }
      set
      {
        this.user = value;
      }
    }

    /// <summary>
    ///   Gets or sets the status of the user on the associated channel.
    /// </summary>
    public virtual ChannelStatus Status
    {
      get
      {
        return this.status;
      }
      set
      {
        this.status = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of hops to the server the user is on.
    /// </summary>
    public virtual int HopCount
    {
      get
      {
        return this.hopCount;
      }
      set
      {
        this.hopCount = value;
      }
    }

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    public virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add(this.User.Username);
        parameters.Add(this.User.Host);
        parameters.Add(this.User.Server);
        parameters.Add(this.User.Nickname);
        parameters.Add((this.User.Away ? "G" : "H") + (this.User.IrcOperator ? "*" : string.Empty) + this.Status.Symbol);
        parameters.Add(this.HopCount.ToString(CultureInfo.InvariantCulture) + " " + this.User.Name);
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.User = new User();

      if (parameters.Count == 8)
      {
        this.Channel = parameters[1];
        this.User.Username = parameters[2];
        this.User.Host = parameters[3];
        this.User.Server = parameters[4];
        this.User.Nickname = parameters[5];

        foreach (char flag in parameters[6])
        {
          switch (flag)
          {
            case 'H': // here
              this.User.Away = false;
              break;
            case 'G': // gone
              this.User.Away = true;
              break;
            case '*': // ircop
            case '!': // ircop (hidden?)
              this.User.IrcOperator = true;
              break;
            case 'r': // registered
              break;
            case 'B': // bot
              break;
            case 'd': // deaf
              break;
            case '?': // can see ircop (?)
              break;
            default: // check for a channel mode
              if (ChannelStatus.IsDefined(flag.ToString(CultureInfo.InvariantCulture)))
              {
                this.Status = ChannelStatus.GetInstance(flag.ToString(CultureInfo.InvariantCulture));
              }
              break;
          }
        }

        string trailing = parameters[7];
        this.HopCount = Convert.ToInt32(trailing.Substring(0, trailing.IndexOf(' ')), CultureInfo.InvariantCulture);
        this.User.Name = trailing.Substring(trailing.IndexOf(' '));
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWhoReply(new IrcMessageEventArgs<WhoReplyMessage>(this));
    }
  }
}
