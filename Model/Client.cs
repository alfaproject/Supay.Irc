using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Supay.Irc.Contacts;
using Supay.Irc.Messages;
using Supay.Irc.Messages.Modes;
using Supay.Irc.Network;

namespace Supay.Irc
{
  /// <summary>
  ///   Represents an IRC client. It has a connection, a user, etc.
  /// </summary>
  /// <remarks>
  ///   A GUI front-end should use one instance of these per client / server
  ///   <see cref="ClientConnection" /> it wants to make.
  /// </remarks>
  [DesignerCategory("Code")]
  public class Client : Component
  {
    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Client" /> class.
    /// </summary>
    public Client()
    {
      this.DefaultQuitMessage = "Quitting";
      this.EnableAutoIdent = true;
      this.ServerName = string.Empty;
      this.ServerSupports = new ServerSupport();

      this.Messages = new MessageConduit();
      this.User = new User();
      this.Connection = new ClientConnection();

      this.ServerQuery = new ServerQuery(this);
      this.Channels = new ChannelCollection();
      this.Queries = new QueryCollection();
      this.Peers = new UserCollection();
      this.Contacts = new ContactList();

      this.Peers.Add(this.User);

      this.hookupEvents();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Client" /> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    public Client(string address)
      : this()
    {
      this.Connection.Address = address;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Client" /> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    /// <param name="nick">The nick of the <see cref="User" />.</param>
    public Client(string address, string nick)
      : this(address)
    {
      this.User.Nickname = nick;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Client" /> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    /// <param name="nick">The <see cref="Supay.Irc.User.Nickname" /> of the <see cref="Client.User" />.</param>
    /// <param name="realName">The <see cref="Supay.Irc.User.Name" /> of the <see cref="Client.User" />.</param>
    public Client(string address, string nick, string realName)
      : this(address, nick)
    {
      this.User.Name = realName;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the conduit through which individual message received events can be attached.
    /// </summary>
    public MessageConduit Messages
    {
      get;
      protected set;
    }

    /// <summary>
    ///   Gets or sets the default quit message if the client has to close the connection itself.
    /// </summary>
    public string DefaultQuitMessage
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets whether the <see cref="Client" /> will automatically start and stop an
    ///   <see cref="Ident" /> service as needed to connect to the IRC server.
    /// </summary>
    public bool EnableAutoIdent
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets the <see cref="ClientConnection" /> of the current <see cref="Client" />.
    /// </summary>
    public ClientConnection Connection
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets or sets the <see cref="User" /> of the current <see cref="Client" />.
    /// </summary>
    public User User
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the name of the server that you are connected to.
    /// </summary>
    /// <remarks>
    ///   This is the name that the server refers to itself by in messages, not necessarily the
    ///   name you use to connect.
    /// </remarks>
    public string ServerName
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets a <see cref="ServerSupport" /> object containing knowledge about what the current
    ///   server supports.
    /// </summary>
    public ServerSupport ServerSupports
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the query window to the server to which this client is connected.
    /// </summary>
    public ServerQuery ServerQuery
    {
      get;
      protected set;
    }

    /// <summary>
    ///   Gets the collection of channels which the user has joined.
    /// </summary>
    public ChannelCollection Channels
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the collection of queries the user is engaged in.
    /// </summary>
    public QueryCollection Queries
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the collection of users which the user has seen.
    /// </summary>
    public UserCollection Peers
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the <see cref="Supay.Irc.Contacts.ContactList" /> for this client.
    /// </summary>
    public ContactList Contacts
    {
      get;
      protected set;
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Sends a <see cref="IrcMessage" /> over a <see cref="ClientConnection" /> to an IRC server.
    /// </summary>
    /// <param name="message">The <see cref="IrcMessage" /> to send.</param>
    public virtual void Send(IrcMessage message)
    {
      if (message == null)
      {
        return;
      }

      var e = new CancelIrcMessageEventArgs<IrcMessage>(message);
      this.OnMessageSending(e);
      if (e.Cancel)
      {
        return;
      }

      message.Validate(this.ServerSupports);
      this.Connection.Write(message + Environment.NewLine);
    }

    /// <summary>
    ///   Determines if the given message originated from the currently connected server.
    /// </summary>
    public virtual bool IsMessageFromServer(IrcMessage msg)
    {
      if (msg == null)
      {
        return false;
      }
      return msg.Sender.Nickname == this.ServerName;
    }

    private bool isMe(string nick)
    {
      return this.User.Nickname.EqualsI(nick);
    }

    private void routeData(string messageData)
    {
      IrcMessage msg;
      try
      {
        msg = IrcMessageFactory.Parse(messageData);
      }
      catch (InvalidMessageException ex)
      {
        // try one more time to parse it as a generic message
        msg = new GenericMessage();
        if (msg.CanParse(messageData))
        {
          msg.Parse(messageData);
        }
        else
        {
          msg = null;
          Trace.WriteLine(ex.Message + " { " + ex.ReceivedMessage + " } ", "Invalid Message");
        }
      }

      if (msg != null)
      {
        this.OnMessageParsed(new IrcMessageEventArgs<IrcMessage>(msg));
        msg.Notify(this.Messages);
      }
    }

    #region Send Helpers

    /// <summary>
    ///   Sends a <see cref="ChatMessage" /> with the given text to the given channel or user.
    /// </summary>
    /// <param name="text">The text of the message.</param>
    /// <param name="target">The target of the message, either a channel or nick.</param>
    public virtual void SendChat(string text, string target)
    {
      this.Send(new ChatMessage(text, target));
    }

    /// <summary>
    ///   Sends a <see cref="ActionRequestMessage" /> with the given text to the given channel or
    ///   user.
    /// </summary>
    /// <param name="text">The text of the action.</param>
    /// <param name="target">The target of the message, either a channel or nick.</param>
    public virtual void SendAction(string text, string target)
    {
      this.Send(new ActionRequestMessage(text, target));
    }

    /// <summary>
    ///   Sends a <see cref="JoinMessage" /> for the given channel.
    /// </summary>
    /// <param name="channel">The channel to join.</param>
    public virtual void SendJoin(string channel)
    {
      this.Send(new JoinMessage(channel));
    }

    /// <summary>
    ///   Sends a <see cref="PartMessage" /> for the given channel.
    /// </summary>
    /// <param name="channel">The channel to part.</param>
    public virtual void SendPart(string channel)
    {
      this.Send(new PartMessage(channel));
    }

    /// <summary>
    ///   Sends an <see cref="AwayMessage" /> with the given reason.
    /// </summary>
    /// <param name="reason">The reason for being away.</param>
    public virtual void SendAway(string reason)
    {
      this.Send(new AwayMessage(reason));
    }

    /// <summary>
    ///   Sends a <see cref="BackMessage" />.
    /// </summary>
    public virtual void SendBack()
    {
      this.Send(new BackMessage());
    }

    /// <summary>
    ///   Sends a <see cref="QuitMessage" />.
    /// </summary>
    public virtual void SendQuit()
    {
      this.SendQuit(this.DefaultQuitMessage);
    }

    /// <summary>
    ///   Sends a <see cref="QuitMessage" /> with the given reason.
    /// </summary>
    /// <param name="reason">The reason for quitting.</param>
    public virtual void SendQuit(string reason)
    {
      this.Send(new QuitMessage(reason));
    }

    #endregion

    #endregion

    #region Events

    /// <summary>
    ///   Occurs when the <see cref="ClientConnection" /> receives data.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> DataReceived;

    /// <summary>
    ///   Occurs when the <see cref="ClientConnection" /> sends data.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> DataSent;

    /// <summary>
    ///   Occurs when any message is received and parsed.
    /// </summary>
    public event EventHandler<IrcMessageEventArgs<IrcMessage>> MessageParsed;

    /// <summary>
    ///   Raises the <see cref="MessageParsed" /> event.
    /// </summary>
    protected void OnMessageParsed(IrcMessageEventArgs<IrcMessage> e)
    {
      if (this.MessageParsed != null)
      {
        this.MessageParsed(this, e);
      }
    }

    /// <summary>
    ///   Occurs when the connection is opened and the server has sent a welcome message.
    /// </summary>
    /// <remarks>
    ///   This is the earliest the messages can be sent over the IRC network.
    /// </remarks>
    public event EventHandler Ready;

    /// <summary>
    ///   Occurs when any message is about to be sent.
    /// </summary>
    public event EventHandler<CancelIrcMessageEventArgs<IrcMessage>> MessageSending;

    /// <summary>
    ///   Raises the <see cref="MessageSending" /> event.
    /// </summary>
    protected void OnMessageSending(CancelIrcMessageEventArgs<IrcMessage> e)
    {
      if (this.MessageSending != null)
      {
        this.MessageSending(this, e);
      }
    }

    #endregion

    #region EventHandlers

    private void hookupEvents()
    {
      this.Connection.Connected += this.handleConnectionConnected;
      this.Connection.DataReceived += this.handleConnectionDataReceived;
      this.Connection.DataSent += this.handleConnectionDataSent;
      this.Connection.Disconnected += handleConnectionDisconnected;

      this.Messages.Welcome += this.handleWelcome;
      this.Messages.Ping += this.handlePing;
      this.Messages.Support += this.handleSupport;

      this.MessageParsed += this.handleMessageParsed;

      this.Messages.Join += this.handleJoin;
      this.Messages.Kick += this.handleKick;
      this.Messages.Kill += this.handleKill;
      this.Messages.Part += this.handlePart;
      this.Messages.Quit += this.handleQuit;

      this.Messages.TopicNoneReply += this.handleTopicNone;
      this.Messages.TopicReply += this.handleTopic;
      this.Messages.TopicSetReply += this.handleTopicSet;
      this.Messages.ChannelModeIsReply += this.handleChannelModeIs;
      this.Messages.ChannelProperty += this.handleChannelProperty;
      this.Messages.ChannelPropertyReply += this.handleChannelPropertyReply;

      this.Messages.NamesReply += this.handleNames;
      this.Messages.NickChange += this.handleNick;
      this.Messages.WhoReply += this.handleWho;
      this.Messages.WhoIsOperReply += this.handleWhoIsOper;
      this.Messages.WhoIsServerReply += this.handleWhoIsServer;
      this.Messages.WhoIsUserReply += this.handleWhoIsUser;
      this.Messages.UserHostReply += this.handleUserHostReply;
      this.Messages.OperReply += this.handleOper;
      this.Messages.UserMode += this.handleUserMode;
      this.Messages.UserModeIsReply += this.handleUserModeIs;

      this.Messages.Away += this.handleAway;
      this.Messages.Back += this.handleBack;
      this.Messages.SelfAway += this.handleSelfAway;
      this.Messages.SelfUnAway += this.handleSelfUnAway;
      this.Messages.UserAway += this.handleUserAway;

      this.Messages.NoSuchChannel += this.handleNoSuchChannel;
      this.Messages.NoSuchNick += this.handleNoSuchNick;

      this.Messages.GenericCtcpRequest += logGenericCtcp;
      this.Messages.GenericMessage += logGenericMessage;
      this.Messages.GenericNumericMessage += logGenericNumeric;
      this.Messages.GenericErrorMessage += logGenericError;
    }

    /// <summary>
    ///   Transforms <see cref="ClientConnection" /> data into raised <see cref="IrcMessage" /> events.
    /// </summary>
    private void handleConnectionDataReceived(object sender, ConnectionDataEventArgs e)
    {
      if (this.DataReceived != null)
      {
        this.DataReceived(this, e);
      }

      this.routeData(e.Data);
    }

    private void handleConnectionDataSent(object sender, ConnectionDataEventArgs e)
    {
      if (this.DataSent != null)
      {
        this.DataSent(this, e);
      }
    }

    /// <summary>
    ///   Keeps an IRC connection alive.
    /// </summary>
    /// <remarks>
    ///   An IRC server will ping you every x seconds to make sure you are still alive. This method
    ///   will auto-pong a return to keep the <see cref="ClientConnection" /> alive auto-magically.
    /// </remarks>
    /// <param name="sender">The connection object sending the ping.</param>
    /// <param name="e">The message sent.</param>
    private void handlePing(object sender, IrcMessageEventArgs<PingMessage> e)
    {
      this.Send(new PongMessage {
        Target = e.Message.Target
      });
    }

    private void handleConnectionConnected(object sender, EventArgs e)
    {
      this.startIdent();

      // send password
      if (!string.IsNullOrEmpty(this.User.Password))
      {
        this.Send(new PasswordMessage(this.User.Password));
      }

      // send nickname
      this.Send(new NickMessage(this.User.Nickname));

      // send user notification
      this.Send(new UserNotificationMessage {
        RealName = string.IsNullOrEmpty(this.User.Name) ? this.User.Nickname : this.User.Name,
        UserName = string.IsNullOrEmpty(this.User.Username) ? this.User.Nickname : this.User.Username,
        InitialInvisibility = true
      });
    }

    private static void handleConnectionDisconnected(object sender, ConnectionDataEventArgs e)
    {
      Ident.Service.Stop();
    }

    private void handleWelcome(object sender, IrcMessageEventArgs<WelcomeMessage> e)
    {
      this.ServerName = e.Message.Sender.Nickname;
      Ident.Service.Stop();

      if (e.Message.Target != this.User.Nickname)
      {
        this.User.Nickname = e.Message.Target;
      }

      // server is ready to receive messages
      this.Ready(this, EventArgs.Empty);
    }

    private void handleNick(object sender, IrcMessageEventArgs<NickMessage> e)
    {
      string oldNick = e.Message.Sender.Nickname;
      string newNick = e.Message.NewNick;
      if (this.isMe(oldNick))
      {
        this.User.Nickname = newNick;
      }
      else
      {
        User user = this.Peers.Find(oldNick);
        if (user != null)
        {
          user.Nickname = newNick;
        }
      }
    }

    private void handleSupport(object sender, IrcMessageEventArgs<SupportMessage> e)
    {
      this.ServerSupports.LoadInfo(e.Message);
    }

    private void handleMessageParsed(object sender, IrcMessageEventArgs<IrcMessage> e)
    {
      IChannelTargetedMessage channelMessage = e.Message as IChannelTargetedMessage;
      if (channelMessage != null)
      {
        Channel channel = this.Channels.FirstOrDefault(c => channelMessage.IsTargetedAtChannel(c.Name));
        if (channel != null)
        {
          channel.Journal.Add(new JournalEntry(e.Message));
          return;
        }
      }
      else
      {
        IQueryTargetedMessage queryMessage = e.Message as IQueryTargetedMessage;
        if (queryMessage != null && queryMessage.IsQueryToUser(this.User))
        {
          User msgSender = this.Peers.EnsureUser(e.Message.Sender);
          Query qry = this.Queries.EnsureQuery(msgSender, this);
          qry.Journal.Add(new JournalEntry(e.Message));
          return;
        }
      }

      this.ServerQuery.Journal.Add(new JournalEntry(e.Message));
    }

    private void handleJoin(object sender, IrcMessageEventArgs<JoinMessage> e)
    {
      User msgUser = e.Message.Sender;
      User joinedUser = this.isMe(msgUser.Nickname) ? this.User : this.Peers.EnsureUser(msgUser);

      foreach (Channel joinedChannel in e.Message.Channels.Select(channelname => this.Channels.EnsureChannel(channelname)))
      {
        joinedChannel.Open = true;
        joinedChannel.Users.Add(joinedUser);
      }
    }

    private void handleKick(object sender, IrcMessageEventArgs<KickMessage> e)
    {
      for (int i = 0; i < e.Message.Channels.Count; i++)
      {
        string channelName = e.Message.Channels[i];
        string nick = e.Message.Nicks[i];
        Channel channel = this.Channels.Find(channelName);

        if (this.isMe(nick))
        {
          // we don't want to actually remove the channel, but just close the channel
          // this allows a consumer to easily keep their reference to channels between kicks and re-joins.
          channel.Open = false;
        }
        else
        {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void handleKill(object sender, IrcMessageEventArgs<KillMessage> e)
    {
      string nick = e.Message.Nick;
      if (this.isMe(nick))
      {
        foreach (Channel c in this.Channels)
        {
          c.Open = false;
        }
      }
      else
      {
        foreach (Channel channel in this.Channels)
        {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void handleNames(object sender, IrcMessageEventArgs<NamesReplyMessage> e)
    {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel);
      foreach (User user in e.Message.Nicks.Keys.Select(nick => this.Peers.EnsureUser(nick)))
      {
        if (!channel.Users.Contains(user))
        {
          channel.Users.Add(user);
        }
        ChannelStatus status = e.Message.Nicks[user.Nickname];
        channel.SetStatusForUser(user, status);
      }
    }

    private void handlePart(object sender, IrcMessageEventArgs<PartMessage> e)
    {
      string nick = e.Message.Sender.Nickname;
      foreach (Channel channel in e.Message.Channels.Select(channelName => this.Channels.Find(channelName)))
      {
        if (this.isMe(nick))
        {
          channel.Open = false;
        }
        else
        {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void handleQuit(object sender, IrcMessageEventArgs<QuitMessage> e)
    {
      string nick = e.Message.Sender.Nickname;
      if (this.isMe(nick))
      {
        foreach (Channel channel in this.Channels)
        {
          channel.Open = false;
        }
      }
      else
      {
        foreach (Channel channel in this.Channels)
        {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void handleTopicNone(object sender, IrcMessageEventArgs<TopicNoneReplyMessage> e)
    {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null)
      {
        channel.Topic = string.Empty;
      }
    }

    private void handleTopic(object sender, IrcMessageEventArgs<TopicReplyMessage> e)
    {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null)
      {
        channel.Topic = e.Message.Topic;
      }
    }

    private void handleTopicSet(object sender, IrcMessageEventArgs<TopicSetReplyMessage> e)
    {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null)
      {
        channel.TopicSetter = this.Peers.EnsureUser(e.Message.User);
        channel.TopicSetTime = e.Message.TimeSet;
      }
    }

    private void handleWho(object sender, IrcMessageEventArgs<WhoReplyMessage> e)
    {
      User whoUser = this.Peers.EnsureUser(e.Message.User);
      string channelName = e.Message.Channel;

      Channel channel = this.Channels.Find(channelName);
      if (channel != null)
      {
        if (!channel.Users.Contains(whoUser))
        {
          channel.Users.Add(whoUser);
        }
        channel.SetStatusForUser(whoUser, e.Message.Status);
      }
    }

    private void handleNoSuchChannel(object sender, IrcMessageEventArgs<NoSuchChannelMessage> e)
    {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null)
      {
        channel.Open = false;
      }
    }

    private void handleNoSuchNick(object sender, IrcMessageEventArgs<NoSuchNickMessage> e)
    {
      string nick = e.Message.Nick;

      // NoSuchNickMessage is sent by some servers instead of a NoSuchChannelMessage
      if (MessageUtil.HasValidChannelPrefix(nick))
      {
        Channel channel = this.Channels.Find(e.Message.Nick);
        if (channel != null)
        {
          channel.Open = false;
        }
      }
      else
      {
        foreach (Channel channel in this.Channels)
        {
          channel.Users.RemoveFirst(nick);
        }
        this.Peers.RemoveFirst(nick);
      }
    }

    private void handleChannelModeIs(object sender, IrcMessageEventArgs<ChannelModeIsReplyMessage> e)
    {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null)
      {
        ChannelModesCreator modes = new ChannelModesCreator {
          ServerSupport = this.ServerSupports
        };
        modes.Parse(e.Message.Modes, e.Message.ModeArguments);
        channel.Modes.ResetWith(modes.Modes);
      }
    }

    private void handleUserModeIs(object sender, IrcMessageEventArgs<UserModeIsReplyMessage> e)
    {
      if (this.isMe(e.Message.Target))
      {
        UserModesCreator modeCreator = new UserModesCreator();
        modeCreator.Parse(e.Message.Modes);
        this.User.Modes.Clear();
        foreach (UserMode mode in modeCreator.Modes)
        {
          this.User.Modes.Add(mode);
        }
      }
    }

    private void handleUserMode(object sender, IrcMessageEventArgs<UserModeMessage> e)
    {
      if (this.isMe(e.Message.User))
      {
        UserModesCreator modeCreator = new UserModesCreator();
        modeCreator.Parse(e.Message.ModeChanges);
        this.User.Modes.Clear();
        foreach (UserMode mode in modeCreator.Modes)
        {
          this.User.Modes.Add(mode);
        }
      }
    }

    private void handleUserHostReply(object sender, IrcMessageEventArgs<UserHostReplyMessage> e)
    {
      foreach (User sentUser in e.Message.Users)
      {
        if (this.isMe(sentUser.Nickname))
        {
          this.User.CopyFrom(sentUser);
        }
        else
        {
          User user = this.Peers.EnsureUser(sentUser);
          if (user != sentUser)
          {
            user.CopyFrom(sentUser);
          }
          if (!user.Away)
          {
            user.AwayMessage = string.Empty;
          }
        }
      }
    }

    private void handleUserAway(object sender, IrcMessageEventArgs<UserAwayMessage> e)
    {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.Away = true;
      user.AwayMessage = e.Message.Text;
    }

    private void handleSelfUnAway(object sender, IrcMessageEventArgs<SelfUnAwayMessage> e)
    {
      this.User.Away = false;
      this.User.AwayMessage = string.Empty;
    }

    private void handleSelfAway(object sender, IrcMessageEventArgs<SelfAwayMessage> e)
    {
      this.User.Away = true;
    }

    private void handleOper(object sender, IrcMessageEventArgs<OperReplyMessage> e)
    {
      this.User.IrcOperator = true;
    }

    private void handleChannelProperty(object sender, IrcMessageEventArgs<ChannelPropertyMessage> e)
    {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel);
      channel.Properties[e.Message.Prop] = e.Message.NewValue;
    }

    private void handleChannelPropertyReply(object sender, IrcMessageEventArgs<ChannelPropertyReplyMessage> e)
    {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel);
      channel.Properties[e.Message.Prop] = e.Message.Value;
    }

    private void handleBack(object sender, IrcMessageEventArgs<BackMessage> e)
    {
      User user = this.Peers.EnsureUser(e.Message.Sender);
      user.Away = false;
      user.AwayMessage = string.Empty;
    }

    private void handleAway(object sender, IrcMessageEventArgs<AwayMessage> e)
    {
      User user = this.Peers.EnsureUser(e.Message.Sender);
      user.Away = true;
      user.AwayMessage = e.Message.Reason;
    }

    private void handleWhoIsUser(object sender, IrcMessageEventArgs<WhoIsUserReplyMessage> e)
    {
      User target = this.Peers.EnsureUser(e.Message.User);
      target.CopyFrom(e.Message.User);
    }

    private void handleWhoIsServer(object sender, IrcMessageEventArgs<WhoIsServerReplyMessage> e)
    {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.Server = e.Message.ServerName;
    }

    private void handleWhoIsOper(object sender, IrcMessageEventArgs<WhoIsOperReplyMessage> e)
    {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.IrcOperator = true;
    }

    private static void logGenericCtcp(object sender, IrcMessageEventArgs<GenericCtcpRequestMessage> e)
    {
      logUnimplementedMessage(e.Message);
    }

    private static void logGenericMessage(object sender, IrcMessageEventArgs<GenericMessage> e)
    {
      logUnimplementedMessage(e.Message);
    }

    private static void logGenericNumeric(object sender, IrcMessageEventArgs<GenericNumericMessage> e)
    {
      logUnimplementedMessage(e.Message);
    }

    private static void logGenericError(object sender, IrcMessageEventArgs<GenericErrorMessage> e)
    {
      logUnimplementedMessage(e.Message);
    }

    private static void logUnimplementedMessage(IrcMessage msg)
    {
      Trace.WriteLine(msg.ToString(), "Unimplemented Message");
    }

    #endregion

    #region Private

    private void startIdent()
    {
      if (this.EnableAutoIdent)
      {
        Ident.Service.User = this.User;
        Ident.Service.Start(true);
        DateTime started = DateTime.Now;
        TimeSpan tooMuchTime = new TimeSpan(0, 0, 5);
        while (Ident.Service.Status != ConnectionStatus.Connected && DateTime.Now.Subtract(started) < tooMuchTime)
        {
          Thread.Sleep(25);
        }
        if (Ident.Service.Status != ConnectionStatus.Connected)
        {
          Trace.WriteLine("Ident Failed To AutoStart", "Ident");
        }
      }
    }

    #endregion
  }
}
