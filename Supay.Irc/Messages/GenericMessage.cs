using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Represents a single generic RFC1459 IRC message to or from an IRC server.
    /// </summary>
    [Serializable]
    public class GenericMessage : IrcMessage
    {
        public GenericMessage()
        {
            this.Command = string.Empty;
            this.Parameters = new List<string>();
        }

        /// <summary>
        ///   Gets or sets the message's command.
        /// </summary>
        public string Command
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the message's parameters after the command.
        /// </summary>
        public IList<string> Parameters
        {
            get;
            private set;
        }

        #region IrcMessage Methods

        /// <summary>
        ///   This is not meant to be used from your code.
        /// </summary>
        /// <remarks>
        ///   The conduit calls Notify on messages to have the message raise the appropriate event on the conduit.
        ///   This is done automatically by your <see cref="Client" /> after messages are received and parsed.
        /// </remarks>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnGenericMessage(new IrcMessageEventArgs<GenericMessage>(this));
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = new List<string> {
                this.Command
            };
            parameters.AddRange(this.Parameters);
            return parameters;
        }

        /// <summary>
        ///   Determines if the given string is parsable by this <see cref="IrcMessage" /> subclass.
        /// </summary>
        /// <remarks>
        ///   <see cref="GenericMessage" /> always returns true.
        /// </remarks>
        public override bool CanParse(string unparsedMessage)
        {
            return true;
        }

        /// <summary>
        ///   Parses the command portion of the message.
        /// </summary>
        protected override void ParseCommand(string command)
        {
            base.ParseCommand(command);
            this.Command = command;
        }

        /// <summary>
        ///   Parses the parameter portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Parameters = parameters;
        }

        #endregion
    }
}
