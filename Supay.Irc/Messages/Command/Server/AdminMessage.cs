using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="AdminMessage" /> is used to find the name of the administrator of the given server, or current server if <see cref="ServerQueryBase.Target" /> is empty.
    /// </summary>
    [Serializable]
    public class AdminMessage : ServerQueryBase
    {
        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "ADMIN";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Target);
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnAdmin(new IrcMessageEventArgs<AdminMessage>(this));
        }
    }
}
