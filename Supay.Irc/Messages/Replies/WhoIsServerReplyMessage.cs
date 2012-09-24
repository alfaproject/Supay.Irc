using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply to a <see cref="WhoIsMessage" /> that specifies what server they are on.
    /// </summary>
    [Serializable]
    public class WhoIsServerReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsServerReplyMessage" /> class.
        /// </summary>
        public WhoIsServerReplyMessage()
            : base(312)
        {
            Nick = string.Empty;
            ServerName = string.Empty;
            Info = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick of the user being examined.
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the server the user is connected to.
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets additional information about the user's server connection.
        /// </summary>
        public string Info
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
            parameters.Add(this.ServerName);
            parameters.Add(this.Info);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 3)
            {
                this.Nick = parameters[1];
                this.ServerName = parameters[2];
                this.Info = parameters[3];
            }
            else
            {
                this.Nick = string.Empty;
                this.ServerName = string.Empty;
                this.Info = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoIsServerReply(new IrcMessageEventArgs<WhoIsServerReplyMessage>(this));
        }
    }
}
