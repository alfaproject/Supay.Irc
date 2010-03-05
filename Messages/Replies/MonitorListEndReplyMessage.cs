using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Monitor system message signaling the end of a monitor list request.
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
    /// Overrides <see href="IrcMessage.AddParametersToFormat" />.
    /// </summary>
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter("End of MONITOR list");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnMonitorListEndReply(new IrcMessageEventArgs<MonitorListEndReplyMessage>(this));
    }

  }

}