using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Watch system notification that a user is offline.
  /// </summary>
  [Serializable]
  public class WatchedUserIsOfflineMessage : WatchedUserOfflineMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="WatchedUserIsOfflineMessage"/>.
    /// </summary>
    public WatchedUserIsOfflineMessage()
      : base(605) {
    }

    /// <exclude />
    protected override string ChangeMessage {
      get {
        return "is offline";
      }
    }

  }

}