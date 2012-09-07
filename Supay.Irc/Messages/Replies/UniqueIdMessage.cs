using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is sent by the server early during connection, and tells the user the alpha-numeric id the server uses to identify the user.
    /// </summary>
    [Serializable]
    public class UniqueIdMessage : NumericMessage
    {
        private const string YOUR_UNIQUE_ID = "your unique ID";

        /// <summary>
        ///   Creates a new instance of the <see cref="UniqueIdMessage" /> class.
        /// </summary>
        public UniqueIdMessage()
            : base(042)
        {
        }

        /// <summary>
        ///   Gets or sets the alpha-numeric id the server uses to identify the client.
        /// </summary>
        public string UniqueId
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
            parameters.Add(this.Target);
            parameters.Add(this.UniqueId);
            parameters.Add(YOUR_UNIQUE_ID);
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
                this.UniqueId = parameters[2];
            }
            else
            {
                this.UniqueId = string.Empty;
                Trace.WriteLine("Unknown format of UniqueIDMessage parameters: '" + MessageUtil.ParametersToString(parameters) + "'");
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUniqueId(new IrcMessageEventArgs<UniqueIdMessage>(this));
        }
    }
}
