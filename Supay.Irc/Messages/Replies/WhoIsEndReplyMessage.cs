using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Signals the end of a <see cref="WhoIsMessage" /> reply.
    /// </summary>
    [Serializable]
    public class WhoIsEndReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsEndReplyMessage" /> class.
        /// </summary>
        public WhoIsEndReplyMessage()
            : base(318)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick for the user examined.
        /// </summary>
        public string Nick
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
            parameters.Add(this.Nick);
            parameters.Add("End of /WHOIS list");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count == 3)
            {
                this.Nick = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoIsEndReply(new IrcMessageEventArgs<WhoIsEndReplyMessage>(this));
        }
    }
}
