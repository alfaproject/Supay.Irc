using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A client session is ended with a QuitMessage.
  /// </summary>
  /// <remarks>
  ///   The server must close the connection to a client which sends a QuitMessage. If a
  ///   <see cref="QuitMessage.Reason" /> is given, this will be sent instead of the default
  ///   message, the nickname.
  /// </remarks>
  [Serializable]
  public class QuitMessage : CommandMessage {
    /// <summary>
    ///   Creates a new instance of the QuitMessage class.
    /// </summary>
    public QuitMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the QuitMessage class with the given reason.
    /// </summary>
    public QuitMessage(string reason)
      : this() {
      Reason = reason;
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "QUIT";
      }
    }

    /// <summary>
    ///   Gets or sets the reason for quitting.
    /// </summary>
    public string Reason {
      get;
      set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      if (!string.IsNullOrEmpty(Reason)) {
        parameters.Add(Reason);
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 1) {
        Reason = parameters[0];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnQuit(new IrcMessageEventArgs<QuitMessage>(this));
    }
  }
}
