using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when a user tries to perform a command which
    ///   requires channel-operator status.
    /// </summary>
    /// <remarks>
    ///   Channel-operator status is set with the <see cref="OperatorMode" /> of the
    ///   <see cref="ChannelModeMessage" />.
    /// </remarks>
    [Serializable]
    public class ChannelOperatorStatusRequiredMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="ChannelOperatorStatusRequiredMessage" /> class.
        /// </summary>
        public ChannelOperatorStatusRequiredMessage()
            : base(482)
        {
        }

        /// <summary>
        ///   Gets or sets the channel on which the command requires <see cref="OperatorMode" /> status.
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
            parameters.Add(this.Channel);
            parameters.Add("You're not channel operator");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = string.Empty;
            if (parameters.Count > 1)
            {
                this.Channel = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnChannelOperatorStatusRequired(new IrcMessageEventArgs<ChannelOperatorStatusRequiredMessage>(this));
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
