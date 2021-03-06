using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Sent to a user who is either (a) not on a channel which is mode +n or (b) not a channel op (or mode +v) on a channel which has mode +m set or where the user is banned and is trying to send a PRIVMSG message to that channel.
    /// </summary>
    [Serializable]
    public class CannotSendToChannelMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="CannotSendToChannelMessage" /> class.
        /// </summary>
        public CannotSendToChannelMessage()
            : base(404)
        {
            Channel = string.Empty;
        }

        /// <summary>
        ///   The channel to which the message can't be sent.
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
            parameters.Add("Cannot send to channel");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = parameters.Count > 1 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnCannotSendToChannel(new IrcMessageEventArgs<CannotSendToChannelMessage>(this));
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
