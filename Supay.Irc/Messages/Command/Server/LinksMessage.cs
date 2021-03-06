using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="LinksMessage" /> asks the server to send a list all servers which are known by the server answering the message.
    /// </summary>
    [Serializable]
    public class LinksMessage : ServerQueryBase
    {
        public LinksMessage()
        {
            Mask = string.Empty;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "LINKS";
            }
        }

        /// <summary>
        ///   Gets or sets the mask for server info to limit the list or replies.
        /// </summary>
        public string Mask
        {
            get;
            set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            if (!string.IsNullOrEmpty(this.Mask))
            {
                parameters.Add(this.Target);
                parameters.Add(this.Mask);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Mask = parameters.Count >= 2 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLinks(new IrcMessageEventArgs<LinksMessage>(this));
        }
    }
}
