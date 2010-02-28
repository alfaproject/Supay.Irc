using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Supay.Irc.Network;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc {

  /// <summary>
  ///   Represents a single irc channel, with it's users. </summary>
  [Serializable]
  public class Channel : INotifyPropertyChanged {

    private Client _client;
    private User _topicSetter;
    private DateTime _topicSetTime;
    private UserCollection _users = new UserCollection();
    private ChannelModeCollection _modes = new ChannelModeCollection();
    private Journal _journal = new Journal();
    private UserStatusMap _userModes = new UserStatusMap();
    private NameValueCollection _properties = new NameValueCollection();

    /// <summary>
    ///   The event raised when a property on the object changes. </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #region ctor

    /// <summary>
    /// Creates a new instance of the <see cref="Channel"/> class on the given client.
    /// </summary>
    public Channel(Client client) {
      this._client = client;
      this._users.CollectionChanged += new NotifyCollectionChangedEventHandler(users_CollectionChanged);
      this.Modes.CollectionChanged += new NotifyCollectionChangedEventHandler(Modes_CollectionChanged);
      this._journal.CollectionChanged += new NotifyCollectionChangedEventHandler(journal_CollectionChanged);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Channel"/> class on the given client with the given name.
    /// </summary>
    public Channel(Client client, String name)
      : this(client) {
      this.Name = name;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the collection of general properties assigned to this channel
    /// </summary>
    public virtual NameValueCollection Properties {
      get {
        return _properties;
      }
    }

    /// <summary>
    /// Gets the client which the channel is on.
    /// </summary>
    public virtual Client Client {
      get {
        return _client;
      }
    }


    /// <summary>
    /// Gets or sets whether the channel is currently open
    /// </summary>
    public bool Open {
      get {
        return _open && (this.Client != null && this.Client.Connection.Status == ConnectionStatus.Connected);
      }
      internal set {
        _open = value;
        this.RaisePropertyChanged("Open");
      }
    }
    private bool _open = false;

    /// <summary>
    /// Gets or sets the name of the channel.
    /// </summary>
    public virtual String Name {
      get {
        return this.Properties["NAME"] ?? "";
      }
      set {
        String currentValue = this.Name;
        if (currentValue != value) {
          this.Properties["NAME"] = value;
          this.RaisePropertyChanged("Name");
        }
      }
    }

    /// <summary>
    /// Gets or sets the topic of the channel
    /// </summary>
    public virtual String Topic {
      get {
        return this.Properties["TOPIC"] ?? "";
      }
      set {
        String originalValue = this.Topic;
        if (originalValue != value) {
          this.Properties["TOPIC"] = value;
          this.RaisePropertyChanged("Topic");
        }
      }
    }

    /// <summary>
    /// Gets or sets the user which set the current topic
    /// </summary>
    public virtual User TopicSetter {
      get {
        return _topicSetter;
      }
      set {
        if (_topicSetter != value) {
          _topicSetter = value;
          this.RaisePropertyChanged("TopicSetter");
        }
      }
    }

    /// <summary>
    /// Gets or sets the time which topic was set
    /// </summary>
    public virtual DateTime TopicSetTime {
      get {
        return _topicSetTime;
      }
      set {
        if (_topicSetTime != value) {
          _topicSetTime = value;
          this.RaisePropertyChanged("TopicSetTime");
        }
      }
    }

    /// <summary>
    /// Gets the users in the channel.
    /// </summary>
    public virtual UserCollection Users {
      get {
        return _users;
      }
    }

    /// <summary>
    /// Gets the modes in the channel.
    /// </summary>
    public virtual Supay.Irc.Messages.Modes.ChannelModeCollection Modes {
      get {
        return _modes;
      }
    }

    /// <summary>
    /// Gets the journal of messages on the channel
    /// </summary>
    public virtual Journal Journal {
      get {
        return _journal;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets the status for the given <see cref="T:User"/> in the channel.
    /// </summary>
    public virtual ChannelStatus GetStatusForUser(User channelUser) {
      VerifyUserInChannel(channelUser);
      if (_userModes.ContainsKey(channelUser)) {
        return _userModes[channelUser];
      }
      return ChannelStatus.None;
    }

    /// <summary>
    /// Applies the given <see cref="T:ChannelStatus"/> to the given <see cref="T:User"/> in the channel.
    /// </summary>
    public virtual void SetStatusForUser(User channelUser, ChannelStatus status) {
      if (status == ChannelStatus.None && _userModes.ContainsKey(channelUser)) {
        _userModes.Remove(channelUser);
      } else {
        VerifyUserInChannel(channelUser);
        _userModes[channelUser] = status;
      }
    }

    private void VerifyUserInChannel(User channelUser) {
      if (channelUser == null) {
        throw new ArgumentNullException("channelUser");
      }
      if (!Users.Contains(channelUser)) {
        throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Supay.Irc.Properties.Resources.UserIsNotInChannel, channelUser.Nickname, this.Name), "channelUser");
      }
    }

    #endregion

    #region Private

    void users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      UserCollection users = this.Users;

      if (e.Action == NotifyCollectionChangedAction.Remove) {
        this._userModes.RemoveAll(delegate(KeyValuePair<User, ChannelStatus> keyValue) {
          return !users.Contains(keyValue.Key);
        }
        );
      }
      this.RaisePropertyChanged("Users");
    }

    void Modes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      this.RaisePropertyChanged("Modes");
    }

    void journal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      this.RaisePropertyChanged("Journal");
    }

    #endregion

    #region UserStatusMap

    private class UserStatusMap : Dictionary<User, ChannelStatus> {

      /// <summary>
      /// Removes all of the items from the dictionary which the given predictate matches.
      /// </summary>
      /// <returns>The number of items removed from the dictionary.</returns>
      public int RemoveAll(Predicate<System.Collections.Generic.KeyValuePair<User, ChannelStatus>> match) {
        if (match == null) {
          throw new ArgumentNullException("match");
        }
        int countOfItemsRemoved = 0;

        User[] users = new User[this.Keys.Count];
        this.Keys.CopyTo(users, 0);
        foreach (User u in users) {
          if (this.ContainsKey(u) && match(new KeyValuePair<User, ChannelStatus>(u, this[u]))) {
            this.Remove(u);
            countOfItemsRemoved++;
          }
        }

        return countOfItemsRemoved;

      }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    ///   Raises the PropertyChanged event. </summary>
    protected void RaisePropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  } //class Channel
} //namespace Supay.Irc