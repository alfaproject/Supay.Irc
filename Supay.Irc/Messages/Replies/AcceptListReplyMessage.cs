using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   An Accept/CallerId system message received in response to an
    ///   <see cref="AcceptListRequestMessage" />.
    /// </summary>
    /// <remarks>
    ///   You may receive more than 1 of these in response to the request.
    /// </remarks>
    [Serializable]
    public class AcceptListReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="AcceptListReplyMessage" />.
        /// </summary>
        public AcceptListReplyMessage()
            : base(281)
        {
            this.Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets the collection of nicks of the users on the watch list.
        /// </summary>
        public IList<string> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            foreach (string nick in this.Nicks)
            {
                parameters.Add(nick);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Nicks.Clear();
            for (int i = 1; i < parameters.Count; i++)
            {
                this.Nicks.Add(parameters[i]);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
        ///   current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnAcceptListReply(new IrcMessageEventArgs<AcceptListReplyMessage>(this));
        }
    }
}
