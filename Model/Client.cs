using System;
using System.Globalization;
using System.IO;
using Supay.Irc.Messages;
using Supay.Irc.Messages.Modes;
using Supay.Irc.Network;

namespace Supay.Irc {

  /// <summary>
  /// Represents an irc client. it has a connection, a user, etc
  /// </summary>
  /// <remarks>A gui frontend should use one instance of these per client/server <see cref="ClientConnection"/> it wants to make.</remarks>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
  [System.ComponentModel.DesignerCategory("Code")]
  public class Client : IDisposable {

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="Client"/> class.</summary>
    public Client() {
      this.DefaultQuitMessage = "Quiting";
      this.EnableAutoIdent = true;
      this.ServerName = "";
      this.ServerSupports = new ServerSupport();

      this.Messages = new MessageConduit();
      this.User = new Supay.Irc.User();
      this.Connection = new ClientConnection();

      this.ServerQuery = new ServerQuery(this);
      this.Channels = new ChannelCollection();
      this.Queries = new QueryCollection();
      this.Peers = new UserCollection();
      this.Contacts = new Supay.Irc.Contacts.ContactList();

      this.Peers.Add(this.User);

      HookupEvents();


    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    public Client(string address)
      : this() {
      this.Connection.Address = address;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    /// <param name="nick">The nick of the <see cref="Client.User"/></param>
    public Client(string address, string nick)
      : this(address) {
      this.User.Nickname = nick;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Client"/> with the given address.
    /// </summary>
    /// <param name="address">The address that will be connected to.</param>
    /// <param name="nick">The <see cref="Supay.Irc.User.Nickname"/> of the <see cref="Client.User"/></param>
    /// <param name="realName">The <see cref="Supay.Irc.User.Name"/> of the <see cref="Client.User"/></param>
    public Client(string address, string nick, string realName)
      : this(address, nick) {
      this.User.Name = realName;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the conduit thru which individual message received events can be attached.
    /// </summary>
    public MessageConduit Messages {
      get;
      protected set;
    }

    /// <summary>
    /// Gets or sets the default quit message if the client 
    /// has to close the connection itself.
    /// </summary>
    public string DefaultQuitMessage {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets whether the <see cref="Client"/> will automaticly start and stop
    /// an <see cref="Ident"/> service as needed to connect to the irc server.
    /// </summary>
    public bool EnableAutoIdent {
      get;
      set;
    }

    /// <summary>
    /// Gets the <see cref="ClientConnection"/> of the current <see cref="Client"/>.
    /// </summary>
    public ClientConnection Connection {
      get;
      private set;
    }

    /// <summary>
    /// Gets or sets the <see cref="User"/> of the current <see cref="Client"/>.
    /// </summary>
    public User User {
      get;
      private set;
    }

    /// <summary>
    /// Gets the name of the server that you are connected to.
    /// </summary>
    /// <remarks>
    /// This is the name that server referes to itself by in messages, not neccesarily the name you use to connect.
    /// </remarks>
    public string ServerName {
      get;
      private set;
    }

    /// <summary>
    /// Gets a <see cref="ServerSupport"/> object containing knowledge about what the current server supports.
    /// </summary>
    public ServerSupport ServerSupports {
      get;
      private set;
    }

    /// <summary>
    /// Gets the query window to the server to which this client is connected
    /// </summary>
    public ServerQuery ServerQuery {
      get;
      protected set;
    }

    /// <summary>
    /// Gets the collection of channels which the user has joined
    /// </summary>
    public ChannelCollection Channels {
      get;
      private set;
    }

    /// <summary>
    /// Gets the collection of queries the user is enganged in
    /// </summary>
    public QueryCollection Queries {
      get;
      private set;
    }

    /// <summary>
    /// Gets the collection users which the user has seen
    /// </summary>
    public UserCollection Peers {
      get;
      private set;
    }

    /// <summary>
    /// Gets the <see cref="Supay.Irc.Contacts.ContactList" /> for this client.
    /// </summary>
    public Contacts.ContactList Contacts {
      get;
      protected set;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sends a <see cref="IrcMessage"/> over a <see cref="ClientConnection"/> to an irc server.
    /// </summary>
    /// <param name="message">The <see cref="IrcMessage"/> to send.</param>
    public virtual void Send(IrcMessage message) {
      if (message == null) {
        return;
      }

      CancelIrcMessageEventArgs<IrcMessage> e = new CancelIrcMessageEventArgs<IrcMessage>(message);
      this.OnMessageSending(e);
      if (e.Cancel) {
        return;
      }

      TextWriter originalWriter = this.writer.InnerWriter;
      using (StringWriter myInnerWriter = new StringWriter(CultureInfo.InvariantCulture)) {
        this.writer.InnerWriter = myInnerWriter;

        message.Validate(this.ServerSupports);
        message.Format(writer);
        this.Connection.Write(myInnerWriter.ToString());


        this.writer.InnerWriter = originalWriter;
      }

    }

    #region Send Helpers

    /// <summary>
    /// Sends a <see cref="ChatMessage"/> with the given text to the given channel or user.
    /// </summary>
    /// <param name="text">The text of the message.</param>
    /// <param name="target">The target of the message, either a channel or nick.</param>
    public virtual void SendChat(string text, string target) {
      this.Send(new ChatMessage(text, target));
    }

    /// <summary>
    /// Sends a <see cref="ActionRequestMessage"/> with the given text to the given  channel or user.
    /// </summary>
    /// <param name="text">The text of the action.</param>
    /// <param name="target">The target of the message, either a channel or nick.</param>
    public virtual void SendAction(string text, string target) {
      this.Send(new ActionRequestMessage(text, target));
    }

    /// <summary>
    /// Sends a <see cref="JoinMessage"/> for the given channel.
    /// </summary>
    /// <param name="channel">The channel to join.</param>
    public virtual void SendJoin(string channel) {
      this.Send(new JoinMessage(channel));
    }

    /// <summary>
    /// Sends a <see cref="PartMessage"/> for the given channel. 
    /// </summary>
    /// <param name="channel">The channel to part.</param>
    public virtual void SendPart(string channel) {
      this.Send(new PartMessage(channel));
    }

    /// <summary>
    /// Sends an <see cref="AwayMessage"/> with the given reason.
    /// </summary>
    /// <param name="reason">The reason for being away.</param>
    public virtual void SendAway(string reason) {
      this.Send(new AwayMessage(reason));
    }

    /// <summary>
    /// Sends a <see cref="BackMessage"/>.
    /// </summary>
    public virtual void SendBack() {
      this.Send(new BackMessage());
    }

    /// <summary>
    /// Sends a <see cref="QuitMessage"/>.
    /// </summary>
    public virtual void SendQuit() {
      SendQuit(this.DefaultQuitMessage);
    }

    /// <summary>
    /// Sends a <see cref="QuitMessage"/> with the given reason.
    /// </summary>
    /// <param name="reason">The reason for quitting.</param>
    public virtual void SendQuit(string reason) {
      this.Send(new QuitMessage(reason));
    }

    #endregion

    /// <summary>
    /// Determines if the given message originated from the currently connected server.
    /// </summary>
    public virtual bool IsMessageFromServer(Supay.Irc.Messages.IrcMessage msg) {
      if (msg == null) {
        return false;
      }
      return (msg.Sender.Nickname == this.ServerName);
    }

    private bool IsMe(string nick) {
      return this.User.Nickname.EqualsI(nick);
    }

    private void RouteData(string messageData) {
      IrcMessage msg = null;
      try {
        msg = MessageParserService.Service.Parse(messageData);
      } catch (Supay.Irc.Messages.InvalidMessageException ex) {
        // Try one more time to load it as a generic message
        msg = new GenericMessage();
        if (msg.CanParse(messageData)) {
          msg.Parse(messageData);
        } else {
          msg = null;
          System.Diagnostics.Trace.WriteLine(ex.Message + " { " + ex.ReceivedMessage + " } ", "Invalid Message");
        }
      }

      if (msg != null) {
        this.OnMessageParsed(new IrcMessageEventArgs<IrcMessage>(msg));

        msg.Notify(this.Messages);
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    private void HookupEvents() {

      Connection.Connected += new EventHandler(sendClientConnectionMessage);
      Connection.DataReceived += new EventHandler<ConnectionDataEventArgs>(dataReceived);
      Connection.DataSent += new EventHandler<ConnectionDataEventArgs>(dataSent);
      Connection.Disconnected += new EventHandler<ConnectionDataEventArgs>(ensureIdentEnds);


      this.Messages.Welcome += new EventHandler<IrcMessageEventArgs<WelcomeMessage>>(serverNameAquired);
      this.Messages.Ping += new EventHandler<IrcMessageEventArgs<PingMessage>>(autoPingPong);
      this.Messages.NickChange += new EventHandler<IrcMessageEventArgs<NickChangeMessage>>(keepOwnNickCorrect);
      this.Messages.Support += new EventHandler<IrcMessageEventArgs<SupportMessage>>(populateSupports);

      this.MessageParsed += new EventHandler<IrcMessageEventArgs<IrcMessage>>(messageParsed);

      this.Messages.Join += new EventHandler<IrcMessageEventArgs<JoinMessage>>(routeJoins);
      this.Messages.Kick += new EventHandler<IrcMessageEventArgs<KickMessage>>(routeKicks);
      this.Messages.Kill += new EventHandler<IrcMessageEventArgs<KillMessage>>(routeKills);
      this.Messages.Part += new EventHandler<IrcMessageEventArgs<PartMessage>>(routeParts);
      this.Messages.Quit += new EventHandler<IrcMessageEventArgs<QuitMessage>>(routeQuits);

      this.Messages.TopicNoneReply += new EventHandler<IrcMessageEventArgs<TopicNoneReplyMessage>>(routeTopicNones);
      this.Messages.TopicReply += new EventHandler<IrcMessageEventArgs<TopicReplyMessage>>(routeTopics);
      this.Messages.TopicSetReply += new EventHandler<IrcMessageEventArgs<TopicSetReplyMessage>>(routeTopicSets);
      this.Messages.ChannelModeIsReply += new EventHandler<IrcMessageEventArgs<ChannelModeIsReplyMessage>>(client_ChannelModeIsReply);
      this.Messages.ChannelProperty += new EventHandler<IrcMessageEventArgs<ChannelPropertyMessage>>(client_ChannelProperty);
      this.Messages.ChannelPropertyReply += new EventHandler<IrcMessageEventArgs<ChannelPropertyReplyMessage>>(client_ChannelPropertyReply);

      this.Messages.NamesReply += new EventHandler<IrcMessageEventArgs<NamesReplyMessage>>(routeNames);
      this.Messages.NickChange += new EventHandler<IrcMessageEventArgs<NickChangeMessage>>(routeNicks);
      this.Messages.WhoReply += new EventHandler<IrcMessageEventArgs<WhoReplyMessage>>(routeWhoReplies);
      this.Messages.WhoIsOperReply += new EventHandler<IrcMessageEventArgs<WhoIsOperReplyMessage>>(client_WhoIsOperReply);
      this.Messages.WhoIsServerReply += new EventHandler<IrcMessageEventArgs<WhoIsServerReplyMessage>>(client_WhoIsServerReply);
      this.Messages.WhoIsUserReply += new EventHandler<IrcMessageEventArgs<WhoIsUserReplyMessage>>(client_WhoIsUserReply);
      this.Messages.UserHostReply += new EventHandler<IrcMessageEventArgs<UserHostReplyMessage>>(client_UserHostReply);
      this.Messages.OperReply += new EventHandler<IrcMessageEventArgs<OperReplyMessage>>(client_OperReply);
      this.Messages.UserMode += new EventHandler<IrcMessageEventArgs<UserModeMessage>>(client_UserMode);
      this.Messages.UserModeIsReply += new EventHandler<IrcMessageEventArgs<UserModeIsReplyMessage>>(client_UserModeIsReply);

      this.Messages.Away += new EventHandler<IrcMessageEventArgs<AwayMessage>>(client_Away);
      this.Messages.Back += new EventHandler<IrcMessageEventArgs<BackMessage>>(client_Back);
      this.Messages.SelfAway += new EventHandler<IrcMessageEventArgs<SelfAwayMessage>>(client_SelfAway);
      this.Messages.SelfUnAway += new EventHandler<IrcMessageEventArgs<SelfUnAwayMessage>>(client_SelfUnAway);
      this.Messages.UserAway += new EventHandler<IrcMessageEventArgs<UserAwayMessage>>(client_UserAway);

      this.Messages.NoSuchChannel += new EventHandler<IrcMessageEventArgs<NoSuchChannelMessage>>(client_NoSuchChannel);
      this.Messages.NoSuchNick += new EventHandler<IrcMessageEventArgs<NoSuchNickMessage>>(client_NoSuchNick);




      this.Messages.GenericCtcpRequest += new EventHandler<IrcMessageEventArgs<GenericCtcpRequestMessage>>(logGenericCtcp);
      this.Messages.GenericMessage += new EventHandler<IrcMessageEventArgs<GenericMessage>>(logGenericMessage);
      this.Messages.GenericNumericMessage += new EventHandler<IrcMessageEventArgs<GenericNumericMessage>>(logGenericNumeric);
      this.Messages.GenericErrorMessage += new EventHandler<IrcMessageEventArgs<GenericErrorMessage>>(logGenericError);

    }

    #region IDisposable

    /// <summary>
    ///   Releases the unmanaged resources used by the <see cref="Client" /> and optionally releases the managed resources. </summary>
    /// <param name="disposing">
    ///   true to release both managed and unmanaged resources;
    ///   false to release only unmanaged resources. </param>
    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        if (this.Connection != null) {
          this.Connection.Dispose();
          this.Connection = null;
        }
      }
    }

    /// <summary>
    ///   Releases all resources used by the <see cref="Client" />. </summary>
    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the <see cref="ClientConnection"/> recieves data.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> DataReceived;

    /// <summary>
    /// Occurs when the <see cref="ClientConnection"/> sends data.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> DataSent;

    /// <summary>
    /// Occurs when any message is received and parsed.
    /// </summary>
    public event EventHandler<IrcMessageEventArgs<IrcMessage>> MessageParsed;
    /// <summary>Raises the MessageParsed event.</summary>
    protected void OnMessageParsed(IrcMessageEventArgs<IrcMessage> e) {
      if (this.MessageParsed != null) {
        this.MessageParsed(this, e);
      }
    }

    /// <summary>
    /// Occurs when the connection is opened and the server has sent a welcome message.
    /// </summary>
    /// <remarks>
    /// This is the earliest the messages can be sent over the irc nextwork
    /// </remarks>
    public event EventHandler Ready;

    /// <summary>
    /// Occurs when any message is about to be sent.
    /// </summary>
    public event EventHandler<CancelIrcMessageEventArgs<IrcMessage>> MessageSending;
    /// <summary>
    /// Raises the MessageSending event.
    /// </summary>
    /// <param name="e"></param>
    protected void OnMessageSending(CancelIrcMessageEventArgs<IrcMessage> e) {
      if (this.MessageSending != null) {
        this.MessageSending(this, e);
      }
    }


    #endregion

    #region EventHandlers

    /// <summary>
    /// Transforms <see cref="ClientConnection"/> data into raised <see cref="IrcMessage"/> events
    /// </summary>
    private void dataReceived(object sender, ConnectionDataEventArgs e) {
      if (this.DataReceived != null) {
        this.DataReceived(this, e);
      }

      RouteData(e.Data);

    }

    private void dataSent(object sender, ConnectionDataEventArgs e) {
      if (this.DataSent != null) {
        this.DataSent(this, e);
      }
    }

    /// <summary>
    /// Keeps an irc connection alive.
    /// </summary>
    /// <remarks>
    /// An irc server will ping you every x seconds to make sure you are still alive.
    /// This method will auto-pong a return to keep the <see cref="ClientConnection"/> alive automagically.
    /// </remarks>
    /// <param name="sender">the connection object sending the ping</param>
    /// <param name="e">the message sent</param>
    private void autoPingPong(object sender, IrcMessageEventArgs<PingMessage> e) {
      PongMessage pong = new PongMessage();
      pong.Target = e.Message.Target;
      this.Send(pong);
    }

    private void sendClientConnectionMessage(object sender, EventArgs e) {
      StartIdent();
      readyRaised = false;

      //Send Password
      if (User.Password != null && User.Password.Length != 0) {
        PasswordMessage pass = new PasswordMessage();
        pass.Password = User.Password;
        this.Send(pass);
      }

      //Send Nick
      NickChangeMessage nick = new NickChangeMessage();
      nick.NewNick = User.Nickname;
      this.Send(nick);

      //Send User
      UserNotificationMessage userNotification = new UserNotificationMessage();
      if (User.Name.Length == 0) {
        userNotification.RealName = User.Nickname;
      } else {
        userNotification.RealName = User.Name;
      }
      if (User.Username.Length == 0) {
        userNotification.UserName = User.Nickname;
      } else {
        userNotification.UserName = User.Username;
      }
      userNotification.InitialInvisibility = true;
      this.Send(userNotification);

    }

    private void ensureIdentEnds(object sender, ConnectionDataEventArgs e) {
      Ident.Service.Stop();
    }

    private void serverNameAquired(object sender, IrcMessageEventArgs<WelcomeMessage> e) {
      this.ServerName = e.Message.Sender.Nickname;
      Ident.Service.Stop();

      if (e.Message.Target != this.User.Nickname) {
        this.User.Nickname = e.Message.Target;
      }

    }

    private void keepOwnNickCorrect(object sender, IrcMessageEventArgs<NickChangeMessage> e) {
      if (e.Message.Sender.Nickname == this.User.Nickname) {
        this.User.Nickname = e.Message.NewNick;
      }
    }

    private void populateSupports(object sender, IrcMessageEventArgs<SupportMessage> e) {
      this.ServerSupports.LoadInfo(e.Message);
    }

    private void lookForReady(object sender, IrcMessageEventArgs<IrcMessage> e) {
      NumericMessage numeric = e.Message as NumericMessage;
      if (numeric == null) {
        return;
      }

      if (NumericMessage.IsDirect(numeric.InternalNumeric)) {
        return;
      }


      if (this.Ready != null) {
        this.readyRaised = true;
        this.Ready(this, EventArgs.Empty);
      }
    }

    private void messageParsed(object sender, IrcMessageEventArgs<IrcMessage> e) {

      if (!readyRaised) {
        lookForReady(sender, e);
      }

      bool routed = false;

      IChannelTargetedMessage channelMessage = e.Message as IChannelTargetedMessage;
      if (channelMessage != null) {
        foreach (Channel channel in this.Channels) {
          if (channelMessage.IsTargetedAtChannel(channel.Name)) {
            channel.Journal.Add(new JournalEntry(e.Message));
            routed = true;
          }
        }
      } else {
        IQueryTargetedMessage queryMessage = e.Message as IQueryTargetedMessage;
        if (queryMessage != null && queryMessage.IsQueryToUser(this.User)) {
          User msgSender = this.Peers.EnsureUser(e.Message.Sender);
          Query qry = this.Queries.EnsureQuery(msgSender, this);
          qry.Journal.Add(new JournalEntry(e.Message));
          routed = true;
        }
      }

      if (!routed) {
        this.ServerQuery.Journal.Add(new JournalEntry(e.Message));
      }

    }

    private void routeJoins(object sender, IrcMessageEventArgs<JoinMessage> e) {
      User msgUser = e.Message.Sender;
      User joinedUser = (IsMe(msgUser.Nickname)) ? User : this.Peers.EnsureUser(msgUser);

      foreach (string channelname in e.Message.Channels) {
        Channel joinedChannel = this.Channels.EnsureChannel(channelname, this);
        joinedChannel.Open = true;
        joinedChannel.Users.Add(joinedUser);
      }
    }

    private void routeKicks(object sender, IrcMessageEventArgs<KickMessage> e) {
      for (int i = 0; i < e.Message.Channels.Count; i++) {
        string channelName = e.Message.Channels[i];
        string nick = e.Message.Nicks[i];
        Channel channel = this.Channels.Find(channelName);

        if (IsMe(nick)) {
          // we don't want to actually remove the channel, but just close the channel
          // this allows a consumer to easily keep their reference to channels between kicks and re-joins.
          channel.Open = false;
        } else {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void routeKills(object sender, IrcMessageEventArgs<KillMessage> e) {
      string nick = e.Message.Nick;
      if (IsMe(nick)) {
        foreach (Channel c in this.Channels) {
          c.Open = false;
        }
      } else {
        foreach (Channel channel in this.Channels) {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void routeNames(object sender, IrcMessageEventArgs<NamesReplyMessage> e) {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel, this);
      foreach (string nick in e.Message.Nicks.Keys) {
        User user = this.Peers.EnsureUser(nick);
        if (!channel.Users.Contains(user)) {
          channel.Users.Add(user);
        }
        ChannelStatus status = e.Message.Nicks[nick];
        channel.SetStatusForUser(user, status);
      }
    }

    private void routeNicks(object sender, IrcMessageEventArgs<NickChangeMessage> e) {
      string oldNick = e.Message.Sender.Nickname;
      string newNick = e.Message.NewNick;
      if (IsMe(oldNick)) {
        this.User.Nickname = newNick;
      } else {
        User u = this.Peers.Find(oldNick);
        if (u != null) {
          u.Nickname = newNick;
        }
      }
    }

    private void routeParts(object sender, IrcMessageEventArgs<PartMessage> e) {
      string nick = e.Message.Sender.Nickname;
      foreach (string channelName in e.Message.Channels) {
        Channel channel = this.Channels.Find(channelName);
        if (IsMe(nick)) {
          channel.Open = false;
        } else {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void routeQuits(object sender, IrcMessageEventArgs<QuitMessage> e) {
      string nick = e.Message.Sender.Nickname;
      if (IsMe(nick)) {
        foreach (Channel c in this.Channels) {
          c.Open = false;
        }
      } else {
        foreach (Channel channel in this.Channels) {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void routeTopicNones(object sender, IrcMessageEventArgs<TopicNoneReplyMessage> e) {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null) {
        channel.Topic = "";
      }
    }

    private void routeTopics(object sender, IrcMessageEventArgs<TopicReplyMessage> e) {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null) {
        channel.Topic = e.Message.Topic;
      }
    }

    private void routeTopicSets(object sender, IrcMessageEventArgs<TopicSetReplyMessage> e) {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null) {
        User topicSetter = this.Peers.EnsureUser(e.Message.User);
        channel.TopicSetter = topicSetter;
        channel.TopicSetTime = e.Message.TimeSet;
      }
    }

    private void routeWhoReplies(object sender, IrcMessageEventArgs<WhoReplyMessage> e) {
      User whoUser = this.Peers.EnsureUser(e.Message.User);
      string channelName = e.Message.Channel;

      Channel channel = this.Channels.Find(channelName);
      if (channel != null) {
        if (!channel.Users.Contains(whoUser)) {
          channel.Users.Add(whoUser);
        }
        channel.SetStatusForUser(whoUser, e.Message.Status);
      }
    }

    private void client_NoSuchChannel(object sender, IrcMessageEventArgs<NoSuchChannelMessage> e) {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null) {
        channel.Open = false;
      }
    }

    private void client_NoSuchNick(object sender, IrcMessageEventArgs<NoSuchNickMessage> e) {
      string nick = e.Message.Nick;

      if (MessageUtil.HasValidChannelPrefix(nick)) { // NoSuchNickMessage is sent by some servers instead of a NoSuchChannelMessage
        Channel channel = this.Channels.Find(e.Message.Nick);
        if (channel != null) {
          channel.Open = false;
        }
      } else {
        this.Peers.RemoveFirst(nick);
        foreach (Channel channel in this.Channels) {
          channel.Users.RemoveFirst(nick);
        }
      }
    }

    private void client_ChannelModeIsReply(object sender, IrcMessageEventArgs<ChannelModeIsReplyMessage> e) {
      Channel channel = this.Channels.Find(e.Message.Channel);
      if (channel != null) {
        ChannelModesCreator modes = new ChannelModesCreator();
        modes.ServerSupport = this.ServerSupports;
        modes.Parse(e.Message.Modes, e.Message.ModeArguments);
        channel.Modes.ResetWith(modes.Modes);
      }

    }

    private void client_UserModeIsReply(object sender, IrcMessageEventArgs<UserModeIsReplyMessage> e) {
      if (IsMe(e.Message.Target)) {
        UserModesCreator modeCreator = new UserModesCreator();
        modeCreator.Parse(e.Message.Modes);
        this.User.Modes.Clear();
        foreach (UserMode mode in modeCreator.Modes) {
          this.User.Modes.Add(mode);
        }
      }
    }

    private void client_UserMode(object sender, IrcMessageEventArgs<UserModeMessage> e) {
      if (IsMe(e.Message.User)) {
        UserModesCreator modeCreator = new UserModesCreator();
        modeCreator.Parse(e.Message.ModeChanges);
        this.User.Modes.Clear();
        foreach (UserMode mode in modeCreator.Modes) {
          this.User.Modes.Add(mode);
        }
      }
    }

    private void client_UserHostReply(object sender, IrcMessageEventArgs<UserHostReplyMessage> e) {
      foreach (User sentUser in e.Message.Users) {
        if (IsMe(sentUser.Nickname)) {
          this.User.CopyFrom(sentUser);
        } else {
          User user = this.Peers.EnsureUser(sentUser);
          if (user != sentUser) {
            user.CopyFrom(sentUser);
          }
          if (!user.Away) {
            user.AwayMessage = string.Empty;
          }
        }
      }
    }

    private void client_UserAway(object sender, IrcMessageEventArgs<UserAwayMessage> e) {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.Away = true;
      user.AwayMessage = e.Message.Text;
    }

    private void client_SelfUnAway(object sender, IrcMessageEventArgs<SelfUnAwayMessage> e) {
      this.User.Away = false;
      this.User.AwayMessage = "";
    }

    private void client_SelfAway(object sender, IrcMessageEventArgs<SelfAwayMessage> e) {
      this.User.Away = true;
    }

    private void client_OperReply(object sender, IrcMessageEventArgs<OperReplyMessage> e) {
      this.User.IrcOperator = true;
    }

    private void client_ChannelProperty(object sender, IrcMessageEventArgs<ChannelPropertyMessage> e) {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel, this);
      channel.Properties[e.Message.Prop] = e.Message.NewValue;
    }

    private void client_ChannelPropertyReply(object sender, IrcMessageEventArgs<ChannelPropertyReplyMessage> e) {
      Channel channel = this.Channels.EnsureChannel(e.Message.Channel, this);
      channel.Properties[e.Message.Prop] = e.Message.Value;
    }

    private void client_Back(object sender, IrcMessageEventArgs<BackMessage> e) {
      User user = this.Peers.EnsureUser(e.Message.Sender);
      user.Away = false;
      user.AwayMessage = "";
    }

    private void client_Away(object sender, IrcMessageEventArgs<AwayMessage> e) {
      User user = this.Peers.EnsureUser(e.Message.Sender);
      user.Away = true;
      user.AwayMessage = e.Message.Reason;
    }

    private void client_WhoIsUserReply(object sender, IrcMessageEventArgs<WhoIsUserReplyMessage> e) {
      User target = this.Peers.EnsureUser(e.Message.User);
      target.CopyFrom(e.Message.User);
    }

    private void client_WhoIsServerReply(object sender, IrcMessageEventArgs<WhoIsServerReplyMessage> e) {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.Server = e.Message.ServerName;
    }

    private void client_WhoIsOperReply(object sender, IrcMessageEventArgs<WhoIsOperReplyMessage> e) {
      User user = this.Peers.EnsureUser(e.Message.Nick);
      user.IrcOperator = true;
    }






    private void logGenericCtcp(object sender, IrcMessageEventArgs<GenericCtcpRequestMessage> e) {
      logUnimplementedMessage(e.Message);
    }
    private void logGenericMessage(object sender, IrcMessageEventArgs<GenericMessage> e) {
      logUnimplementedMessage(e.Message);
    }
    private void logGenericNumeric(object sender, IrcMessageEventArgs<GenericNumericMessage> e) {
      logUnimplementedMessage(e.Message);
    }
    private void logGenericError(object sender, IrcMessageEventArgs<GenericErrorMessage> e) {
      logUnimplementedMessage(e.Message);
    }

    private void logUnimplementedMessage(IrcMessage msg) {
      System.Diagnostics.Trace.WriteLine(msg.ToString(), "Unimplemented Message");
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private void selfDie(object sender, EventArgs e) {
      if (this.Connection != null && this.Connection.Status != ConnectionStatus.Disconnected) {
        this.Connection.DataReceived -= new EventHandler<ConnectionDataEventArgs>(this.dataReceived);
        this.Connection.DataSent -= new EventHandler<ConnectionDataEventArgs>(this.dataSent);
        this.Die();
      }
    }

    #endregion

    #region Private

    private bool readyRaised = false;
    IrcMessageWriter writer = new IrcMessageWriter();

    private void StartIdent() {
      if (this.EnableAutoIdent) {
        Ident.Service.User = this.User;
        Ident.Service.Start(true);
        DateTime started = DateTime.Now;
        TimeSpan tooMuchTime = new TimeSpan(0, 0, 5);
        while (Ident.Service.Status != ConnectionStatus.Connected && DateTime.Now.Subtract(started) < tooMuchTime) {
          System.Threading.Thread.Sleep(25);
        }
        if (Ident.Service.Status != ConnectionStatus.Connected) {
          System.Diagnostics.Trace.WriteLine("Ident Failed To AutoStart", "Ident");
        }
      }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private void StopIdent() {
      Ident.Service.Stop();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    private void Die() {
      this.SendQuit(this.DefaultQuitMessage);
      this.Connection.DisconnectForce();
    }

    #endregion

  }

}