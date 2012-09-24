using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to the <see cref="TimeRequestMessage" /> query.
    /// </summary>
    [Serializable]
    public class TimeReplyMessage : CtcpReplyMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="TimeReplyMessage" /> class.
        /// </summary>
        public TimeReplyMessage()
        {
            this.InternalCommand = "TIME";
            CurrentTime = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the time, sent in any format the client finds useful.
        /// </summary>
        public string CurrentTime
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
                return this.CurrentTime;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnTimeReply(new IrcMessageEventArgs<TimeReplyMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.CurrentTime = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
