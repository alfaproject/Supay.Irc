using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to the <see cref="SilenceMessage" /> query.
    /// </summary>
    [Serializable]
    public class SilenceReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="SilenceReplyMessage" />.
        /// </summary>
        public SilenceReplyMessage()
            : base(271)
        {
            SilencedUser = new User();
            SilenceListOwner = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the user being silenced.
        /// </summary>
        public User SilencedUser
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the nick of the owner of the silence list
        /// </summary>
        public string SilenceListOwner
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
            parameters.Add(this.SilenceListOwner);
            parameters.Add(this.SilencedUser.IrcMask);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 2)
            {
                this.SilenceListOwner = parameters[1];
                this.SilencedUser = new User(parameters[2]);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSilenceReply(new IrcMessageEventArgs<SilenceReplyMessage>(this));
        }
    }
}
