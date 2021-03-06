using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is sent to all users with <see cref="Supay.Irc.Messages.Modes.ReceiveWallopsMode" />,
    ///   <see cref="Supay.Irc.Messages.Modes.NetworkOperatorMode" />, or <see cref="Supay.Irc.Messages.Modes.ServerOperatorMode" /> user modes.
    /// </summary>
    [Serializable]
    public class WallopsMessage : CommandMessage
    {
        public WallopsMessage()
        {
            Text = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the text of the <see cref="WallopsMessage" />.
        /// </summary>
        public string Text
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
                return "WALLOPS";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Text);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Text = parameters.Count >= 1 ? parameters[0] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWallops(new IrcMessageEventArgs<WallopsMessage>(this));
        }
    }
}
