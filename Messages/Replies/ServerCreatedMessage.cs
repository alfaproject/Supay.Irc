using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This message is sent from the server after connection,
  ///   and contains information about the creation of the server.
  /// </summary>
  [Serializable]
  public class ServerCreatedMessage : NumericMessage {
    private const string thisServerCreated = "This server was created ";
    private string createdDate = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ServerCreatedMessage" /> class.
    /// </summary>
    public ServerCreatedMessage()
      : base(003) {
    }

    /// <summary>
    ///   Gets or sets the date on which the server was created.
    /// </summary>
    public virtual string CreatedDate {
      get {
        return createdDate;
      }
      set {
        createdDate = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(thisServerCreated + CreatedDate);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      string reply = parameters[1];
      if (reply.IndexOf(thisServerCreated, StringComparison.Ordinal) != -1) {
        int startOfDate = thisServerCreated.Length;
        CreatedDate = reply.Substring(startOfDate);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnServerCreated(new IrcMessageEventArgs<ServerCreatedMessage>(this));
    }
  }
}
