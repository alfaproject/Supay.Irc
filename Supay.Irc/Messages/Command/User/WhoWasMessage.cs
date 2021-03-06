using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Requests information about a user who is no longer connected to IRC.
    /// </summary>
    [Serializable]
    public class WhoWasMessage : CommandMessage
    {
        public WhoWasMessage()
        {
            Nick = string.Empty;
            Server = string.Empty;
            MaximumResults = 1;
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
        ///   Gets or sets the server that should search for the information.
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the maximum number of results to receive.
        /// </summary>
        public int MaximumResults
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
                return "WHOWAS";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Nick);
            if (this.MaximumResults > 0)
            {
                parameters.Add(this.MaximumResults.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(this.Server))
                {
                    parameters.Add(this.Server);
                }
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Nick = string.Empty;
            this.Server = string.Empty;
            this.MaximumResults = 1;

            if (parameters.Count > 0)
            {
                this.Nick = parameters[0];
                if (parameters.Count > 1)
                {
                    this.Server = parameters[1];
                    if (parameters.Count > 2)
                    {
                        this.MaximumResults = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoWas(new IrcMessageEventArgs<WhoWasMessage>(this));
        }
    }
}
