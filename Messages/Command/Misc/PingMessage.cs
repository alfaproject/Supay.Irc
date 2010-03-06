using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The PingMessage is used to test the presence of an active client at the other end of the connection.
  /// </summary>
  /// <remarks>
  /// PingMessage is sent at regular intervals if no other activity detected coming from a connection. 
  /// If a connection fails to respond to a PingMessage within a set amount of time, that connection is closed.
  /// </remarks>
  [Serializable]
  public class PingMessage : CommandMessage {

    /// <summary>
    /// Gets or sets the target of the ping.
    /// </summary>
    public virtual string Target {
      get {
        return this.target;
      }
      set {
        this.target = value;
      }
    }
    private string target = string.Empty;

    /// <summary>
    /// Gets or sets the server that the ping should be forwarded to.
    /// </summary>
    public virtual string ForwardServer {
      get {
        return this.forwardServer;
      }
      set {
        this.forwardServer = value;
      }
    }
    private string forwardServer = string.Empty;

    /// <summary>
    /// Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "PING";
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Target);
      if (!string.IsNullOrEmpty(ForwardServer)) {
        parameters.Add(ForwardServer);
      }
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      ForwardServer = string.Empty;
      Target = string.Empty;
      if (parameters.Count >= 1) {
        Target = parameters[0];
        if (parameters.Count == 2) {
          ForwardServer = parameters[1];
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnPing(new IrcMessageEventArgs<PingMessage>(this));
    }

  }

}