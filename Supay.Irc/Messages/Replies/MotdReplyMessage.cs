using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   One line of data in a reply to the <see cref="MotdMessage" /> query.
    /// </summary>
    [Serializable]
    public class MotdReplyMessage : NumericMessage
    {
        private string text = string.Empty;

        /// <summary>
        ///   Creates a new instance of the <see cref="MotdReplyMessage" /> class.
        /// </summary>
        public MotdReplyMessage()
            : base(372)
        {
        }

        /// <summary>
        ///   Gets or sets the text of the MOTD line.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
                parameters.Add("- " + this.Text);
                return parameters;
            }
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            string lastOne = parameters[parameters.Count - 1];
            if (lastOne.StartsWith("- ", StringComparison.Ordinal))
            {
                this.Text = lastOne.Substring(2);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMotdReply(new IrcMessageEventArgs<MotdReplyMessage>(this));
        }
    }
}
