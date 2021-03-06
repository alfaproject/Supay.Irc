using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when a <see cref="TextMessage" /> is sent with an
    ///   empty Text property.
    /// </summary>
    [Serializable]
    public class NoTextToSendMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="NoTextToSendMessage" /> class.
        /// </summary>
        public NoTextToSendMessage()
            : base(412)
        {
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add("No text to send");
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnNoTextToSend(new IrcMessageEventArgs<NoTextToSendMessage>(this));
        }
    }
}
