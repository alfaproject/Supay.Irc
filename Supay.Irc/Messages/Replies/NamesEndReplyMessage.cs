using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Marks the end of the replies to a <see cref="NamesMessage" /> query.
    /// </summary>
    [Serializable]
    public class NamesEndReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="NamesEndReplyMessage" /> class.
        /// </summary>
        public NamesEndReplyMessage()
            : base(366)
        {
            Channel = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the channel to which this reply list ends.
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
            parameters.Add("End of /NAMES list.");
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
            conduit.OnNamesEndReply(new IrcMessageEventArgs<NamesEndReplyMessage>(this));
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
