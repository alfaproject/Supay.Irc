using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The server reply to an <see cref="IsOnMessage" />.
    /// </summary>
    [Serializable]
    public class IsOnReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="IsOnReplyMessage" /> class.
        /// </summary>
        public IsOnReplyMessage()
            : base(303)
        {
            Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets the list of nicks of people who are known to be online.
        /// </summary>
        public ICollection<string> Nicks
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
            parameters.Add(string.Join(" ", this.Nicks));
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            
            this.Nicks.Clear();
            foreach (var nick in parameters[parameters.Count - 1].Split(' '))
            {
                Nicks.Add(nick);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnIsOnReply(new IrcMessageEventArgs<IsOnReplyMessage>(this));
        }
    }
}
