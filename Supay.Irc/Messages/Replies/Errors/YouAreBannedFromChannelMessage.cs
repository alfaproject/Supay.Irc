using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when attempting to join a channel on which the user is banned.
    /// </summary>
    [Serializable]
    public class YouAreBannedFromChannelMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="YouAreBannedFromChannelMessage" /> class.
        /// </summary>
        public YouAreBannedFromChannelMessage()
            : base(474)
        {
        }

        /// <summary>
        ///   Gets or sets the channel on which the user is banned
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
            parameters.Add("Cannot join channel (+b)");
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
            conduit.OnYouAreBannedFromChannel(new IrcMessageEventArgs<YouAreBannedFromChannelMessage>(this));
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
