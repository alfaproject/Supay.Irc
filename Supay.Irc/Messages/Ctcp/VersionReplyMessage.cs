using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to a <see cref="VersionRequestMessage" />.
    /// </summary>
    [Serializable]
    public class VersionReplyMessage : CtcpReplyMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="VersionReplyMessage" /> class.
        /// </summary>
        public VersionReplyMessage()
        {
            this.InternalCommand = "VERSION";
            Response = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the version of the client.
        /// </summary>
        public string Response
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the data payload of the Ctcp request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                return this.Response;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnVersionReply(new IrcMessageEventArgs<VersionReplyMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.Response = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
