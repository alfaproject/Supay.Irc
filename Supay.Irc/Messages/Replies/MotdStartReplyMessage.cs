using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Signifies the start of the MOTD sent by the server.
    /// </summary>
    [Serializable]
    public class MotdStartReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="MotdStartReplyMessage" /> class.
        /// </summary>
        public MotdStartReplyMessage()
            : base(375)
        {
        }

        /// <summary>
        ///   Gets or sets the info included in the message.
        /// </summary>
        public string Info
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
            parameters.Add(this.Info);
            return parameters;
        }

        /// <summary>
        ///   Overrides <see cref="IrcMessage.ParseParameters" />
        /// </summary>
        /// <param name="parameters"></param>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Info = parameters[parameters.Count - 1];
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMotdStartReply(new IrcMessageEventArgs<MotdStartReplyMessage>(this));
        }
    }
}
