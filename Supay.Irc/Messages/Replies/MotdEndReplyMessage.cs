using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Signifies the end of the MOTD sent by the server.
    /// </summary>
    [Serializable]
    public class MotdEndReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="MotdEndReplyMessage" /> class.
        /// </summary>
        public MotdEndReplyMessage()
            : base(376)
        {
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add("End of /MOTD command");
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMotdEndReply(new IrcMessageEventArgs<MotdEndReplyMessage>(this));
        }
    }
}
