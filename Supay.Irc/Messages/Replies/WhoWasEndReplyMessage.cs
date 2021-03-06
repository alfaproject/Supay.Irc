using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Signals the end of a reply to a <see cref="WhoWasMessage" />.
    /// </summary>
    [Serializable]
    public class WhoWasEndReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoWasEndReplyMessage" /> class.
        /// </summary>
        public WhoWasEndReplyMessage()
            : base(369)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick of the user being examined.
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
            parameters.Add("End of WHOWAS");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nick = parameters.Count == 3 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoWasEndReply(new IrcMessageEventArgs<WhoWasEndReplyMessage>(this));
        }
    }
}
