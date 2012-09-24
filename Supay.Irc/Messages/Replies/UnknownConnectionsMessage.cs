using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message indicates the number of unknown connections on the server.
    /// </summary>
    /// <remarks>
    ///   This is most likely the number of clients who are starting to connect 
    ///   but not yet registered with the server. (fully connected) 
    ///   Unsure if this is server only or network wide. 
    ///   Note that if there are no unknown connections, this will not be sent.
    /// </remarks>
    [Serializable]
    public class UnknownConnectionsMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="UnknownConnectionsMessage" /> class.
        /// </summary>
        public UnknownConnectionsMessage()
            : base(253)
        {
            UnknownConnectionCount = -1;
        }

        /// <summary>
        ///   Gets or sets the number of unknown connections
        /// </summary>
        public int UnknownConnectionCount
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
            parameters.Add(this.UnknownConnectionCount.ToString(CultureInfo.InvariantCulture));
            parameters.Add("unknown connection(s)");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.UnknownConnectionCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUnknownConnections(new IrcMessageEventArgs<UnknownConnectionsMessage>(this));
        }
    }
}
