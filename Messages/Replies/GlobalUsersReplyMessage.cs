using System;
using System.Collections.Specialized;
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
      : base() {
      this.InternalNumeric = 266;
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

    private string currentGlobalUsers = "Current global users: ";
    private string max = " Max: ";

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      // we only write the official version of this message, although other versions exist,
      // thus the message may not be the same raw as parsed.
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.currentGlobalUsers + this.userCount + this.max + this.UserLimit);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 2) {
        string payload = parameters[1];
        this.UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, this.currentGlobalUsers, this.max), CultureInfo.InvariantCulture);
        this.UserLimit = Convert.ToInt32(payload.Substring(payload.IndexOf(this.max, StringComparison.Ordinal) + this.max.Length), CultureInfo.InvariantCulture);
      } else if (parameters.Count == 4) {
        this.UserCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
        this.UserLimit = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
      } else {
        this.UserCount = -1;
        this.UserLimit = -1;
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