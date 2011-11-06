using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Monitor system notification that a monitored user is online
  /// </summary>
  [Serializable]
  public class MonitoredUserOnlineMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="MonitoredUserOnlineMessage" />.
    /// </summary>
    public MonitoredUserOnlineMessage()
      : base(730) {
    }

    /// <summary>
    ///   Gets the collection of users who are online.
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
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Users, ",", user => user.IrcMask));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Users.Clear();

      if (parameters.Count > 1) {
        string userListParam = parameters[1];
        string[] userList = userListParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string userMask in userList) {
          User newUser = new User(userMask);
          Users.Add(newUser);
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnMonitoredUserOnline(new IrcMessageEventArgs<MonitoredUserOnlineMessage>(this));
    }
  }
}
