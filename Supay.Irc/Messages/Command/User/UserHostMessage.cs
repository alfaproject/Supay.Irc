using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Requests information about the nicks supplied in the Nick property.
    /// </summary>
    [Serializable]
    public class UserHostMessage : CommandMessage
    {
        public UserHostMessage()
        {
            Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "USERHOST";
            }
        }

        /// <summary>
        ///   Gets the collection of nicks to request information for.
        /// </summary>
        public ICollection<string> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(string.Join(" ", this.Nicks));
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nicks.Clear();
            foreach (string nick in parameters)
            {
                this.Nicks.Add(nick);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUserHost(new IrcMessageEventArgs<UserHostMessage>(this));
        }
    }
}
