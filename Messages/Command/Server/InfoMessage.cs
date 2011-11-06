using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="InfoMessage" /> requests information which describes the server;
  ///   its version, when it was compiled, the patch-level, when it was started, 
  ///   and any other miscellaneous information which may be considered to be relevant.
  /// </summary>
  [Serializable]
  public class InfoMessage : ServerQueryBase {
    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "INFO";
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Target);
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnInfo(new IrcMessageEventArgs<InfoMessage>(this));
    }
  }
}
