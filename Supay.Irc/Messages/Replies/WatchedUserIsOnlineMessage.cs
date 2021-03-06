using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A Watch system notification that a user is online.
    /// </summary>
    [Serializable]
    public class WatchedUserIsOnlineMessage : WatchedUserOnlineMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WatchedUserIsOnlineMessage" />.
        /// </summary>
        public WatchedUserIsOnlineMessage()
            : base(604)
        {
        }

        protected override string ChangeMessage
        {
            get
            {
                return "is online";
            }
        }
    }
}
