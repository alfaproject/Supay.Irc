using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is received from the server when it acknowledges a client's
    ///   <see cref="AwayMessage" />.
    /// </summary>
    [Serializable]
    public class SelfAwayMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="SelfAwayMessage" /> class.
        /// </summary>
        public SelfAwayMessage()
            : base(306)
        {
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add("You have been marked as being away");
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSelfAway(new IrcMessageEventArgs<SelfAwayMessage>(this));
        }
    }
}
