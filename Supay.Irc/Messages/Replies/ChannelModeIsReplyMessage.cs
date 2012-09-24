using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is the reply to an empty <see cref="ChannelModeMessage" />.
    /// </summary>
    [Serializable]
    public class ChannelModeIsReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ChannelModeIsReplyMessage" /> class.
        /// </summary>
        public ChannelModeIsReplyMessage()
            : base(324)
        {
            Channel = string.Empty;
            Modes = string.Empty;
            ModeArguments = new List<string>();
        }

        /// <summary>
        ///   Gets or sets the channel referred to.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the modes in effect.
        /// </summary>
        /// <remarks>
        ///   An example Modes might look like "+ml".
        /// </remarks>
        public string Modes
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the collection of arguments ( parameters ) for the <see cref="ChannelModeIsReplyMessage.Modes" /> property.
        /// </summary>
        /// <remarks>
        ///   Some modes require a parameter, such as +l ( user limit ) requires the number being limited to.
        /// </remarks>
        public IList<string> ModeArguments
        {
            get;
            private set;
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
            parameters.Add(this.Modes);
            if (this.ModeArguments.Count != 0)
            {
                parameters.Add(string.Join(" ", this.ModeArguments));
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.ModeArguments.Clear();
            if (parameters.Count > 2)
            {
                this.Channel = parameters[1];
                this.Modes = parameters[2];
                for (int i = 3; i < parameters.Count; i++)
                {
                    this.ModeArguments.Add(parameters[i]);
                }
            }
            else
            {
                this.Channel = string.Empty;
                this.Modes = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnChannelModeIsReply(new IrcMessageEventArgs<ChannelModeIsReplyMessage>(this));
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
