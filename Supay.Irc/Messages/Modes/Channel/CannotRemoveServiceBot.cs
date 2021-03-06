using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when a user tries to kill, kick, or de-op a bot which provides channel services.
    /// </summary>
    [Serializable]
    public class CannotRemoveServiceBotMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="CannotRemoveServiceBotMessage" /> class.
        /// </summary>
        public CannotRemoveServiceBotMessage()
            : base(484)
        {
        }

        /// <summary>
        ///   Gets or sets the nick of the bot
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the channel on which the bot resides
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Nick);
            parameters.Add(this.Channel);
            parameters.Add("Cannot kill, kick or de-op channel service");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nick = string.Empty;
            this.Channel = string.Empty;
            if (parameters.Count > 2)
            {
                this.Nick = parameters[1];
                this.Channel = parameters[2];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnCannotRemoveServiceBot(new IrcMessageEventArgs<CannotRemoveServiceBotMessage>(this));
        }
    }
}
