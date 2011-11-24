using System;
using System.Text;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc
{
  /// <summary>
  ///   Represents a User on an IRC server.
  /// </summary>
  [Serializable]
  public class User : Mask
  {
    private bool away;
    private string awayMessage;
    private bool ircOperator;
    private string name;
    private bool online;
    private string password;
    private string server;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class.
    /// </summary>
    public User()
      : base(string.Empty, string.Empty, string.Empty)
    {
      this.Initialize();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class with the given mask string.
    /// </summary>
    /// <param name="mask">The mask string to parse.</param>
    public User(string mask)
      : base(mask)
    {
      this.Initialize();
    }

    private void Initialize()
    {
      this.name = string.Empty;
      this.password = string.Empty;
      this.server = string.Empty;
      this.ircOperator = false;
      this.online = true;
      this.away = false;
      this.awayMessage = string.Empty;

      this.Modes = new UserModeCollection();
      this.Modes.CollectionChanged += (s, e) => this.OnPropertyChanged("Modes");
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the supposed real name of the User.
    /// </summary>
    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        if (this.name != value)
        {
          this.name = value;
          this.OnPropertyChanged("Name");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the Password the User will use on the server.
    /// </summary>
    public string Password
    {
      get
      {
        return this.password;
      }
      set
      {
        if (this.password != value)
        {
          this.password = value;
          this.OnPropertyChanged("Password");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the name of the server which the User is connected to.
    /// </summary>
    public string Server
    {
      get
      {
        return this.server;
      }
      set
      {
        if (this.server != value)
        {
          this.server = value;
          this.OnPropertyChanged("Server");
        }
      }
    }

    /// <summary>
    ///   Gets or sets if the User is an IRC Operator.
    /// </summary>
    public bool IrcOperator
    {
      get
      {
        return this.ircOperator;
      }
      set
      {
        if (this.ircOperator != value)
        {
          this.ircOperator = value;
          this.OnPropertyChanged("IrcOperator");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the online status of this User.
    /// </summary>
    public bool Online
    {
      get
      {
        return this.online;
      }
      set
      {
        if (this.online != value)
        {
          this.online = value;
          this.OnPropertyChanged("Online");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away status of this User.
    /// </summary>
    public bool Away
    {
      get
      {
        return this.away;
      }
      set
      {
        if (this.away != value)
        {
          this.away = value;
          this.OnPropertyChanged("Away");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the away message of this User.
    /// </summary>
    public string AwayMessage
    {
      get
      {
        return this.awayMessage;
      }
      set
      {
        if (this.awayMessage != value)
        {
          this.awayMessage = value;
          this.OnPropertyChanged("AwayMessage");
        }
      }
    }

    /// <summary>
    ///   Gets the modes which apply to the user.
    /// </summary>
    public UserModeCollection Modes
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets a string that uniquely identifies this user.
    /// </summary>
    public string FingerPrint
    {
      get
      {
        if (string.IsNullOrEmpty(this.Host) || string.IsNullOrEmpty(this.Username))
        {
          return string.Empty;
        }

        int indexOfPoint = this.Host.IndexOf('.');
        if (indexOfPoint > 0)
        {
          return this.Username.TrimStart('~') + "@*" + this.Host.Substring(indexOfPoint);
        }
        return this.Username.TrimStart('~') + "@" + this.Host;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Represents this User's information.
    /// </summary>
    public override String ToString() {
      var result = new StringBuilder(this.Nickname);

      if (!string.IsNullOrEmpty(this.Username)) {
        result.Append('!');
        result.Append(this.Username);
      }

      if (!string.IsNullOrEmpty(this.Host)) {
        result.Append('@');
        result.Append(this.Host);
      }

      return result.ToString();
    }

    /// <summary>
    ///   Copies the properties of the given User onto this User.
    /// </summary>
    public void CopyFrom(User user)
    {
      this.Host = user.Host;
      this.Nickname = user.Nickname;
      this.Password = user.Password;
      this.Name = user.Name;
      this.Username = user.Username;
      this.Server = user.Server;
      this.IrcOperator = user.IrcOperator;
      this.Online = user.Online;
      this.Away = user.Away;
      this.AwayMessage = user.AwayMessage;
    }

    #endregion
  }
}
