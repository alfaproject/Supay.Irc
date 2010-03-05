using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Monitor system notification that a monitored user is online
  /// </summary>
  [Serializable]
  public class MonitoredUserOnlineMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="MonitoredUserOnlineMessage"/>.
    /// </summary>
    public MonitoredUserOnlineMessage()
      : base() {
      this.InternalNumeric = 730;
    }

    /// <summary>
    /// Gets the collection of users who are online.
    /// </summary>
    public UserCollection Users {
      get {
        if (users == null) {
          users = new UserCollection();
        }
        return users;
      }
    }
    private UserCollection users;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.AddParametersToFormat"/>. </summary>
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);

      string usersOnline = MessageUtil.CreateList<User>(this.Users, ",", user => user.IrcMask);
      writer.AddParameter(usersOnline);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      this.Users.Clear();

      if (parameters.Count > 1) {
        string userListParam = parameters[1];
        string[] userList = userListParam.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string userMask in userList) {
          User newUser = new User(userMask);
          this.Users.Add(newUser);
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnMonitoredUserOnline(new IrcMessageEventArgs<MonitoredUserOnlineMessage>(this));
    }

  }

}