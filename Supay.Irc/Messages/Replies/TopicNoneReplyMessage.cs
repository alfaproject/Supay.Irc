using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to a <see cref="TopicMessage" /> which requests the topic, 
    ///   and there is none set.
    /// </summary>
    [Serializable]
    public class TopicNoneReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="TopicNoneReplyMessage" /> class.
        /// </summary>
        public TopicNoneReplyMessage()
            : base(331)
        {
            Channel = string.Empty;
        }

        /// <summary>
        ///   The name of the channel which has no topic set.
        /// </summary>
        public string Channel
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
            parameters.Add("No topic is set");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = parameters.Count >= 2 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnTopicNoneReply(new IrcMessageEventArgs<TopicNoneReplyMessage>(this));
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
