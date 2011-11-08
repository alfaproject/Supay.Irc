using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This message indicates the number of local-server users.
  /// </summary>
  [Serializable]
  public class LocalUsersReplyMessage : NumericMessage {
    private const string currentLocalUsers = "Current local users: ";
    private const string max = " Max: ";
    private int userCount = -1;
    private int userLimit = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="LocalUsersReplyMessage" /> class.
    /// </summary>
    public LocalUsersReplyMessage()
      : base(265) {
    }

    /// <summary>
    ///   Gets or sets the number of local users.
    /// </summary>
    public virtual int UserCount {
      get {
        return userCount;
      }
      set {
        userCount = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of users for the server.
    /// </summary>
    public virtual int UserLimit {
      get {
        return userLimit;
      }
      set {
        userLimit = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(currentLocalUsers + UserCount + max + UserLimit);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      switch (parameters.Count) {
        case 2:
          string payload = parameters[1];
          UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, currentLocalUsers, max), CultureInfo.InvariantCulture);
          UserLimit = Convert.ToInt32(payload.Substring(payload.IndexOf(max, StringComparison.Ordinal) + max.Length), CultureInfo.InvariantCulture);
          break;
        case 4:
          UserCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
          UserLimit = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
          break;
        default:
          UserCount = -1;
          UserLimit = -1;
          break;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnLocalUsersReply(new IrcMessageEventArgs<LocalUsersReplyMessage>(this));
    }
  }
}
