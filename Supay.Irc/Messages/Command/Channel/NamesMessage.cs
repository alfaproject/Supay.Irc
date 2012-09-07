using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   By using the NamesMessage, a user can list all nicknames that are visible to them on any channel that they can see.
    /// </summary>
    /// <remarks>
    ///   Channel names which they can see are those which aren't private ( <see cref="Supay.Irc.Messages.Modes.PrivateMode" /> ) or secret ( <see cref="Supay.Irc.Messages.Modes.SecretMode" /> ) or those which they are actually on.
    /// </remarks>
    [Serializable]
    public class NamesMessage : CommandMessage
    {
        private readonly List<string> channels = new List<string>();

        /// <summary>
        ///   Gets the channels that should be queried for their users.
        /// </summary>
        public virtual List<string> Channels
        {
            get
            {
                return this.channels;
            }
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "NAMES";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
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
            this.Channels.Clear();
            if (parameters.Count >= 1)
            {
                this.Channels.AddRange(parameters[0].Split(','));
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnNames(new IrcMessageEventArgs<NamesMessage>(this));
        }
    }
}
