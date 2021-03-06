using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when the client attempts to remove a nick from his accept list
    ///   when that nick does not exist on the list.
    /// </summary>
    [Serializable]
    public class AcceptDoesNotExistMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="AcceptDoesNotExistMessage" /> class.
        /// </summary>
        public AcceptDoesNotExistMessage()
            : base(458)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick which wasn't added
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
            parameters.Add("is not on your accept list");
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
            conduit.OnAcceptDoesNotExist(new IrcMessageEventArgs<AcceptDoesNotExistMessage>(this));
        }
    }
}
