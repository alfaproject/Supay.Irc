using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    /// 730 - RPL_MONONLINE
    /// -------------------
    /// :&lt;server&gt; 730 &lt;nick&gt; :nick!user@host[,nick!user@host]*
    ///
    /// This numeric is used to indicate to a client that either a nickname has just
    /// become online, or that a nickname they have added to their monitor list is online.
    ///
    /// The server may send "*" instead of the target nick (&lt;nick&gt;).
    /// (This makes it possible to send the exact same message to all clients monitoring a certain nick.)
    ///
    /// http://git.atheme.org/charybdis/tree/doc/monitor.txt
    /// </summary>
    [Serializable]
    public class MonitoredUserOnlineMessage : NumericMessage
    {
        private UserCollection users;

        /// <summary>
        ///   Creates a new instance of the <see cref="MonitoredUserOnlineMessage" />.
        /// </summary>
        public MonitoredUserOnlineMessage()
            : base(730)
        {
        }

        /// <summary>
        ///   Gets the collection of users who are online.
        /// </summary>
        public UserCollection Users
        {
            get
            {
                return this.users ?? (this.users = new UserCollection());
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
                parameters.Add(string.Join(",", this.Users.Values));
                return parameters;
            }
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Users.Clear();

            if (parameters.Count > 1)
            {
                var userList = parameters[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var newUser in userList.Select(userMask => new User(userMask)))
                {
                    this.Users.Add(newUser.Nickname, newUser);
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMonitoredUserOnline(new IrcMessageEventArgs<MonitoredUserOnlineMessage>(this));
        }
    }
}
