using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="TopicMessage" /> is used to change or view the topic of a channel.
    /// </summary>
    [Serializable]
    public class TopicMessage : CommandMessage, IChannelTargetedMessage
    {
        private string channel = string.Empty;
        private string topic = string.Empty;

        /// <summary>
        ///   Creates a new instance of the <see cref="TopicMessage" /> class.
        /// </summary>
        public TopicMessage()
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="TopicMessage" /> class for the given channel and topic.
        /// </summary>
        /// <param name="channel">The channel to affect.</param>
        /// <param name="topic">The new topic to set.</param>
        public TopicMessage(string channel, string topic)
        {
            this.channel = channel;
            this.topic = topic;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "TOPIC";
            }
        }

        /// <summary>
        ///   Gets or sets the channel affected
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
        ///   Gets or sets the new Topic to apply
        /// </summary>
        /// <remarks>
        ///   If Topic is blank, the server will send a <see cref="TopicReplyMessage" /> and probably a <see cref="TopicSetReplyMessage" />,
        ///   telling you what the current topic is, who set it, and when.
        /// </remarks>
        public virtual string Topic
        {
            get
            {
                return this.topic;
            }
            set
            {
                this.topic = value;
            }
        }


        #region IChannelTargetedMessage Members

        bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
        {
            return this.IsTargetedAtChannel(channelName);
        }

        #endregion


        /// <summary>
        ///   Validates this message against the given server support
        /// </summary>
        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var parameters = base.Tokens;
                parameters.Add(this.Channel);
                if (!string.IsNullOrEmpty(this.Topic))
                {
                    parameters.Add(this.Topic);
                }
                return parameters;
            }
        }

        /// <summary>
        ///   Parse the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = string.Empty;
            this.Topic = string.Empty;
            if (parameters.Count >= 1)
            {
                this.Channel = parameters[0];
                if (parameters.Count >= 2)
                {
                    this.Topic = parameters[1];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnTopic(new IrcMessageEventArgs<TopicMessage>(this));
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
