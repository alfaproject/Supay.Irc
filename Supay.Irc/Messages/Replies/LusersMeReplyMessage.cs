using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   One of the responses to the <see cref="LusersMessage" /> query.
    /// </summary>
    [Serializable]
    public class LusersMeReplyMessage : NumericMessage
    {
        private const string I_HAVE = "I have ";
        private const string CLIENTS_AND = " clients and ";
        private const string SERVERS = " servers";

        /// <summary>
        ///   Creates a new instance of the <see cref="LusersMeReplyMessage" /> class.
        /// </summary>
        public LusersMeReplyMessage()
            : base(255)
        {
            ClientCount = -1;
            ServerCount = -1;
        }

        /// <summary>
        ///   Gets or sets the number of clients connected to the server.
        /// </summary>
        public int ClientCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of servers linked to the current server.
        /// </summary>
        public int ServerCount
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
            parameters.Add(I_HAVE + this.ClientCount + CLIENTS_AND + this.ServerCount + SERVERS);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            string payload = parameters[1];
            this.ClientCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, I_HAVE, CLIENTS_AND), CultureInfo.InvariantCulture);
            this.ServerCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, CLIENTS_AND, SERVERS), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLusersMeReply(new IrcMessageEventArgs<LusersMeReplyMessage>(this));
        }
    }
}
