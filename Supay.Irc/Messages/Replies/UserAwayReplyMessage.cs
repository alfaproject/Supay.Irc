using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The message is received by a client from a server 
    ///   when they attempt to send a message to a user who
    ///   is marked as away using the <see cref="AwayMessage" />.
    /// </summary>
    [Serializable]
    public class UserAwayMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="UserAwayMessage" />.
        /// </summary>
        public UserAwayMessage()
            : base(301)
        {
            Text = string.Empty;
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the user's away message.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the nick of the user who is away.
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
            parameters.Add(this.Text);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 2)
            {
                this.Nick = parameters[1];
                this.Text = parameters[2];
            }
            else
            {
                this.Nick = string.Empty;
                this.Text = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUserAway(new IrcMessageEventArgs<UserAwayMessage>(this));
        }
    }
}
