using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Used to indicate the nickname parameter supplied to a command is currently unused.
    /// </summary>
    [Serializable]
    public class NoSuchNickMessage : ErrorMessage
    {
        private string nick = string.Empty;

        /// <summary>
        ///   Creates a new instances of the <see cref="NoSuchNickMessage" /> class.
        /// </summary>
        public NoSuchNickMessage()
            : base(401)
        {
        }

        /// <summary>
        ///   Gets or sets the nick which wasn't accepted.
        /// </summary>
        public virtual string Nick
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
            parameters.Add("No such nick/channel");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
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
            conduit.OnNoSuchNick(new IrcMessageEventArgs<NoSuchNickMessage>(this));
        }
    }
}
