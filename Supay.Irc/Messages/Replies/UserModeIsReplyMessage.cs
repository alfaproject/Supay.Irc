using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is the reply to an empty <see cref="UserModeMessage" />.
    /// </summary>
    [Serializable]
    public class UserModeIsReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="UserModeIsReplyMessage" /> class.
        /// </summary>
        public UserModeIsReplyMessage()
            : base(221)
        {
            Modes = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the modes in effect.
        /// </summary>
        /// <remarks>
        ///   An example Modes might look like "+i".
        /// </remarks>
        public string Modes
        {
            get;
            set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Modes);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Modes = parameters.Count >= 1 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUserModeIsReply(new IrcMessageEventArgs<UserModeIsReplyMessage>(this));
        }
    }
}
