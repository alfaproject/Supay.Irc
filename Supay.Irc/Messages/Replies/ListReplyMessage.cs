using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A single reply to the <see cref="ListMessage" /> query.
    /// </summary>
    [Serializable]
    public class ListReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ListReplyMessage" /> class.
        /// </summary>
        public ListReplyMessage()
            : base(322)
        {
            Channel = string.Empty;
            MemberCount = -1;
            Topic = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the channel for this reply.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of people in the channel.
        /// </summary>
        public int MemberCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the topic of the channel.
        /// </summary>
        public string Topic
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
            parameters.Add(this.MemberCount.ToString(CultureInfo.InvariantCulture));
            parameters.Add(this.Topic);
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
                this.Channel = parameters[1];
                this.MemberCount = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
                this.Topic = parameters[3];
            }
            else
            {
                this.Channel = string.Empty;
                this.MemberCount = -1;
                this.Topic = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnListReply(new IrcMessageEventArgs<ListReplyMessage>(this));
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
