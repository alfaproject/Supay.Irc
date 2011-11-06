using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Returned when a nickname parameter expected for a command and isn't found.
  /// </summary>
  [Serializable]
  public class NoNickGivenMessage : ErrorMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="NoNickGivenMessage" /> class.
    /// </summary>
    public NoNickGivenMessage()
      : base(431) {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("No nickname given");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNoNickGiven(new IrcMessageEventArgs<NoNickGivenMessage>(this));
    }
  }
}
