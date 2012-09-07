using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   One of the responses to the <see cref="LusersMessage" /> query.
    /// </summary>
    [Serializable]
    public class LusersChannelsReplyMessage : NumericMessage
    {
        private const string CHANNELS_FORMED = "channels formed";
        private int channelCount = -1;

        /// <summary>
        ///   Creates a new instance of the <see cref="LusersChannelsReplyMessage" /> class.
        /// </summary>
        public LusersChannelsReplyMessage()
            : base(254)
        {
        }

        /// <summary>
        ///   Gets or sets the number of channels available.
        /// </summary>
        public virtual int ChannelCount
        {
            get
            {
                return this.channelCount;
            }
            set
            {
                this.channelCount = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.ChannelCount.ToString(CultureInfo.InvariantCulture));
            parameters.Add(CHANNELS_FORMED);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.ChannelCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLusersChannelsReply(new IrcMessageEventArgs<LusersChannelsReplyMessage>(this));
        }
    }
}
