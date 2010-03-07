using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   This is sent at the end of a channel ban list, when requested. (with MODE #chan +b.) </summary>
  /// <remarks>
  ///   Numeric: 368
  ///   Name:    RPL_ENDOFBANLIST
  ///   Syntax:  368 channel :info
  ///   Example: 368 #howdy :End of Channel Ban List </remarks>
  /// <seealso cref="BansReplyMessage"/>
  [Serializable]
  public class BansEndReplyMessage : NumericMessage, IChannelTargetedMessage {

    private const string DEFAULT_INFO = "End of Channel Ban List";

    private string _channel;
    private string _info;

    /// <summary>
    ///   Creates a new instance of the <see cref="BansEndReplyMessage"/> class. </summary>
    public BansEndReplyMessage()
      : base(368) {
      _channel = null;
      _info = DEFAULT_INFO;
    }

    /// <summary>
    ///   Gets or sets the channel the ban list refers to. </summary>
    public virtual string Channel {
      get { return _channel; }
      set { _channel = value; }
    }

    /// <summary>
    ///   Gets or sets the info message of this reply. </summary>
    public virtual string Info {
      get { return _info; }
      set { _info = value; }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(string.IsNullOrEmpty(Info) ? DEFAULT_INFO : Info);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        Channel = parameters[1];
        Info = (parameters.Count > 2 ? parameters[2] : DEFAULT_INFO);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    ///   current <see cref="IrcMessage"/> subclass. </summary>
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