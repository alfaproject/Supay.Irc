using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The InviteMessage is used to invite users to a channel.
    /// </summary>
    /// <remarks>
    ///   This message wraps the INVITE command.
    /// </remarks>
    [Serializable]
    public class InviteMessage : CommandMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="InviteMessage" /> class with the given channel and nick.
        /// </summary>
        /// <param name="channel">The channel the person is being invited into.</param>
        /// <param name="nick">The nick of the user invited</param>
        public InviteMessage(string channel, string nick)
        {
            this.Channel = channel;
            this.Nick = nick;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="InviteMessage" /> class.
        /// </summary>
        public InviteMessage()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///   Gets or sets the channel the person is being invited into.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the nick of the user invited
        /// </summary>
        public string Nick
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
                return "INVITE";
            }
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
            this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Channel);
            parameters.Add(this.Nick);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 2)
            {
                this.Channel = parameters[0];
                this.Nick = parameters[1];
            }
            else
            {
                this.Channel = string.Empty;
                this.Nick = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnInvite(new IrcMessageEventArgs<InviteMessage>(this));
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
