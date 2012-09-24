using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply to a <see cref="ChannelPropertyMessage" /> designed to read one or all channel properties.
    /// </summary>
    [Serializable]
    public class ChannelPropertyReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ChannelPropertyReplyMessage" />.
        /// </summary>
        public ChannelPropertyReplyMessage()
            : base(818)
        {
            Channel = string.Empty;
            Prop = string.Empty;
            Value = string.Empty;
        }

        /// <summary>
        ///   Gets or sets channel being referenced.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the channel property being referenced.
        /// </summary>
        public string Prop
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the value of the channel property.
        /// </summary>
        public string Value
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
            parameters.Add(this.Prop);
            parameters.Add(this.Value);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Channel = string.Empty;
            this.Prop = string.Empty;
            this.Value = string.Empty;

            if (parameters.Count > 1)
            {
                this.Channel = parameters[1];
                if (parameters.Count > 2)
                {
                    this.Prop = parameters[2];
                    if (parameters.Count > 3)
                    {
                        this.Value = parameters[3];
                    }
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnChannelPropertyReply(new IrcMessageEventArgs<ChannelPropertyReplyMessage>(this));
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
