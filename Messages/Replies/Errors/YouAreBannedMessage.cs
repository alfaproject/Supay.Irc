using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Returned after an attempt to connect and register yourself with a server which has been set up to explicitly deny connections to you.
  /// </summary>
  [Serializable]
  public class YouAreBannedMessage : ErrorMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="YouAreBannedMessage" /> class.
    /// </summary>
    public YouAreBannedMessage()
      : base(465) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("You are banned from this server");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnYouAreBanned(new IrcMessageEventArgs<YouAreBannedMessage>(this));
    }
  }
}
