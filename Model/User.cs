using System;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc {

  /// <summary>
  ///   Represents a User on an IRC server. </summary>
  [Serializable]
  public class User : Mask {

    private string _name;
    private string _password;
    private UserOnlineStatus _onlineStatus;
    private string _server;
    private bool _ircOperator;
    private bool _away;
    private string _awayMessage;
    private UserModeCollection _modes;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class. </summary>
    public User()
      : base(string.Empty, string.Empty, string.Empty) {
      this.Initialize();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="User"/> class with the given mask string. </summary>
    /// <param name="mask">
    ///   The mask string to parse. </param>
    public User(string mask)
      : base(mask) {
      this.Initialize();
    }

    private void Initialize() {
      _name = string.Empty;
      _password = string.Empty;
      _onlineStatus = UserOnlineStatus.Online;
      _server = string.Empty;
      _ircOperator = false;
      _away = false;
      _awayMessage = string.Empty;

      _modes = new UserModeCollection();
      _modes.CollectionChanged += (s, e) => this.RaisePropertyChanged("Modes");
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the supposed real name of the User. </summary>
    public string Name {
      get {
        return _name;
      }
      set {
        if (_name != value) {
          _name = value;
          this.RaisePropertyChanged("Name");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the Password the User will use on the server. </summary>
    public string Password {
      get {
        return _password;
      }
      set {
        if (_password != value) {
          _password = value;
          this.RaisePropertyChanged("Password");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the online status of this User. </summary>
    public UserOnlineStatus OnlineStatus {
      get {
        return _onlineStatus;
      }
      set {
        if (_onlineStatus != value) {
          _onlineStatus = value;
          this.RaisePropertyChanged("OnlineStatus");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the name of the server which the User is connected to. </summary>
    public string Server {
      get {
        return _server;
      }
      set {
        if (_server != value) {
          _server = value;
          this.RaisePropertyChanged("Server");
        }
      }
    }

    /// <summary>
    ///   Gets or sets if the User is an IRC Operator. </summary>
    public bool IrcOperator {
      get {
        return _ircOperator;
      }
      set {
        if (_ircOperator != value) {
          _ircOperator = value;
          this.RaisePropertyChanged("IrcOperator");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away status of this User. </summary>
    public bool Away {
      get {
        return _away;
      }
      set {
        if (_away != value) {
          _away = value;
          this.RaisePropertyChanged("Away");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away message of this User. </summary>
    public string AwayMessage {
      get {
        return _awayMessage;
      }
      set {
        if (_awayMessage != value) {
          _awayMessage = value;
          this.RaisePropertyChanged("AwayMessage");
        }
      }
    }

    /// <summary>
    ///   Gets the modes which apply to the user. </summary>
    public UserModeCollection Modes {
      get {
        return _modes;
      }
    }

    /// <summary>
    ///   Gets a string that uniquely identifies this user. </summary>
    public string FingerPrint {
      get {
        if (string.IsNullOrEmpty(this.Host) || string.IsNullOrEmpty(this.Username)) {
          return string.Empty;
        }

        int indexOfPoint = this.Host.IndexOf('.');
        if (indexOfPoint > 0) {
          return this.Username.TrimStart('~') + "@*" + this.Host.Substring(indexOfPoint);
        } else {
          return this.Username.TrimStart('~') + "@" + this.Host;
        }
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Represents this User's information as an IRC mask. </summary>
    public override string ToString() {
      return this.IrcMask;
    }

    /// <summary>
    ///   Copies the properties of the given User onto this User. </summary>
    public void CopyFrom(User user) {
      this.OnlineStatus = user.OnlineStatus;
      this.Host = user.Host;
      this.Nickname = user.Nickname;
      this.Password = user.Password;
      this.Name = user.Name;
      this.Username = user.Username;
      this.Server = user.Server;
      this.IrcOperator = user.IrcOperator;
      this.Away = user.Away;
      this.AwayMessage = user.AwayMessage;
    }

    #endregion

  } //class User
} //namespace Supay.Irc