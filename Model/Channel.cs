using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Supay.Irc.Messages.Modes;
using Supay.Irc.Properties;

namespace Supay.Irc
{
  /// <summary>
  ///   Represents a single irc channel, with it's users.
  /// </summary>
  [Serializable]
  public class Channel : INotifyPropertyChanged
  {
    private readonly Journal journal;
    private readonly ChannelModeCollection modes;
    private readonly NameValueCollection properties;
    private readonly Dictionary<User, ChannelStatus> userModes;
    private readonly UserCollection users;
    private bool open;
    private DateTime topicSetTime;
    private User topicSetter;

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
    public Channel(string name)
    {
      this.open = false;

      this.properties = new NameValueCollection(2);
      this.properties["NAME"] = name;
      this.properties["TOPIC"] = string.Empty;

      this.users = new UserCollection();
      this.users.CollectionChanged += this.UsersCollectionChanged;

      this.userModes = new Dictionary<User, ChannelStatus>();

      this.modes = new ChannelModeCollection();
      this.modes.CollectionChanged += (s, e) => this.OnPropertyChanged("Modes");

      this.journal = new Journal();
      this.journal.CollectionChanged += (s, e) => this.OnPropertyChanged("Journal");
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="Channel" /> class on the given client.
    /// </summary>
    public Channel()
      : this(string.Empty)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets whether the channel is currently open.
    /// </summary>
    public bool Open
    {
      get
      {
        return this.open;
      }
      internal set
      {
        this.open = value;
        this.OnPropertyChanged("Open");
      }
    }

    /// <summary>
    ///   Gets or sets the name of the channel.
    /// </summary>
    public string Name
    {
      get
      {
        return this.properties["NAME"];
      }
      set
      {
        if (this.properties["NAME"] != value)
        {
          this.properties["NAME"] = value;
          this.OnPropertyChanged("Name");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the topic of the channel.
    /// </summary>
    public string Topic
    {
      get
      {
        return this.Properties["TOPIC"];
      }
      set
      {
        if (this.properties["TOPIC"] != value)
        {
          this.properties["TOPIC"] = value;
          this.OnPropertyChanged("Topic");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the user which set the current topic.
    /// </summary>
    public User TopicSetter
    {
      get
      {
        return this.topicSetter;
      }
      set
      {
        if (this.topicSetter != value)
        {
          this.topicSetter = value;
          this.OnPropertyChanged("TopicSetter");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the time which topic was set.
    /// </summary>
    public DateTime TopicSetTime
    {
      get
      {
        return this.topicSetTime;
      }
      set
      {
        if (this.topicSetTime != value)
        {
          this.topicSetTime = value;
          this.OnPropertyChanged("TopicSetTime");
        }
      }
    }

    /// <summary>
    ///   Gets the collection of general properties assigned to this channel.
    /// </summary>
    public NameValueCollection Properties
    {
      get
      {
        return this.properties;
      }
    }

    /// <summary>
    ///   Gets the users in the channel.
    /// </summary>
    public UserCollection Users
    {
      get
      {
        return this.users;
      }
    }

    /// <summary>
    ///   Gets the modes in the channel.
    /// </summary>
    public ChannelModeCollection Modes
    {
      get
      {
        return this.modes;
      }
    }

    /// <summary>
    ///   Gets the journal of messages on the channel.
    /// </summary>
    public Journal Journal
    {
      get
      {
        return this.journal;
      }
    }

    #endregion

    #region Users Status

    /// <summary>
    ///   Gets the status for the given <see cref="User" /> in the channel.
    /// </summary>
    public ChannelStatus GetStatusForUser(User channelUser)
    {
      this.VerifyUserInChannel(channelUser);
      return this.userModes.ContainsKey(channelUser) ? this.userModes[channelUser] : ChannelStatus.None;
    }

    /// <summary>
    ///   Applies the given <see cref="ChannelStatus" /> to the given <see cref="User" /> in the channel.
    /// </summary>
    public void SetStatusForUser(User channelUser, ChannelStatus status)
    {
      if (status == ChannelStatus.None && this.userModes.ContainsKey(channelUser))
      {
        this.userModes.Remove(channelUser);
      }
      else
      {
        this.VerifyUserInChannel(channelUser);
        this.userModes[channelUser] = status;
      }
    }

    #endregion

    #region Private Methods

    private void VerifyUserInChannel(User channelUser)
    {
      if (channelUser == null)
      {
        throw new ArgumentNullException("channelUser");
      }
      if (!this.users.Contains(channelUser))
      {
        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.UserIsNotInChannel, channelUser.Nickname, this.Name), "channelUser");
      }
    }

    private void UsersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (User user in e.OldItems.Cast<User>().Where(user => this.userModes.ContainsKey(user)))
        {
          this.userModes.Remove(user);
        }
      }

      this.OnPropertyChanged("Users");
    }

    #endregion

    #region Protected Methods

    /// <summary>
    ///   Raises the PropertyChanged event.
    /// </summary>
    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
