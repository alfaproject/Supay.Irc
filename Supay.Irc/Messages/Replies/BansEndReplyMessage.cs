using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is sent at the end of a channel ban list, when requested. (with MODE #chan +b.)
    /// </summary>
    /// <remarks>
    ///   Numeric: 368
    ///   Name:    RPL_ENDOFBANLIST
    ///   Syntax:  368 channel :info
    ///   Example: 368 #howdy :End of Channel Ban List
    /// </remarks>
    /// <seealso cref="BansReplyMessage" />
    [Serializable]
    public class BansEndReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        private const string DEFAULT_INFO = "End of Channel Ban List";

        /// <summary>
        ///   Creates a new instance of the <see cref="BansEndReplyMessage" /> class.
        /// </summary>
        public BansEndReplyMessage()
            : base(368)
        {
            this.Info = DEFAULT_INFO;
        }

        /// <summary>
        ///   Gets or sets the channel the ban list refers to.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the info message of this reply.
        /// </summary>
        public string Info
        {
            get;
            set;
        }


        #region IChannelTargetedMessage Members

        /// <summary>
        ///   Determines if the the current message is targeted at the given channel.
        /// </summary>
        public virtual bool IsTargetedAtChannel(string channelName)
        {
            return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
        }

        #endregion


        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Channel);
            parameters.Add(string.IsNullOrEmpty(this.Info) ? DEFAULT_INFO : this.Info);
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
                this.Info = parameters.Count > 2 ? parameters[2] : DEFAULT_INFO;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
        ///   current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnBansEndReply(new IrcMessageEventArgs<BansEndReplyMessage>(this));
        }
    }
}
