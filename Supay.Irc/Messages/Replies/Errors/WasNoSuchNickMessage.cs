using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Returned from the server in response to a <see cref="WhoWasMessage" /> to indicate that
    ///   there is no history information for that nick.
    /// </summary>
    [Serializable]
    public class WasNoSuchNickMessage : ErrorMessage
    {
        private string nick;

        /// <summary>
        ///   Creates a new instances of the <see cref="WasNoSuchNickMessage" /> class.
        /// </summary>
        public WasNoSuchNickMessage()
            : base(406)
        {
        }

        /// <summary>
        ///   The nick which had no information
        /// </summary>
        public string Nick
        {
            get
            {
                return this.nick;
            }
            set
            {
                this.nick = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Nick);
            parameters.Add("There was no such nickname");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameter portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nick = parameters.Count > 1 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWasNoSuchNick(new IrcMessageEventArgs<WasNoSuchNickMessage>(this));
        }
    }
}
