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
        private readonly Dictionary<User, ChannelStatus> _userModes;
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
        public Channel(string name)
        {
            this._open = false;

            this.Properties = new NameValueCollection(2);
            this.Properties["NAME"] = name;
            this.Properties["TOPIC"] = string.Empty;

            this.Users = new UserCollection();
            this.Users.CollectionChanged += this.UsersCollectionChanged;

            this._userModes = new Dictionary<User, ChannelStatus>();

            this.Modes = new ChannelModeCollection();
            this.Modes.CollectionChanged += (s, e) => this.OnPropertyChanged("Modes");

            this.Journal = new Journal();
            this.Journal.CollectionChanged += (s, e) => this.OnPropertyChanged("Journal");
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
                return this._open;
            }
            internal set
            {
                this._open = value;
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
                return this.Properties["NAME"];
            }
            set
            {
                if (this.Properties["NAME"] != value)
                {
                    this.Properties["NAME"] = value;
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
                if (this.Properties["TOPIC"] != value)
                {
                    this.Properties["TOPIC"] = value;
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
                return this._topicSetter;
            }
            set
            {
                if (this._topicSetter != value)
                {
                    this._topicSetter = value;
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
                return this._topicSetTime;
            }
            set
            {
                if (this._topicSetTime != value)
                {
                    this._topicSetTime = value;
                    this.OnPropertyChanged("TopicSetTime");
                }
            }
        }

        /// <summary>
        ///   Gets the collection of general properties assigned to this channel.
        /// </summary>
        public NameValueCollection Properties
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the users in the channel.
        /// </summary>
        public UserCollection Users
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the modes in the channel.
        /// </summary>
        public ChannelModeCollection Modes
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the journal of messages on the channel.
        /// </summary>
        public Journal Journal
        {
            get;
            private set;
        }

        #endregion


        #region Users Status

        /// <summary>
        ///   Gets the status for the given <see cref="User" /> in the channel.
        /// </summary>
        public ChannelStatus GetStatusForUser(User channelUser)
        {
            this.VerifyUserInChannel(channelUser);
            return this._userModes.ContainsKey(channelUser) ? this._userModes[channelUser] : ChannelStatus.None;
        }

        /// <summary>
        ///   Applies the given <see cref="ChannelStatus" /> to the given <see cref="User" /> in the channel.
        /// </summary>
        public void SetStatusForUser(User channelUser, ChannelStatus status)
        {
            if (status == ChannelStatus.None && this._userModes.ContainsKey(channelUser))
            {
                this._userModes.Remove(channelUser);
            }
            else
            {
                this.VerifyUserInChannel(channelUser);
                this._userModes[channelUser] = status;
            }
        }

        #endregion


        #region Private Methods

        private void VerifyUserInChannel(Mask channelUser)
        {
            if (channelUser == null)
            {
                throw new ArgumentNullException("channelUser");
            }
            if (!this.Users.ContainsKey(channelUser.Nickname))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.UserIsNotInChannel, channelUser.Nickname, this.Name), "channelUser");
            }
        }

        private void UsersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (User user in e.OldItems.Cast<User>().Where(user => this._userModes.ContainsKey(user)))
                {
                    this._userModes.Remove(user);
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
