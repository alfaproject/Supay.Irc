using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Monitor system message signalling the end of a monitor list request.
  /// </summary>
  [Serializable]
  public class MonitorListEndReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="MonitorListReplyMessage"/>.
    /// </summary>
    public MonitorListEndReplyMessage()
      : base() {
      this.InternalNumeric = 733;
    }

    /// <summary>
    ///   Overrides <see href="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("End of MONITOR list");
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnMonitorListEndReply(new IrcMessageEventArgs<MonitorListEndReplyMessage>(this));
    }

  }

}