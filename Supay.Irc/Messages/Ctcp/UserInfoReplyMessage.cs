using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply for the <see cref="UserInfoRequestMessage" /> query.
    /// </summary>
    [Serializable]
    public class UserInfoReplyMessage : CtcpReplyMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="UserInfoReplyMessage" /> class.
        /// </summary>
        public UserInfoReplyMessage()
        {
            this.InternalCommand = "USERINFO";
            Response = string.Empty;
        }

        /// <summary>
        ///   The information that the client wants to return.
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
            conduit.OnUserInfoReply(new IrcMessageEventArgs<UserInfoReplyMessage>(this));
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
