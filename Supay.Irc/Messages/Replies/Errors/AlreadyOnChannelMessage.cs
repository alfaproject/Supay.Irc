using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when a user tries to invite a person onto a channel which they
    ///   are already on
    /// </summary>
    [Serializable]
    public class AlreadyOnChannelMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="AlreadyOnChannelMessage" /> class.
        /// </summary>
        public AlreadyOnChannelMessage()
            : base(443)
        {
        }

        /// <summary>
        ///   Gets or sets the nick of the user invited
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the channel being invited to
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


        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Nick);
            parameters.Add(this.Channel);
            parameters.Add("is already on channel");
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
            conduit.OnAlreadyOnChannel(new IrcMessageEventArgs<AlreadyOnChannelMessage>(this));
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
