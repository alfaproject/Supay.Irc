using System;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc {
  /// <summary>
  ///   Represents a User on an IRC server.
  /// </summary>
  [Serializable]
  public class User : Mask {
    private bool _away;
    private string _awayMessage;
    private bool _ircOperator;
    private UserModeCollection _modes;
    private string _name;
    private bool _online;
    private string _password;
    private string _server;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class.
    /// </summary>
    public User()
      : base(string.Empty, string.Empty, string.Empty) {
      Initialize();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class with the given mask string.
    /// </summary>
    /// <param name="mask">The mask string to parse.</param>
    public User(string mask)
      : base(mask) {
      Initialize();
    }

    private void Initialize() {
      _name = string.Empty;
      _password = string.Empty;
      _server = string.Empty;
      _ircOperator = false;
      _online = true;
      _away = false;
      _awayMessage = string.Empty;

      _modes = new UserModeCollection();
      _modes.CollectionChanged += (s, e) => RaisePropertyChanged("Modes");
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the supposed real name of the User.
    /// </summary>
    public string Name {
      get {
        return _name;
      }
      set {
        if (_name != value) {
          _name = value;
          RaisePropertyChanged("Name");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the Password the User will use on the server.
    /// </summary>
    public string Password {
      get {
        return _password;
      }
      set {
        if (_password != value) {
          _password = value;
          RaisePropertyChanged("Password");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the name of the server which the User is connected to.
    /// </summary>
    public string Server {
      get {
        return _server;
      }
      set {
        if (_server != value) {
          _server = value;
          RaisePropertyChanged("Server");
        }
      }
    }

    /// <summary>
    ///   Gets or sets if the User is an IRC Operator.
    /// </summary>
    public bool IrcOperator {
      get {
        return _ircOperator;
      }
      set {
        if (_ircOperator != value) {
          _ircOperator = value;
          RaisePropertyChanged("IrcOperator");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the online status of this User.
    /// </summary>
    public bool Online {
      get {
        return _online;
      }
      set {
        if (_online != value) {
          _online = value;
          RaisePropertyChanged("Online");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away status of this User.
    /// </summary>
    public bool Away {
      get {
        return _away;
      }
      set {
        if (_away != value) {
          _away = value;
          RaisePropertyChanged("Away");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away message of this User.
    /// </summary>
    public string AwayMessage {
      get {
        return _awayMessage;
      }
      set {
        if (_awayMessage != value) {
          _awayMessage = value;
          RaisePropertyChanged("AwayMessage");
        }
      }
    }

    /// <summary>
    ///   Gets the modes which apply to the user.
    /// </summary>
    public UserModeCollection Modes {
      get {
        return _modes;
      }
    }

    /// <summary>
    ///   Gets a string that uniquely identifies this user.
    /// </summary>
    public string FingerPrint {
      get {
        if (string.IsNullOrEmpty(Host) || string.IsNullOrEmpty(Username)) {
          return string.Empty;
        }

        int indexOfPoint = Host.IndexOf('.');
        if (indexOfPoint > 0) {
          return Username.TrimStart('~') + "@*" + Host.Substring(indexOfPoint);
        }
        return Username.TrimStart('~') + "@" + Host;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Represents this User's information as an IRC mask.
    /// </summary>
    public override string ToString() {
      return IrcMask;
    }

    /// <summary>
    ///   Copies the properties of the given User onto this User.
    /// </summary>
    public void CopyFrom(User user) {
      Host = user.Host;
      Nickname = user.Nickname;
      Password = user.Password;
      Name = user.Name;
      Username = user.Username;
      Server = user.Server;
      IrcOperator = user.IrcOperator;
      Online = user.Online;
      Away = user.Away;
      AwayMessage = user.AwayMessage;
    }

    #endregion
  }
}
