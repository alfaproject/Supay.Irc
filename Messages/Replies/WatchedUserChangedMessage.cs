using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Watch system notification that a watched user's status has changed
  /// </summary>
  [Serializable]
  public abstract class WatchedUserChangedMessage : NumericMessage {
    private DateTime changeTime;
    private User watchedUser;

    protected WatchedUserChangedMessage(int number)
      : base(number) {
    }

    /// <summary>
    ///   Gets or sets the watched User who's status has changed.
    /// </summary>
    public User WatchedUser {
      get {
        return watchedUser ?? (watchedUser = new User());
      }
      set {
        watchedUser = value;
      }
    }

    /// <summary>
    ///   Gets or sets the time at which the change occurred.
    /// </summary>
    public DateTime TimeOfChange {
      get {
        return changeTime;
      }
      set {
        changeTime = value;
      }
    }

    /// <exclude />
    protected abstract string ChangeMessage {
      get;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(WatchedUser.Nickname);
      parameters.Add(WatchedUser.Username);
      parameters.Add(WatchedUser.Host);
      parameters.Add(MessageUtil.ConvertToUnixTime(TimeOfChange).ToString(CultureInfo.InvariantCulture));
      parameters.Add(ChangeMessage);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);

      WatchedUser = new User();
      TimeOfChange = DateTime.MinValue;

      if (parameters.Count == 6) {
        WatchedUser.Nickname = parameters[1];
        WatchedUser.Username = parameters[2];
        WatchedUser.Host = parameters[3];
        TimeOfChange = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[4], CultureInfo.InvariantCulture));
      }
    }
  }
}
