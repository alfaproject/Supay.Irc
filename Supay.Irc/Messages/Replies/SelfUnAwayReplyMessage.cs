using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is received from the server when it acknowledges a client's
    ///   <see cref="BackMessage" />.
    /// </summary>
    [Serializable]
    public class SelfUnAwayMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="SelfUnAwayMessage" /> class.
        /// </summary>
        public SelfUnAwayMessage()
            : base(305)
        {
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
                parameters.Add("You are no longer marked as being away");
                return parameters;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSelfUnAway(new IrcMessageEventArgs<SelfUnAwayMessage>(this));
        }
    }
}
