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
        private string banId = string.Empty;
        private string channel = string.Empty;

        /// <summary>
        ///   Creates a new instances of the <see cref="BansReplyMessage" /> class.
        /// </summary>
        public BansReplyMessage()
            : base(367)
        {
        }

        /// <summary>
        ///   Gets or sets the channel the ban list refers to.
        /// </summary>
        public virtual string Channel
        {
            get
            {
                return this.channel;
            }
            set
            {
                this.channel = value;
            }
        }

        /// <summary>
        ///   Gets or sets the ban referenced.
        /// </summary>
        public virtual string BanId
        {
            get
            {
                return this.banId;
            }
            set
            {
                this.banId = value;
            }
        }


        #region IChannelTargetedMessage Members

        bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
        {
            return this.IsTargetedAtChannel(channelName);
        }

        #endregion


        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
                parameters.Add(this.Channel);
                parameters.Add(this.BanId);
                return parameters;
            }
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
