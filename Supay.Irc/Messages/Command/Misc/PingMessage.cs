using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The PingMessage is used to test the presence of an active client at the other end of the connection.
    /// </summary>
    /// <remarks>
    ///   PingMessage is sent at regular intervals if no other activity detected coming from a connection. 
    ///   If a connection fails to respond to a PingMessage within a set amount of time, that connection is closed.
    /// </remarks>
    [Serializable]
    public class PingMessage : CommandMessage
    {
        public PingMessage()
        {
            Target = string.Empty;
            ForwardServer = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the target of the ping.
        /// </summary>
        public string Target
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the server that the ping should be forwarded to.
        /// </summary>
        public string ForwardServer
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "PING";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Target);
            if (!string.IsNullOrEmpty(this.ForwardServer))
            {
                parameters.Add(this.ForwardServer);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.ForwardServer = string.Empty;
            this.Target = string.Empty;
            if (parameters.Count >= 1)
            {
                this.Target = parameters[0];
                if (parameters.Count == 2)
                {
                    this.ForwardServer = parameters[1];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnPing(new IrcMessageEventArgs<PingMessage>(this));
        }
    }
}
