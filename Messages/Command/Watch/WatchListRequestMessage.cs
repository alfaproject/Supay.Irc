using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Watch system message that requests the list of nicks currently being watched.
  /// </summary>
  [Serializable]
  public class WatchListRequestMessage : WatchMessage {
    #region Properties

    private bool onlineOnly;

    /// <summary>
    ///   Gets or sets if the message requests that only online contacts are in the list.
    /// </summary>
    public bool OnlineOnly {
      get {
        return onlineOnly;
      }
      set {
        onlineOnly = value;
      }
    }

    #endregion

    #region Parsing

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      Collection<string> param = MessageUtil.GetParameters(unparsedMessage);
      return (param.Count == 0 || (param.Count == 1 && param[0].EqualsI("L")));
    }

    /// <summary>
    ///   Overrides <see href = "IrcMessage.ParseParameters" />
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      OnlineOnly = parameters.Count == 0 || parameters[0] == "l";
    }

    #endregion

    #region Formatting

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(OnlineOnly ? "l" : "L");
      return parameters;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWatchListRequest(new IrcMessageEventArgs<WatchListRequestMessage>(this));
    }

    #endregion
  }
}
