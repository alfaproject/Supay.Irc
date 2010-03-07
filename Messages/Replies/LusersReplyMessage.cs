using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// One of the responses to the <see cref="LusersMessage"/> query.
  /// </summary>
  [Serializable]
  public class LusersReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="LusersReplyMessage"/> class.
    /// </summary>
    public LusersReplyMessage()
      : base(251) {
    }


    /// <summary>
    /// Gets or sets the number of users connected to IRC.
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
    /// Gets or sets the number of invisible users connected to IRC.
    /// </summary>
    public virtual int InvisibleCount {
      get {
        return invisibleCount;
      }
      set {
        invisibleCount = value;
      }
    }

    /// <summary>
    /// Gets or sets the number of servers connected on the network.
    /// </summary>
    public virtual int ServerCount {
      get {
        return serverCount;
      }
      set {
        serverCount = value;
      }
    }

    private int userCount = -1;
    private int invisibleCount = -1;
    private int serverCount = -1;
    private const string thereAre = "There are ";
    private const string usersAnd = " users and ";
    private const string invisibleOn = " invisible on ";
    private const string servers = " servers";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(thereAre + UserCount + usersAnd + InvisibleCount + invisibleOn + ServerCount + servers);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      string payload = parameters[1];
      this.UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, thereAre, usersAnd), CultureInfo.InvariantCulture);
      this.InvisibleCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, usersAnd, invisibleOn), CultureInfo.InvariantCulture);
      this.ServerCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, invisibleOn, servers), CultureInfo.InvariantCulture);

    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnLusersReply(new IrcMessageEventArgs<LusersReplyMessage>(this));
    }

  }

}