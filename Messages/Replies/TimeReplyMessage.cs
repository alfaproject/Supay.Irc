using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This is the reply to the <see cref="TimeMessage"/> server query.
  /// </summary>
  [Serializable]
  public class ServerTimeReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ServerTimeReplyMessage"/> class
    /// </summary>
    public ServerTimeReplyMessage()
      : base() {
      this.InternalNumeric = 391;
    }

    /// <summary>
    /// Gets or sets the server replying to the time request.
    /// </summary>
    public virtual String Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    /// <summary>
    /// Gets or sets the time value requested.
    /// </summary>
    public virtual String Time {
      get {
        return time;
      }
      set {
        time = value;
      }
    }

    private String server = "";
    private String time = "";


    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Server);
      writer.AddParameter(this.Time);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 3) {
        this.Server = parameters[1];
        this.Time = parameters[2];
      } else {
        this.Server = "";
        this.Time = "";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnServerTimeReply(new IrcMessageEventArgs<ServerTimeReplyMessage>(this));
    }

  }

}