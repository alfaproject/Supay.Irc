using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Supay.Core;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc {

  /// <summary>
  ///   Represents a User on an IRC server. </summary>
  [Serializable]
  public class User : INotifyPropertyChanged {

    /// <summary>
    ///   Raised when a property on the instance has changed. </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    private ChangeNotifier<string> _nickname;
    private ChangeNotifier<string> _name;
    private ChangeNotifier<string> _password;
    private ChangeNotifier<string> _username;
    private ChangeNotifier<string> _host;
    private ChangeNotifier<UserOnlineStatus> _onlineStatus;
    private ChangeNotifier<string> _awayMessage;
    private ChangeNotifier<string> _server;
    private ChangeNotifier<bool> _ircOperator;
    private ChangeNotifier<UserModeCollection> _modes;

    #region CTor

    /// <summary>
    ///   Initializes a new instance of the <see cref="User" /> class. </summary>
    public User() {
      ChangeNotifier notifier = new ChangeNotifier(() => this.PropertyChanged);
      _nickname = notifier.Create(() => this.Nick, string.Empty);
      _name = notifier.Create(() => this.Name, string.Empty);
      _password = notifier.Create(() => this.Password, string.Empty);
      _username = notifier.Create(() => this.Username, string.Empty);
      _host = notifier.Create(() => this.Host, string.Empty);
      _onlineStatus = notifier.Create(() => this.OnlineStatus, UserOnlineStatus.Online);
      _awayMessage = notifier.Create(() => this.AwayMessage);
      _server = notifier.Create(() => this.Server);
      _ircOperator = notifier.Create(() => this.IrcOperator);
      _modes = notifier.Create(() => this.Modes, new UserModeCollection());

      // Mask and FingerPrint change notification depends on Nickname, Username and/or Host changes.
      notifier.CreateDependent(() => this.Mask, () => this.Nick, () => this.Username, () => this.Host);
      notifier.CreateDependent(() => this.FingerPrint, () => this.Username, () => this.Host);
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="User"/> class with the given mask string. </summary>
    /// <param name="mask">
    ///   The mask string to parse. </param>
    public User(string mask)
      : this() {
      Parse(mask);
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the nickname of the User. </summary>
    public string Nick {
      get { return _nickname.Value; }
      set { _nickname.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the supposed real name of the User. </summary>
    public string Name {
      get { return _name.Value; }
      set { _name.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the Password the User will use on the server. </summary>
    public string Password {
      get { return _password.Value; }
      set { _password.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the username of the User on its local server. </summary>
    public string Username {
      get { return _username.Value; }
      set { _username.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the hostname of the local machine of this User. </summary>
    public string Host {
      get { return _host.Value; }
      set { _host.Value = value; }
    }

    /// <summary>
    ///   Gets this User's information with a guarenteed nickname!username@host format. </summary>
    public string Mask {
      get {
        return (string.IsNullOrEmpty(this.Nick) ? "*" : this.Nick) +
          "!" + (string.IsNullOrEmpty(this.Username) ? "*" : this.Username) +
          "@" + (string.IsNullOrEmpty(this.Host) ? "*" : this.Host);
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

    /// <summary>
    ///   Gets or sets the online status of this User. </summary>
    public UserOnlineStatus OnlineStatus {
      get { return _onlineStatus.Value; }
      set { _onlineStatus.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the away message of this User. </summary>
    public string AwayMessage {
      get { return _awayMessage.Value; }
      set { _awayMessage.Value = value; }
    }

    /// <summary>
    ///   Gets or sets the name of the server which the User is connected to. </summary>
    public string Server {
      get { return _server.Value; }
      set { _server.Value = value; }
    }

    /// <summary>
    ///   Gets or sets if the User is an IRC Operator. </summary>
    public bool IrcOperator {
      get { return _ircOperator.Value; }
      set { _ircOperator.Value = value; }
    }

    /// <summary>
    ///   Gets the modes which apply to the user. </summary>
    public UserModeCollection Modes {
      get { return _modes.Value; }
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Represents this User's information as an IRC mask. </summary>
    public override string ToString() {
      StringBuilder result = new StringBuilder();
      result.Append(Nick);
      if (!string.IsNullOrEmpty(this.Username)) {
        result.Append("!");
        result.Append(this.Username);
      }
      if (!string.IsNullOrEmpty(this.Host)) {
        result.Append("@");
        result.Append(this.Host);
      }

      return result.ToString();
    }

    /// <summary>
    ///   Determines wether the current user mask matches the given user mask. </summary>
    /// <param name="wildcardMask">
    ///   The wild-card filled mask to compare with the current. </param>
    /// <returns>
    ///   True if this mask is described by the given wildcard Mask. False if not.  </returns>
    public bool IsMatch(User wildcardMask) {
      if (wildcardMask == null) {
        return false;
      }

      // First we'll return quickly if there are exact matches
      if (this.Nick == wildcardMask.Nick && this.Username == wildcardMask.Username && this.Host == wildcardMask.Host) {
        return true;
      }

      return (true
        && Regex.IsMatch(this.Nick, makeRegexPattern(wildcardMask.Nick), RegexOptions.IgnoreCase)
        && Regex.IsMatch(this.Username, makeRegexPattern(wildcardMask.Username), RegexOptions.IgnoreCase)
        && Regex.IsMatch(this.Host, makeRegexPattern(wildcardMask.Host), RegexOptions.IgnoreCase)
        );
    }

    /// <summary>
    ///   Decides if the given user address matches the given address mask. </summary>
    /// <param name="actualMask">
    ///   The user address mask to compare match.  </param>
    /// <param name="wildcardMask">
    ///   The address mask containing wildcards to match with. </param>
    /// <returns>
    ///   True if <parmref>actualMask</parmref> is contained within (or described with) the <paramref>wildcardMask</paramref>.
    ///   False if not. </returns>
    public static bool IsMatch(string actualMask, string wildcardMask) {
      return new User(actualMask).IsMatch(new User(wildcardMask));
    }

    /// <summary>
    ///   Parses the given string as a mask to populate this user object. </summary>
    /// <param name="rawMask">
    ///   The mask to parse. </param>
    public void Parse(string rawMask) {
      this.Reset();

      string mask = rawMask;
      int indexOfBang = mask.IndexOf("!", StringComparison.Ordinal);
      int indexOfAt = mask.LastIndexOf("@", StringComparison.Ordinal);

      if (indexOfAt > 1) {
        this.Host = mask.Substring(indexOfAt + 1);
        mask = mask.Substring(0, indexOfAt);
      }

      if (indexOfBang != -1) {
        this.Username = mask.Substring(indexOfBang + 1);
        mask = mask.Substring(0, indexOfBang);
      }

      if (!string.IsNullOrEmpty(mask)) {
        string newNick = mask;
        string firstLetter = newNick.Substring(0, 1);
        if (ChannelStatus.Exists(firstLetter)) {
          newNick = newNick.Substring(1);
        }
        this.Nick = newNick;
      }
    }

    /// <summary>
    ///   Resets the User properties to the default values. </summary>
    public void Reset() {
      this.Nick = "";
      this.Username = "";
      this.Host = "";
      this.OnlineStatus = UserOnlineStatus.Online;
      this.AwayMessage = "";
      this.IrcOperator = false;
      this.Modes.Clear();
      this.Password = "";
      this.Name = "";
      this.Server = "";
      this.Username = "";
    }

    /// <summary>
    ///   Copies the properties of the given User onto this User. </summary>
    public void CopyFrom(User user) {
      this.OnlineStatus = user.OnlineStatus;
      this.AwayMessage = user.AwayMessage;
      this.Host = user.Host;
      this.Nick = user.Nick;
      this.Password = user.Password;
      this.Name = user.Name;
      this.Username = user.Username;
      this.Server = user.Server;
      this.IrcOperator = user.IrcOperator;
    }

    private static string makeRegexPattern(string wildcardString) {
      return Regex.Escape(wildcardString).Replace(@"\*", @".*").Replace(@"\?", @".");
    }

    #endregion

  } //class User
} //namespace Supay.Irc