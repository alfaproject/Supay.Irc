using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A reply to a <see cref="WhoIsMessage"/> that specifies what server they are on.
  /// </summary>
  [Serializable]
  public class WhoIsServerReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="WhoIsServerReplyMessage"/> class.
    /// </summary>
    public WhoIsServerReplyMessage()
      : base() {
      this.InternalNumeric = 312;
    }

    /// <summary>
    /// Gets or sets the nick of the user being examined.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the server the user is connected to.
    /// </summary>
    public virtual string ServerName {
      get {
        return serverName;
      }
      set {
        serverName = value;
      }
    }

    /// <summary>
    /// Gets or sets additional information about the user's server connection.
    /// </summary>
    public virtual string Info {
      get {
        return info;
      }
      set {
        info = value;
      }
    }

    private string nick = string.Empty;
    private string serverName = string.Empty;
    private string info = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add(ServerName);
      parameters.Add(Info);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 3) {
        this.Nick = parameters[1];
        this.ServerName = parameters[2];
        this.Info = parameters[3];
      } else {
        this.Nick = string.Empty;
        this.ServerName = string.Empty;
        this.Info = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWhoIsServerReply(new IrcMessageEventArgs<WhoIsServerReplyMessage>(this));
    }

  }

}