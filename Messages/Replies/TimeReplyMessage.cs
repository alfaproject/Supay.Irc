using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This is the reply to the <see cref="TimeMessage" /> server query.
  /// </summary>
  [Serializable]
  public class ServerTimeReplyMessage : NumericMessage {
    private string server = string.Empty;
    private string time = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ServerTimeReplyMessage" /> class
    /// </summary>
    public ServerTimeReplyMessage()
      : base(391) {
    }

    /// <summary>
    ///   Gets or sets the server replying to the time request.
    /// </summary>
    public virtual string Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    /// <summary>
    ///   Gets or sets the time value requested.
    /// </summary>
    public virtual string Time {
      get {
        return time;
      }
      set {
        time = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Server);
      parameters.Add(Time);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        Server = parameters[1];
        Time = parameters[2];
      } else {
        Server = string.Empty;
        Time = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnServerTimeReply(new IrcMessageEventArgs<ServerTimeReplyMessage>(this));
    }
  }
}
