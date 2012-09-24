using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The KillMessage is used to cause a client-server connection to be closed by the server which has the actual connection.
    /// </summary>
    /// <remarks>
    ///   KillMessage is used by servers when they encounter a duplicate entry in the list of valid nicknames and is used to remove both entries. 
    ///   It is also available to operators.
    /// </remarks>
    [Serializable]
    public class KillMessage : CommandMessage
    {
        public KillMessage()
        {
            Nick = string.Empty;
            Reason = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick of the user killed.
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the reason for the kill.
        /// </summary>
        public string Reason
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "KILL";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Nick);
            parameters.Add(string.IsNullOrEmpty(this.Reason) ? "kill" : this.Reason);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 2)
            {
                this.Nick = parameters[0];
                this.Reason = parameters[1];
            }
            else
            {
                this.Nick = string.Empty;
                this.Reason = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnKill(new IrcMessageEventArgs<KillMessage>(this));
        }
    }
}
