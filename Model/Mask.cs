﻿using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Supay.Irc {

  /// <summary>
  ///   Represents a Mask on an IRC server. (nickname!username@host) </summary>
  [Serializable]
  public class Mask : IEquatable<Mask>, INotifyPropertyChanged {

    /// <summary>
    ///   Raised when a property on the instance has changed. </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    private string _nickname;
    private string _username;
    private string _host;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="Mask" /> class with the given mask values. </summary>
    /// <param name="nickname">
    ///   The nickname of this mask. </param>
    /// <param name="username">
    ///   The hostname of this mask. </param>
    /// <param name="host">
    ///   The host of this mask. </param>
    public Mask(string nickname, string username, string host) {
      _nickname = nickname;
      _username = username;
      _host = host;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="Mask" /> class with the given mask string. </summary>
    /// <param name="mask">
    ///   The mask string to parse. </param>
    public Mask(string mask) {
      int indexOfAt = mask.LastIndexOf('@');
      if (indexOfAt != -1) {
        _host = mask.Substring(indexOfAt + 1);
        mask = mask.Substring(0, indexOfAt);
      }

      int indexOfBang = mask.IndexOf('!');
      if (indexOfBang != -1) {
        _username = mask.Substring(indexOfBang + 1);
        mask = mask.Substring(0, indexOfBang);
      }

      if (!string.IsNullOrEmpty(mask)) {
        _nickname = mask;
      }
    }

    /// <summary>
    ///   Initializes a new empty instance of the <see cref="Mask" /> class. </summary>
    public Mask() {
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the nickname of this mask. </summary>
    public string Nickname {
      get {
        return _nickname;
      }
      set {
        if (_nickname != value) {
          _nickname = value;
          this.RaisePropertyChanged("Nickname");
          this.RaisePropertyChanged("IrcMask");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the username of this mask. </summary>
    public string Username {
      get {
        return _username;
      }
      set {
        if (_username != value) {
          _username = value;
          this.RaisePropertyChanged("Username");
          this.RaisePropertyChanged("IrcMask");
        }
      }
    }

    /// <summary>
    ///   Gets or sets the host of this mask. </summary>
    public string Host {
      get {
        return _host;
      }
      set {
        if (_host != value) {
          _host = value;
          this.RaisePropertyChanged("Host");
          this.RaisePropertyChanged("IrcMask");
        }
      }
    }

    /// <summary>
    ///   Gets this Mask information with a guarenteed nickname!username@host format. </summary>
    public string IrcMask {
      get {
        return (string.IsNullOrEmpty(_nickname) ? "*" : _nickname) +
          "!" + (string.IsNullOrEmpty(_username) ? "*" : _username) +
          "@" + (string.IsNullOrEmpty(_host) ? "*" : _host);
      }
    }

    /// <summary>
    ///   Indicates wether this mask has wildcards. </summary>
    public bool HasWildcards {
      get {
        return (this.IrcMask.IndexOfAny(new char[] { '?', '*' }) == -1);
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Represents this mask using IRC format. </summary>
    public override string ToString() {
      return this.IrcMask;
    }

    /// <summary>
    ///   Determines whether this instance and another specified <see cref="Mask"/> object have the same value. </summary>
    /// <param name="other">
    ///   A <see cref="Mask"/>. </param>
    /// <returns>
    ///   true if the value of the <paramref name="other"/> parameter is the same as this instance;
    ///   otherwise, false. </returns>
    public bool Equals(Mask other) {
      if (object.ReferenceEquals(null, other)) {
        return false;
      }
      return this.IrcMask.Equals(other.IrcMask, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Determines whether this instance of <see cref="Mask"/> and a specified object,
    ///   which must also be a <see cref="Mask"/> object, have the same value. </summary>
    /// <param name="obj">
    ///   An <see cref="Object"/>. </param>
    /// <returns>
    ///   true if <paramref name="obj"/> is a <see cref="Mask"/> and its value is the same as this instance;
    ///   otherwise, false. </returns>
    public override bool Equals(object obj) {
      return this.Equals(obj as Mask);
    }

    /// <summary>
    ///   Serves as a hash function for this particular type. </summary>
    /// <returns>
    ///   A hash code for the current <see cref="Mask"/>. </returns>
    public override int GetHashCode() {
      return this.IrcMask.GetHashCode();
    }

    /// <summary>
    ///   Indicates whether pattern mask matches this instance mask. </summary>
    /// <param name="pattern">
    ///   The <see cref="Mask"/> pattern to match. </param>
    /// <returns>
    ///   true if there is a match; otherwise, false. </returns>
    public bool IsMatch(Mask pattern) {
      return Mask.IsMatch(this, pattern);
    }

    /// <summary>
    ///   Indicates whether pattern mask matches input mask. </summary>
    /// <param name="input">
    ///   The <see cref="Mask"/> to check for a match. </param>
    /// <param name="pattern">
    ///   The <see cref="Mask"/> pattern to match. </param>
    /// <returns>
    ///   true if there is a match; otherwise, false. </returns>
    public static bool IsMatch(Mask input, Mask pattern) {
      string regexPattern = Regex.Escape(pattern.IrcMask).Replace(@"\?", ".").Replace(@"\*", ".*");
      return Regex.IsMatch(input.IrcMask, regexPattern, RegexOptions.IgnoreCase);
    }

    #endregion

    #region Protected Methods

    protected void RaisePropertyChanged(string propertyName) {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  } //class Mask
} //namespace Supay.Irc