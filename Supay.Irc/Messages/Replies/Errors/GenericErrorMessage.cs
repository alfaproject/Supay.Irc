using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Represents an error message with a numeric command that is either unparsable or unimplemented.
    /// </summary>
    [Serializable]
    public class GenericErrorMessage : ErrorMessage
    {
        public GenericErrorMessage()
        {
            Data = new List<string>();
        }

        /// <summary>
        ///   Gets the text of the Message
        /// </summary>
        public ICollection<string> Data
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
            parameters.Add(string.Join(" ", this.Data));
            return parameters;
        }

        /// <summary>
        ///   Parses the command portion of the message.
        /// </summary>
        protected override void ParseCommand(string command)
        {
            base.ParseCommand(command);
            InternalNumeric = Convert.ToInt32(command, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Data = parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnGenericErrorMessage(new IrcMessageEventArgs<GenericErrorMessage>(this));
        }
    }
}
