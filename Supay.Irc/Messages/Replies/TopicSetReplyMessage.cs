using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply sent when the server acknowledges that a channel's topic has been changed.
    /// </summary>
    [Serializable]
    public class TopicSetReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="TopicSetReplyMessage" /> class.
        /// </summary>
        public TopicSetReplyMessage()
            : base(333)
        {
            Channel = string.Empty;
            User = new User();
            TimeSet = DateTime.Now;
        }

        /// <summary>
        ///   Gets or sets the channel with the changed topic.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the user which changed the topic.
        /// </summary>
        public User User
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the time at which the topic was changed.
        /// </summary>
        public DateTime TimeSet
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
            parameters.Add(this.User.IrcMask);
            parameters.Add(MessageUtil.ConvertToUnixTime(this.TimeSet).ToString(CultureInfo.InvariantCulture));
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = parameters[1];
            this.User = new User(parameters[2]);
            this.TimeSet = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnTopicSetReply(new IrcMessageEventArgs<TopicSetReplyMessage>(this));
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
