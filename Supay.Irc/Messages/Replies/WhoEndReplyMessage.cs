using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Signals the end of the <see cref="WhoReplyMessage" /> list.
    /// </summary>
    [Serializable]
    public class WhoEndReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoEndReplyMessage" /> class.
        /// </summary>
        public WhoEndReplyMessage()
            : base(315)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick of the user in the Who reply list.
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
            parameters.Add("End of /WHO list");
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
            conduit.OnWhoEndReply(new IrcMessageEventArgs<WhoEndReplyMessage>(this));
        }
    }
}
