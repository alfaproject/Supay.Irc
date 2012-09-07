using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   With the AwayMessage, clients can set an automatic reply string for any <see cref="ChatMessage" />s directed at them (not to a channel they are on).
    /// </summary>
    /// <remarks>
    ///   The automatic reply is sent by the server to client sending the <see cref="ChatMessage" />.
    ///   The only replying server is the one to which the sending client is connected to.
    /// </remarks>
    [Serializable]
    public class AwayMessage : CommandMessage
    {
        private string reason = string.Empty;

        /// <summary>
        ///   Creates a new instance of the AwayMessage class.
        /// </summary>
        public AwayMessage()
        {
        }

        /// <summary>
        ///   Creates a new instance of the AwayMessage class with the given reason.
        /// </summary>
        public AwayMessage(string reason)
        {
            this.reason = reason;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "AWAY";
            }
        }

        /// <summary>
        ///   Gets or sets the reason for being away.
        /// </summary>
        public virtual string Reason
        {
            get
            {
                return this.reason;
            }
            set
            {
                this.reason = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(string.IsNullOrEmpty(this.Reason) ? "away" : this.Reason);
            return parameters;
        }

        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            return base.CanParse(unparsedMessage) && MessageUtil.GetParameters(unparsedMessage).Count > 0;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Reason = parameters.Count > 0 ? parameters[0] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnAway(new IrcMessageEventArgs<AwayMessage>(this));
        }
    }
}
