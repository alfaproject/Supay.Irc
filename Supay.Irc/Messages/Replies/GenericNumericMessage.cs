using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Represents a message with a numeric command that is either unparsable or unimplemented.
    /// </summary>
    [Serializable]
    public class GenericNumericMessage : NumericMessage
    {
        private ICollection<string> data = new List<string>();

        /// <summary>
        ///   Gets the Numeric command of the Message
        /// </summary>
        public virtual int Command
        {
            get
            {
                return this.InternalNumeric;
            }
            set
            {
                this.InternalNumeric = value;
            }
        }

        /// <summary>
        ///   Gets the text of the Message
        /// </summary>
        public virtual ICollection<string> Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            foreach (string datum in this.Data)
            {
                parameters.Add(datum);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the command portion of the message.
        /// </summary>
        protected override void ParseCommand(string command)
        {
            base.ParseCommand(command);
            this.Command = Convert.ToInt32(command, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.data = parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnGenericNumericMessage(new IrcMessageEventArgs<GenericNumericMessage>(this));
        }
    }
}
