using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is sent from the server after connection,
    ///   and contains information about the creation of the server.
    /// </summary>
    [Serializable]
    public class ServerCreatedMessage : NumericMessage
    {
        private const string THIS_SERVER_CREATED = "This server was created ";

        /// <summary>
        ///   Creates a new instance of the <see cref="ServerCreatedMessage" /> class.
        /// </summary>
        public ServerCreatedMessage()
            : base(003)
        {
            CreatedDate = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the date on which the server was created.
        /// </summary>
        public string CreatedDate
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
            parameters.Add(THIS_SERVER_CREATED + this.CreatedDate);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            string reply = parameters[1];
            if (reply.IndexOf(THIS_SERVER_CREATED, StringComparison.Ordinal) != -1)
            {
                int startOfDate = THIS_SERVER_CREATED.Length;
                this.CreatedDate = reply.Substring(startOfDate);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnServerCreated(new IrcMessageEventArgs<ServerCreatedMessage>(this));
        }
    }
}
