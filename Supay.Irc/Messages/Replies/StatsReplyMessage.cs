using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to a <see cref="StatsMessage" /> query.
    /// </summary>
    [Serializable]
    public class StatsReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="StatsReplyMessage" /> class.
        /// </summary>
        public StatsReplyMessage()
            : base(250)
        {
            Stats = string.Empty;
        }

        /// <summary>
        ///   The information requested.
        /// </summary>
        public string Stats
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
            parameters.Add(this.Stats);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Stats = parameters[1];
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnStatsReply(new IrcMessageEventArgs<StatsReplyMessage>(this));
        }
    }
}
