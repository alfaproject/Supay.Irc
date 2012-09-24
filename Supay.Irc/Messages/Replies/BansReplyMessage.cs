using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Contains a Channel and BanId as one of possible many replies to a ban list request.
    /// </summary>
    [Serializable]
    public class BansReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="BansReplyMessage" /> class.
        /// </summary>
        public BansReplyMessage()
            : base(367)
        {
            Channel = string.Empty;
            BanId = string.Empty;
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
        ///   Gets or sets the ban referenced.
        /// </summary>
        public string BanId
        {
            get;
            set;
        }


        #region IChannelTargetedMessage Members

        bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
        {
            return this.IsTargetedAtChannel(channelName);
        }

        #endregion


        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Channel);
            parameters.Add(this.BanId);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 2)
            {
                this.Channel = parameters[1];
                this.BanId = parameters[2];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnBansReply(new IrcMessageEventArgs<BansReplyMessage>(this));
        }

        /// <summary>
        ///   Determines if the the current message is targeted at the given channel.
        /// </summary>
        protected virtual bool IsTargetedAtChannel(string channelName)
        {
            return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
