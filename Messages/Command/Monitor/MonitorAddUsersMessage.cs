using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// A Monitor system message that adds users to your monitor list.
  /// </summary>
  [Serializable]
  public class MonitorAddUsersMessage : MonitorMessage {
    /// <summary>
    /// Gets the collection of nicks being added to the monitor list.
    /// </summary>
    public Collection<string> Nicks {
      get {
        if (nicks == null) {
          nicks = new Collection<string>();
        }
        return nicks;
      }
    }

    private Collection<string> nicks;

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      string firstParam = MessageUtil.GetParameter(unparsedMessage, 0);
      return firstParam.StartsWith("+", StringComparison.Ordinal);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      string nicksParam = parameters[parameters.Count - 1];
      string[] splitNicksParam = nicksParam.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
      foreach (string nick in splitNicksParam) {
        this.Nicks.Add(nick);
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("+");
      if (Nicks != null && Nicks.Count != 0) {
        parameters.Add(MessageUtil.CreateList(Nicks, ","));
      }
      return parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnMonitorAddUsers(new IrcMessageEventArgs<MonitorAddUsersMessage>(this));
    }
  }
}
