using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message indicates the number of network-wide users.
  /// </summary>
  [Serializable]
  public class GlobalUsersReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="GlobalUsersReplyMessage"/> class.
    /// </summary>
    public GlobalUsersReplyMessage()
      : base(266) {
    }

    /// <summary>
    /// Gets or sets the number of global users.
    /// </summary>
    public virtual int UserCount {
      get {
        return userCount;
      }
      set {
        userCount = value;
      }
    }
    private int userCount = -1;

    /// <summary>
    /// Gets or sets the maximum number of users for the network.
    /// </summary>
    public virtual int UserLimit {
      get {
        return userLimit;
      }
      set {
        userLimit = value;
      }
    }
    private int userLimit = -1;

    private const string currentGlobalUsers = "Current global users: ";
    private const string max = " Max: ";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      // we only write the official version of this message, although other versions exist,
      // thus the message may not be the same raw as parsed.
      Collection<string> parameters = base.GetParameters();
      parameters.Add(currentGlobalUsers + UserCount + max + UserLimit);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      switch (parameters.Count) {
        case 2:
          string payload = parameters[1];
          UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, currentGlobalUsers, max), CultureInfo.InvariantCulture);
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
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnGlobalUsersReply(new IrcMessageEventArgs<GlobalUsersReplyMessage>(this));
    }

  }

}