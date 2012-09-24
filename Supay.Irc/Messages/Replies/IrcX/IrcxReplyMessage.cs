using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply to a <see cref="IrcxMessage" /> or a <see cref="IsIrcxMessage" />.
    /// </summary>
    [Serializable]
    public class IrcxReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="IrcxReplyMessage" />.
        /// </summary>
        public IrcxReplyMessage()
            : base(800)
        {
            Version = string.Empty;
            AuthenticationPackages = new List<string>();
            MaximumMessageLength = -1;
            Options = "*";
        }

        /// <summary>
        ///   Gets or sets if the server has set the client into IRCX mode.
        /// </summary>
        public bool IsIrcxClientMode
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the version of IRCX the server implements.
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the collection of authentication packages
        /// </summary>
        public ICollection<string> AuthenticationPackages
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the maximum message length, in bytes.
        /// </summary>
        public int MaximumMessageLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <remarks>
        /// There are no known servers that implement this property.
        /// It is almost always just *.
        /// </remarks>
        public string Options
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
            parameters.Add(this.IsIrcxClientMode ? "1" : "0");
            parameters.Add(this.Version);
            parameters.Add(string.Join(",", this.AuthenticationPackages));
            parameters.Add(this.MaximumMessageLength.ToString(CultureInfo.InvariantCulture));
            parameters.Add(this.Options);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 5)
            {
                this.IsIrcxClientMode = parameters[1] == "1";
                this.Version = parameters[2];
                this.AuthenticationPackages.Clear();
                foreach (string package in parameters[3].Split(','))
                {
                    this.AuthenticationPackages.Add(package);
                }
                this.MaximumMessageLength = int.Parse(parameters[4], CultureInfo.InvariantCulture);
                this.Options = parameters.Count == 6 ? parameters[5] : string.Empty;
            }
            else
            {
                this.IsIrcxClientMode = false;
                this.Version = string.Empty;
                this.AuthenticationPackages.Clear();
                this.MaximumMessageLength = -1;
                this.Options = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnIrcxReply(new IrcMessageEventArgs<IrcxReplyMessage>(this));
        }
    }
}
