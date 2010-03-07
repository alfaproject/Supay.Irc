using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Message that clears the list of users on your watch list.
  /// </summary>
  [Serializable]
  public class WatchListClearMessage : WatchMessage {

    #region Parsing

    /// <summary>
    ///   Determines if the message can be parsed by this type. </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      StringCollection param = MessageUtil.GetParameters(unparsedMessage);
      return (param.Count == 1 && param[0].EqualsI("C"));
    }

    #endregion

    #region Formatting

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("C");
      return parameters;
    }

    #endregion

    #region Events

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWatchListClear(new IrcMessageEventArgs<WatchListClearMessage>(this));
    }

    #endregion

  }

}