using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   One of the responses to the <see cref="LusersMessage" /> query.
  /// </summary>
  [Serializable]
  public class LusersMeReplyMessage : NumericMessage {
    private const string iHave = "I have ";
    private const string clientsAnd = " clients and ";
    private const string servers = " servers";
    private int clientCount = -1;
    private int serverCount = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="LusersMeReplyMessage" /> class.
    /// </summary>
    public LusersMeReplyMessage()
      : base(255) {
    }

    /// <summary>
    ///   Gets or sets the number of clients connected to the server.
    /// </summary>
    public virtual int ClientCount {
      get {
        return clientCount;
      }
      set {
        clientCount = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of servers linked to the current server.
    /// </summary>
    public virtual int ServerCount {
      get {
        return serverCount;
      }
      set {
        serverCount = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(iHave + ClientCount + clientsAnd + ServerCount + servers);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      string payload = parameters[1];
      ClientCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, iHave, clientsAnd), CultureInfo.InvariantCulture);
      ServerCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, clientsAnd, servers), CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnLusersMeReply(new IrcMessageEventArgs<LusersMeReplyMessage>(this));
    }
  }
}
