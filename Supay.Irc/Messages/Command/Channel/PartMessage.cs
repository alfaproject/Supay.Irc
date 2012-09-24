using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The PartMessage causes the client sending the message to be removed 
    ///   from the list of active users for all given channels listed in the <see cref="PartMessage.Channels" /> property.
    /// </summary>
    [Serializable]
    public class PartMessage : CommandMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="PartMessage" /> class with the given channel.
        /// </summary>
        public PartMessage(params string[] channels)
        {
            Channels = new List<string>(channels);
            Reason = string.Empty;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="PartMessage" /> class.
        /// </summary>
        public PartMessage()
            : this(new string[] { })
        {
        }

        /// <summary>
        ///   Gets the channel name parted.
        /// </summary>
        public ICollection<string> Channels
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the reason for the part.
        /// </summary>
        public string Reason
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "PART";
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
            parameters.Add(string.Join(",", this.Channels));
            if (!string.IsNullOrEmpty(this.Reason))
            {
                parameters.Add(this.Reason);
            }
            return parameters;
        }

        /// <summary>
        ///   Validates this message against the given server support
        /// </summary>
        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            Channels = (from channel in Channels
                        select MessageUtil.EnsureValidChannelName(channel, serverSupport)).ToList();
        }

        /// <summary>
        ///   Parse the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            Channels.Clear();
            if (parameters.Count != 0)
            {
                foreach (var channel in parameters[0].Split(','))
                {
                    Channels.Add(channel);
                }

                if (parameters.Count > 1)
                {
                    this.Reason = parameters[1];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnPart(new IrcMessageEventArgs<PartMessage>(this));
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
