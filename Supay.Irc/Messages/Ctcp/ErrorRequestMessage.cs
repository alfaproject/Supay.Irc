using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   I see no real need for this message... but it should generate an <see cref="ErrorReplyMessage" /> from the target.
    /// </summary>
    [Serializable]
    public class ErrorRequestMessage : CtcpRequestMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ErrorRequestMessage" /> class.
        /// </summary>
        public ErrorRequestMessage()
        {
            this.InternalCommand = "ERRMSG";
            Query = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the string to be parroted back to you, with an indication that no error occured.
        /// </summary>
        public string Query
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
                return this.Query;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnErrorRequest(new IrcMessageEventArgs<ErrorRequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.Query = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
