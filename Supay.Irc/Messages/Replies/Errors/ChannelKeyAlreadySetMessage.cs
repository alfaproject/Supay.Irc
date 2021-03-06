using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when attempting to set a key on a channel which already
    ///   has a key set.
    /// </summary>
    /// <remarks>
    ///   A channel must have its key removed before setting a new one. This is done with a
    ///   <see cref="ChannelModeMessage" /> and the <see cref="KeyMode" />.
    /// </remarks>
    [Serializable]
    public class ChannelKeyAlreadySetMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="ChannelKeyAlreadySetMessage" /> class.
        /// </summary>
        public ChannelKeyAlreadySetMessage()
            : base(467)
        {
        }

        /// <summary>
        ///   Gets or sets the channel which has the key set
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
            parameters.Add("Channel key already set");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channel = string.Empty;
            if (parameters.Count > 2)
            {
                this.Channel = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnChannelKeyAlreadySet(new IrcMessageEventArgs<ChannelKeyAlreadySetMessage>(this));
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
