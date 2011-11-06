using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply to a <see cref="WhoMessage" /> query.
  /// </summary>
  [Serializable]
  public class WhoReplyMessage : NumericMessage, IChannelTargetedMessage {
    private string _channel = string.Empty;
    private int _hopCount = -1;
    private ChannelStatus _status = ChannelStatus.None;
    private User _user = new User();

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoReplyMessage" /> class.
    /// </summary>
    public WhoReplyMessage()
      : base(352) {
    }

    /// <summary>
    ///   Gets or sets the channel associated with the user.
    /// </summary>
    /// <remarks>
    ///   In the case of a non-channel based <see cref="WhoMessage" />, Channel will contain the
    ///   most recent channel which the user joined and is still on.
    /// </remarks>
    public virtual string Channel {
      get {
        return _channel;
      }
      set {
        _channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user being examined.
    /// </summary>
    public virtual User User {
      get {
        return _user;
      }
      set {
        _user = value;
      }
    }

    /// <summary>
    ///   Gets or sets the status of the user on the associated channel.
    /// </summary>
    public virtual ChannelStatus Status {
      get {
        return _status;
      }
      set {
        _status = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of hops to the server the user is on.
    /// </summary>
    public virtual int HopCount {
      get {
        return _hopCount;
      }
      set {
        _hopCount = value;
      }
    }

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    public virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(User.Username);
      parameters.Add(User.Host);
      parameters.Add(User.Server);
      parameters.Add(User.Nickname);
      parameters.Add((User.Away ? "G" : "H") + (User.IrcOperator ? "*" : string.Empty) + Status.Symbol);
      parameters.Add(HopCount.ToString(CultureInfo.InvariantCulture) + " " + User.Name);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      User = new User();

      if (parameters.Count == 8) {
        Channel = parameters[1];
        User.Username = parameters[2];
        User.Host = parameters[3];
        User.Server = parameters[4];
        User.Nickname = parameters[5];

        foreach (char flag in parameters[6]) {
          switch (flag) {
            case 'H': // here
              User.Away = false;
              break;
            case 'G': // gone
              User.Away = true;
              break;
            case '*': // ircop
            case '!': // ircop (hidden?)
              User.IrcOperator = true;
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
              if (ChannelStatus.IsDefined(flag.ToString())) {
                Status = ChannelStatus.GetInstance(flag.ToString());
              }
              break;
          }
        }

        string trailing = parameters[7];
        HopCount = Convert.ToInt32(trailing.Substring(0, trailing.IndexOf(' ')), CultureInfo.InvariantCulture);
        User.Name = trailing.Substring(trailing.IndexOf(' '));
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoReply(new IrcMessageEventArgs<WhoReplyMessage>(this));
    }
  }
}
