using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is sent to all channel operators.
    /// </summary>
    [Serializable]
    public class WallchopsMessage : CommandMessage, IChannelTargetedMessage
    {
        private string channel = string.Empty;
        private string text = string.Empty;

        /// <summary>
        ///   Gets or sets the text of the <see cref="WallchopsMessage" />.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.text = value;
            }
        }

        /// <summary>
        ///   Gets or sets the channel being targeted by the message.
        /// </summary>
        public virtual string Channel
        {
            get
            {
                return this.channel;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                this.channel = value;
            }
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "WALLCHOPS";
            }
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
            parameters.Add(this.Text);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Text = string.Empty;
            this.Channel = string.Empty;

            if (parameters.Count >= 1)
            {
                this.Channel = parameters[0];
                if (parameters.Count >= 2)
                {
                    this.Text = parameters[1];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWallchops(new IrcMessageEventArgs<WallchopsMessage>(this));
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
