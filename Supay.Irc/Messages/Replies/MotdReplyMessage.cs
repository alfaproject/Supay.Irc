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
        /// <summary>
        ///   Creates a new instance of the <see cref="MotdReplyMessage" /> class.
        /// </summary>
        public MotdReplyMessage()
            : base(372)
        {
            Text = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the text of the MOTD line.
        /// </summary>
        public string Text
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
            parameters.Add("- " + this.Text);
            return parameters;
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
