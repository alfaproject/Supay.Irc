using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage sent when a TextMessage is sent with an empty Text property.
  /// </summary>
  [Serializable]
  public class NoTextToSendMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NoTextToSendMessage"/> class.
    /// </summary>
    public NoTextToSendMessage()
      : base() {
      this.InternalNumeric = 412;
    }

    /// <summary>
    ///   Overrides <see href="IrcMessage.GetParameters" />. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("No text to send");
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNoTextToSend(new IrcMessageEventArgs<NoTextToSendMessage>(this));
    }

  }

}