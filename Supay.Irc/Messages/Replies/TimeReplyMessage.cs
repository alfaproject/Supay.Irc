using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is the reply to the <see cref="TimeMessage" /> server query.
    /// </summary>
    [Serializable]
    public class ServerTimeReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ServerTimeReplyMessage" /> class
        /// </summary>
        public ServerTimeReplyMessage()
            : base(391)
        {
            Time = string.Empty;
            Server = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the server replying to the time request.
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the time value requested.
        /// </summary>
        public string Time
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
            parameters.Add(this.Server);
            parameters.Add(this.Time);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count == 3)
            {
                this.Server = parameters[1];
                this.Time = parameters[2];
            }
            else
            {
                this.Server = string.Empty;
                this.Time = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnServerTimeReply(new IrcMessageEventArgs<ServerTimeReplyMessage>(this));
        }
    }
}
