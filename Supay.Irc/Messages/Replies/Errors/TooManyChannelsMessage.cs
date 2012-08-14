using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Sent to a user when they have joined the maximum number of allowed channels and they try to join another channel.
    /// </summary>
    [Serializable]
    public class TooManyChannelsMessage : ErrorMessage, IChannelTargetedMessage
    {
        private string channel = string.Empty;

        /// <summary>
        ///   Creates a new instances of the <see cref="TooManyChannelsMessage" /> class.
        /// </summary>
        public TooManyChannelsMessage()
            : base(405)
        {
        }

        /// <summary>
        ///   The channel to which entry was denied.
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
                parameters.Add("You have joined too many channels");
                return parameters;
            }
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
            conduit.OnTooManyChannels(new IrcMessageEventArgs<TooManyChannelsMessage>(this));
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
