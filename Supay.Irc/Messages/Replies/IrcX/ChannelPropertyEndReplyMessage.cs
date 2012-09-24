using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Marks the end of the replies to a <see cref="ChannelPropertyMessage" /> designed to read one or all channel properties.
    /// </summary>
    [Serializable]
    public class ChannelPropertyEndReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ChannelPropertyEndReplyMessage" /> class.
        /// </summary>
        public ChannelPropertyEndReplyMessage()
            : base(819)
        {
            Channel = string.Empty;
        }

        /// <summary>
        ///   Gets or sets channel being referenced.
        /// </summary>
        public string Channel
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
            parameters.Add(this.Channel);
            parameters.Add("End of properties");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 1)
            {
                this.Channel = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnChannelPropertyEndReply(new IrcMessageEventArgs<ChannelPropertyEndReplyMessage>(this));
        }
    }
}
