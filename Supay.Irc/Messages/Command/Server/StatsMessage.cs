using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A request for some information about the server.
    /// </summary>
    [Serializable]
    public class StatsMessage : ServerQueryBase
    {
        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "STATS";
            }
        }

        /// <summary>
        ///   Gets or sets the code the what information is requested.
        /// </summary>
        public string Query
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the index of the parameter which holds the server which should respond to the query.
        /// </summary>
        protected override int TargetParsingPosition
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            if (!string.IsNullOrEmpty(this.Query))
            {
                parameters.Add(this.Query);
                parameters.Add(this.Target);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Query = parameters.Count >= 1 ? parameters[0] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnStats(new IrcMessageEventArgs<StatsMessage>(this));
        }
    }
}
