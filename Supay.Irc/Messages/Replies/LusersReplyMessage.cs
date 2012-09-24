using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   One of the responses to the <see cref="LusersMessage" /> query.
    /// </summary>
    [Serializable]
    public class LusersReplyMessage : NumericMessage
    {
        private const string THERE_ARE = "There are ";
        private const string USERS_AND = " users and ";
        private const string INVISIBLE_ON = " invisible on ";
        private const string SERVERS = " servers";

        /// <summary>
        ///   Creates a new instance of the <see cref="LusersReplyMessage" /> class.
        /// </summary>
        public LusersReplyMessage()
            : base(251)
        {
            UserCount = -1;
            InvisibleCount = -1;
            ServerCount = -1;
        }

        /// <summary>
        ///   Gets or sets the number of users connected to IRC.
        /// </summary>
        public int UserCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of invisible users connected to IRC.
        /// </summary>
        public int InvisibleCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of servers connected on the network.
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
            parameters.Add(THERE_ARE + this.UserCount + USERS_AND + this.InvisibleCount + INVISIBLE_ON + this.ServerCount + SERVERS);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            string payload = parameters[1];
            this.UserCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, THERE_ARE, USERS_AND), CultureInfo.InvariantCulture);
            this.InvisibleCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, USERS_AND, INVISIBLE_ON), CultureInfo.InvariantCulture);
            this.ServerCount = Convert.ToInt32(MessageUtil.StringBetweenStrings(payload, INVISIBLE_ON, SERVERS), CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLusersReply(new IrcMessageEventArgs<LusersReplyMessage>(this));
        }
    }
}
