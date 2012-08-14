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
        private int maximumResults = 1;
        private string nick = string.Empty;
        private string server = string.Empty;

        /// <summary>
        ///   Gets or sets the nick of the user being examined.
        /// </summary>
        public virtual string Nick
        {
            get
            {
                return this.nick;
            }
            set
            {
                this.nick = value;
            }
        }

        /// <summary>
        ///   Gets or sets the server that should search for the information.
        /// </summary>
        public virtual string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum number of results to receive.
        /// </summary>
        public virtual int MaximumResults
        {
            get
            {
                return this.maximumResults;
            }
            set
            {
                this.maximumResults = value;
            }
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
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
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
