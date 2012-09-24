using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply to a <see cref="WhoIsMessage" /> when the user is an IRC operator.
    /// </summary>
    [Serializable]
    public class WhoIsOperReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsOperReplyMessage" /> class.
        /// </summary>
        public WhoIsOperReplyMessage()
            : base(313)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the Nick of the user being examined.
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
            parameters.Add("is an IRC operator");
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
            conduit.OnWhoIsOperReply(new IrcMessageEventArgs<WhoIsOperReplyMessage>(this));
        }
    }
}
