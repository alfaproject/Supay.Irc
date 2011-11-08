using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   PongMessage is a reply to ping message.
  /// </summary>
  [Serializable]
  public class PongMessage : CommandMessage {
    private string forwardServer = string.Empty;
    private string target = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "PONG";
      }
    }

    /// <summary>
    ///   Gets or sets the target of the pong.
    /// </summary>
    public virtual string Target {
      get {
        return target;
      }
      set {
        target = value;
      }
    }

    /// <summary>
    ///   Gets or sets the server that the ping should be forwarded to.
    /// </summary>
    public virtual string ForwardServer {
      get {
        return forwardServer;
      }
      set {
        forwardServer = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Target);
      if (!string.IsNullOrEmpty(ForwardServer)) {
        parameters.Add(ForwardServer);
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 2) {
        Target = parameters[0];
        ForwardServer = parameters[1];
      } else {
        ForwardServer = string.Empty;
        Target = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnPong(new IrcMessageEventArgs<PongMessage>(this));
    }
  }
}
