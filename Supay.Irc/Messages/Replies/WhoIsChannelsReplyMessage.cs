using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Reply to a <see cref="WhoIsMessage" />, stating the channels a user is in.
    /// </summary>
    [Serializable]
    public class WhoIsChannelsReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsChannelsReplyMessage" /> class.
        /// </summary>
        public WhoIsChannelsReplyMessage()
            : base(319)
        {
            Nick = string.Empty;
            Channels = new List<string>();
        }

        /// <summary>
        ///   Gets or sets the Nick of the user being
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the collection of channels the user is a member of.
        /// </summary>
        public ICollection<string> Channels
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
            parameters.Add(this.Nick);
            if (this.Channels.Count != 0)
            {
                parameters.Add(string.Join(" ", this.Channels));
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nick = string.Empty;
            this.Channels.Clear();

            if (parameters.Count == 3)
            {
                this.Nick = parameters[1];
                foreach (var channel in parameters[2].Split(' '))
                {
                    Channels.Add(channel);
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoIsChannelsReply(new IrcMessageEventArgs<WhoIsChannelsReplyMessage>(this));
        }

        /// <summary>
        ///   Determines if the the current message is targeted at the given channel.
        /// </summary>
        protected virtual bool IsTargetedAtChannel(string channelName)
        {
            return this.Channels.Contains(channelName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
