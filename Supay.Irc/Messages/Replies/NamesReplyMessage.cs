using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A single reply to the <see cref="NamesMessage" /> query.
    /// </summary>
    [Serializable]
    public class NamesReplyMessage : NumericMessage, IChannelTargetedMessage
    {
        #region ChannelVisibility enum

        /// <summary>
        ///   The list of channel visibility settings for the <see cref="NamesReplyMessage" />.
        /// </summary>
        public enum ChannelVisibility
        {
            /// <summary>
            ///   The channel is in <see cref="Supay.Irc.Messages.Modes.SecretMode" />
            /// </summary>
            Secret,

            /// <summary>
            ///   The channel is in <see cref="Supay.Irc.Messages.Modes.PrivateMode" />
            /// </summary>
            Private,

            /// <summary>
            ///   The channel has no hidden modes applied.
            /// </summary>
            Public
        }

        #endregion

        /// <summary>
        ///   Creates a new instance of the <see cref="NamesReplyMessage" /> class.
        /// </summary>
        public NamesReplyMessage()
            : base(353)
        {
            this.Channel = string.Empty;
            this.Visibility = ChannelVisibility.Public;
            this.Nicks = new Dictionary<string, ChannelStatus>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   Gets or sets the visibility of the channel specified in the reply.
        /// </summary>
        public ChannelVisibility Visibility
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the channel specified in the reply.
        /// </summary>
        public string Channel
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the collection of nicks in the channel.
        /// </summary>
        public Dictionary<string, ChannelStatus> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            switch (this.Visibility)
            {
                case ChannelVisibility.Public:
                    parameters.Add("=");
                    break;
                case ChannelVisibility.Private:
                    parameters.Add("*");
                    break;
                case ChannelVisibility.Secret:
                    parameters.Add("@");
                    break;
            }
            parameters.Add(this.Channel);
            parameters.Add(string.Join(" ", this.Nicks.Select(user => user.Value.Symbol + user.Key)));
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            string nicknamesWithStatuses;
            switch (parameters.Count)
            {
                case 3:
                    this.Visibility = ChannelVisibility.Public;
                    this.Channel = parameters[1];
                    nicknamesWithStatuses = parameters[2];
                    break;
                case 4:
                    switch (parameters[1])
                    {
                        case "=":
                            this.Visibility = ChannelVisibility.Public;
                            break;
                        case "*":
                            this.Visibility = ChannelVisibility.Private;
                            break;
                        case "@":
                            this.Visibility = ChannelVisibility.Secret;
                            break;
                        default:
                            throw new InvalidMessageException("Unknown visibility mode in RPL_NAMREPLY.");
                    }
                    this.Channel = parameters[2];
                    nicknamesWithStatuses = parameters[3];
                    break;
                default:
                    throw new InvalidMessageException("Invalid number of parameters in RPL_NAMREPLY.");
            }

            this.Nicks.Clear();
            foreach (var nicknameWithStatus in nicknamesWithStatuses.Split(' '))
            {
                var status = ChannelStatus.None;
                
                var nickname = nicknameWithStatus;
                if (nickname.Length > 1)
                {
                    var firstLetter = nickname.Substring(0, 1);
                    if (ChannelStatus.IsDefined(firstLetter))
                    {
                        status = ChannelStatus.GetInstance(firstLetter);
                        nickname = nickname.Substring(1);
                    }
                }

                this.Nicks.Add(nickname, status);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnNamesReply(new IrcMessageEventArgs<NamesReplyMessage>(this));
        }

        /// <summary>
        ///   Determines if the the current message is targeted at the given channel.
        /// </summary>
        public virtual bool IsTargetedAtChannel(string channelName)
        {
            return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
