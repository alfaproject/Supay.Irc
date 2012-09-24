using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The KickMessage can be used to forcibly remove a user from a channel.
    ///   It 'kicks them out' of the channel.
    /// </summary>
    /// <remarks>
    ///   Only a channel operator may kick another user out of a channel.
    ///   This message wraps the KICK message.
    /// </remarks>
    [Serializable]
    public class KickMessage : CommandMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="KickMessage" /> class with the given channel and nick.
        /// </summary>
        /// <param name="channel">The name of the channel affected.</param>
        /// <param name="nick">The nick of the user being kicked out.</param>
        public KickMessage(string channel, string nick)
        {
            Channels = new List<string>(new[] { channel });
            Nicks = new List<string>(new[] { nick });
            Reason = string.Empty;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="KickMessage" /> class.
        /// </summary>
        public KickMessage()
        {
            Channels = new List<string>();
            Nicks = new List<string>();
            Reason = string.Empty;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "KICK";
            }
        }

        /// <summary>
        ///   Gets the channels affected.
        /// </summary>
        public IList<string> Channels
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the nicks of the users being kicked.
        /// </summary>
        public IList<string> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the reason for the kick
        /// </summary>
        public string Reason
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
        ///   Validates this message against the given server support
        /// </summary>
        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            if (serverSupport == null)
            {
                return;
            }
            if (this.Reason.Length > serverSupport.MaxKickCommentLength)
            {
                this.Reason = this.Reason.Substring(0, serverSupport.MaxKickCommentLength);
            }
            for (int i = 0; i < this.Channels.Count; i++)
            {
                this.Channels[i] = MessageUtil.EnsureValidChannelName(this.Channels[i], serverSupport);
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(string.Join(",", this.Channels));
            parameters.Add(string.Join(",", this.Nicks));
            if (!string.IsNullOrEmpty(this.Reason))
            {
                parameters.Add(this.Reason);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Channels.Clear();
            this.Nicks.Clear();
            this.Reason = string.Empty;
            if (parameters.Count >= 2)
            {
                foreach (var channel in parameters[0].Split(','))
                {
                    Channels.Add(channel);
                }

                foreach (var nick in parameters[1].Split(','))
                {
                    Nicks.Add(nick);
                }

                if (parameters.Count >= 3)
                {
                    this.Reason = parameters[2];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnKick(new IrcMessageEventArgs<KickMessage>(this));
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
