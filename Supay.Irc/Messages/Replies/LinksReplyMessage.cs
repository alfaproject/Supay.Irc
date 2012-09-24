using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to the <see cref="LinksMessage" /> query.
    /// </summary>
    [Serializable]
    public class LinksReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="LinksReplyMessage" />.
        /// </summary>
        public LinksReplyMessage()
            : base(364)
        {
            Mask = string.Empty;
            Server = string.Empty;
            HopCount = -1;
            ServerInfo = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the mask which will limit the list of returned servers.
        /// </summary>
        public string Mask
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the server which should respond.
        /// </summary>
        /// <remarks>
        ///   If empty, the current server is used.
        /// </remarks>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of hops from the answering server to the listed server.
        /// </summary>
        public int HopCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets any additional server information.
        /// </summary>
        public string ServerInfo
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
            parameters.Add(this.Mask);
            parameters.Add(this.Server);
            parameters.Add(this.HopCount.ToString(CultureInfo.InvariantCulture) + " " + this.ServerInfo);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count == 4)
            {
                this.Mask = parameters[1];
                this.Server = parameters[2];
                string trailing = parameters[3];
                string first = trailing.Substring(0, trailing.IndexOf(" ", StringComparison.Ordinal));
                this.HopCount = Convert.ToInt32(first, CultureInfo.InvariantCulture);
                this.ServerInfo = trailing.Substring(first.Length);
            }
            else
            {
                this.Mask = string.Empty;
                this.Server = string.Empty;
                this.HopCount = -1;
                this.ServerInfo = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLinksReply(new IrcMessageEventArgs<LinksReplyMessage>(this));
        }
    }
}
