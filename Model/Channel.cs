using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using Supay.Irc.Messages.Modes;
using Supay.Irc.Properties;

namespace Supay.Irc {
  /// <summary>
  ///   Represents a single irc channel, with it's users.
  /// </summary>
  [Serializable]
  public class Channel : INotifyPropertyChanged {
    private readonly Journal _journal;
    private readonly ChannelModeCollection _modes;
    private readonly NameValueCollection _properties;
    private readonly Dictionary<User, ChannelStatus> _userModes;
    private readonly UserCollection _users;
    private bool _open;
    private DateTime _topicSetTime;
    private User _topicSetter;

    #region INotifyPropertyChanged Members

    /// <summary>
    ///   The event raised when a property on the object changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Constructors

    /// <summary>
    ///   Creates a new instance of the <see cref="Channel" /> class on the given client with the given name.
    /// </summary>
    public Channel(string name) {
      _open = false;

      _properties = new NameValueCollection(2);
      _properties["NAME"] = name;
      _properties["TOPIC"] = string.Empty;

      _users = new UserCollection();
      _users.CollectionChanged += _users_CollectionChanged;

      _userModes = new Dictionary<User, ChannelStatus>();

      _modes = new ChannelModeCollection();
      _modes.CollectionChanged += (s, e) => this.OnPropertyChanged("Modes");

      _journal = new Journal();
      _journal.CollectionChanged += (s, e) => this.OnPropertyChanged("Journal");
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="Channel" /> class on the given client.
    /// </summary>
    public Channel()
      : this(string.Empty) {
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets whether the channel is currently open.
    /// </summary>
    public bool Open {
      get {
        return _open;
      }
      internal set {
        _open = value;
        this.OnPropertyChanged("Open");
      }
    }

    /// <summary>
    ///   Gets or sets the name of the channel.
    /// </summary>
    public string Name {
      get {
        return _properties["NAME"];
      }
      set {
        if (_properties["NAME"] != value) {
          _properties["NAME"] = value;
          this.OnPropertyChanged("Name");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the topic of the channel.
    /// </summary>
    public string Topic {
      get {
        return Properties["TOPIC"];
      }
      set {
        if (_properties["TOPIC"] != value) {
          _properties["TOPIC"] = value;
          this.OnPropertyChanged("Topic");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the user which set the current topic.
    /// </summary>
    public User TopicSetter {
      get {
        return _topicSetter;
      }
      set {
        if (_topicSetter != value) {
          _topicSetter = value;
          this.OnPropertyChanged("TopicSetter");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the time which topic was set.
    /// </summary>
    public DateTime TopicSetTime {
      get {
        return _topicSetTime;
      }
      set {
        if (_topicSetTime != value) {
          _topicSetTime = value;
          this.OnPropertyChanged("TopicSetTime");
        }
      }
    }

    /// <summary>
    ///   Gets the collection of general properties assigned to this channel.
    /// </summary>
    public NameValueCollection Properties {
      get {
        return _properties;
      }
    }

    /// <summary>
    ///   Gets the users in the channel.
    /// </summary>
    public UserCollection Users {
      get {
        return _users;
      }
    }

    /// <summary>
    ///   Gets the modes in the channel.
    /// </summary>
    public ChannelModeCollection Modes {
      get {
        return _modes;
      }
    }

    /// <summary>
    ///   Gets the journal of messages on the channel.
    /// </summary>
    public Journal Journal {
      get {
        return _journal;
      }
    }

    #endregion

    #region Users Status

    /// <summary>
    ///   Gets the status for the given <see cref="User" /> in the channel.
    /// </summary>
    public ChannelStatus GetStatusForUser(User channelUser) {
      VerifyUserInChannel(channelUser);
      if (_userModes.ContainsKey(channelUser)) {
        return _userModes[channelUser];
      }
      return ChannelStatus.None;
    }

    /// <summary>
    ///   Applies the given <see cref="ChannelStatus" /> to the given <see cref="User" /> in the channel.
    /// </summary>
    public void SetStatusForUser(User channelUser, ChannelStatus status) {
      if (status == ChannelStatus.None && _userModes.ContainsKey(channelUser)) {
        _userModes.Remove(channelUser);
      } else {
        VerifyUserInChannel(channelUser);
        _userModes[channelUser] = status;
      }
    }

    #endregion

    #region Private Methods

    private void VerifyUserInChannel(User channelUser) {
      if (channelUser == null) {
        throw new ArgumentNullException("channelUser");
      }
      if (!_users.Contains(channelUser)) {
        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.UserIsNotInChannel, channelUser.Nickname, Name), "channelUser");
      }
    }

    private void _users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      if (e.Action == NotifyCollectionChangedAction.Remove) {
        foreach (User user in e.OldItems) {
          if (_userModes.ContainsKey(user)) {
            _userModes.Remove(user);
          }
        }
      }

      this.OnPropertyChanged("Users");
    }

    #endregion

    #region Protected Methods

    /// <summary>
    ///   Raises the PropertyChanged event.
    /// </summary>
    protected void OnPropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
