using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Returned when a <see cref="NickMessage" /> is processed that results in an attempt to change to a currently existing nickname.
    /// </summary>
    [Serializable]
    public class NickInUseMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="NickInUseMessage" /> class.
        /// </summary>
        public NickInUseMessage()
            : base(433)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick which was already taken.
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
            parameters.Add("Nickname is already in use.");
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
            conduit.OnNickInUse(new IrcMessageEventArgs<NickInUseMessage>(this));
        }
    }
}
