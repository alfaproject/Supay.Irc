using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message indicates the number of local-server users.
  /// </summary>
  [Serializable]
  public class LocalUsersReplyMessage : NumericMessage
  {
    private const string CURRENT_LOCAL_USERS = "Current local users: ";
    private const string MAX = " Max: ";
    private int userCount = -1;
    private int userLimit = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="LocalUsersReplyMessage" /> class.
    /// </summary>
    public LocalUsersReplyMessage()
      : base(265)
    {
    }

    /// <summary>
    ///   Gets or sets the number of local users.
    /// </summary>
    public virtual int UserCount
    {
      get
      {
        return this.userCount;
      }
      set
      {
        this.userCount = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of users for the server.
    /// </summary>
    public virtual int UserLimit
    {
      get
      {
        return this.userLimit;
      }
      set
      {
        this.userLimit = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(CURRENT_LOCAL_USERS + this.UserCount + MAX + this.UserLimit);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      switch (parameters.Count)
      {
        case 2:
          string payload = parameters[1];
          this.UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, CURRENT_LOCAL_USERS, MAX), CultureInfo.InvariantCulture);
          this.UserLimit = Convert.ToInt32(payload.Substring(payload.IndexOf(MAX, StringComparison.Ordinal) + MAX.Length), CultureInfo.InvariantCulture);
          break;
        case 4:
          this.UserCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
          this.UserLimit = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
          break;
        default:
          this.UserCount = -1;
          this.UserLimit = -1;
          break;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnLocalUsersReply(new IrcMessageEventArgs<LocalUsersReplyMessage>(this));
    }
  }
}
