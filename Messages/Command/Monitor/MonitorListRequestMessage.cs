using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Monitor system message that requests the list of nicks currently being monitored.
  /// </summary>
  [Serializable]
  public class MonitorListRequestMessage : MonitorMessage {

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      Collection<string> param = MessageUtil.GetParameters(unparsedMessage);
      return (param.Count == 1 && param[0].EqualsI("L"));
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("L");
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnMonitorListRequest(new IrcMessageEventArgs<MonitorListRequestMessage>(this));
    }

  }

}