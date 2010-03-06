using System;
using System.Collections.ObjectModel;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   The <see cref="ErrorMessage"/> received when a <see cref="UserModeMessage"/> was sent with
  ///   a <see cref="UserMode"/> which the server didn't recognize. </summary>
  [Serializable]
  public class UnknownUserModeMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="UnknownUserModeMessage"/> class.
    /// </summary>
    public UnknownUserModeMessage()
      : base(501) {
    }

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("Unknown MODE flag");
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUnknownUserMode(new IrcMessageEventArgs<UnknownUserModeMessage>(this));
    }

  }

}