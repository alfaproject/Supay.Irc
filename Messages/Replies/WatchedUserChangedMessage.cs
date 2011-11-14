using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Watch system notification that a watched user's status has changed
  /// </summary>
  [Serializable]
  public abstract class WatchedUserChangedMessage : NumericMessage
  {
    private DateTime changeTime;
    private User watchedUser;

    protected WatchedUserChangedMessage(int number)
      : base(number)
    {
    }

    /// <summary>
    ///   Gets or sets the watched User who's status has changed.
    /// </summary>
    public User WatchedUser
    {
      get
      {
        return this.watchedUser ?? (this.watchedUser = new User());
      }
      set
      {
        this.watchedUser = value;
      }
    }

    /// <summary>
    ///   Gets or sets the time at which the change occurred.
    /// </summary>
    public DateTime TimeOfChange
    {
      get
      {
        return this.changeTime;
      }
      set
      {
        this.changeTime = value;
      }
    }

    /// <exclude />
    protected abstract string ChangeMessage
    {
      get;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.WatchedUser.Nickname);
      parameters.Add(this.WatchedUser.Username);
      parameters.Add(this.WatchedUser.Host);
      parameters.Add(MessageUtil.ConvertToUnixTime(this.TimeOfChange).ToString(CultureInfo.InvariantCulture));
      parameters.Add(this.ChangeMessage);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.WatchedUser = new User();
      this.TimeOfChange = DateTime.MinValue;

      if (parameters.Count == 6)
      {
        this.WatchedUser.Nickname = parameters[1];
        this.WatchedUser.Username = parameters[2];
        this.WatchedUser.Host = parameters[3];
        this.TimeOfChange = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[4], CultureInfo.InvariantCulture));
      }
    }
  }
}
