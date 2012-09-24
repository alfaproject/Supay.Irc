using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   With the WhisperMessage, clients can send messages to people within the context of a channel.
    /// </summary>
    [Serializable]
    public class WhisperMessage : CommandMessage, IChannelTargetedMessage
    {
        public WhisperMessage()
        {
            Channel = string.Empty;
            Targets = new List<string>();
            Text = string.Empty;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "WHISPER";
            }
        }

        /// <summary>
        ///   Gets or sets the channel being targeted.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the target of this <see cref="TextMessage" />.
        /// </summary>
        public ICollection<string> Targets
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the actual text of this <see cref="TextMessage" />.
        /// </summary>
        /// <remarks>
        ///   This property holds the core purpose of IRC itself... sending text communication to others.
        /// </remarks>
        public string Text
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
            parameters.Add(string.Join(",", this.Targets));
            parameters.Add(this.Text);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Targets.Clear();
            if (parameters.Count > 2)
            {
                this.Channel = parameters[0];
                foreach (var target in parameters[1].Split(','))
                {
                    Targets.Add(target);
                }
                this.Text = parameters[2];
            }
            else
            {
                this.Channel = string.Empty;
                this.Text = string.Empty;
            }
        }

        /// <summary>
        ///   Validates this message's properties according to the given <see cref="ServerSupport" />.
        /// </summary>
        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhisper(new IrcMessageEventArgs<WhisperMessage>(this));
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
