using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A client-to-client ping request message.
    /// </summary>
    [Serializable]
    public class PingRequestMessage : CtcpRequestMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="PingRequestMessage" /> class.
        /// </summary>
        public PingRequestMessage()
        {
            this.InternalCommand = "PING";
            TimeStamp = string.Empty;
        }

        /// <summary>
        ///   The custom timestamp to send in the ping request.
        /// </summary>
        /// <remarks>
        ///   The ping reply should have this same exact timestamp,
        ///   so you could subtract the original timestamp with the
        ///   current one to determine the lag time.
        /// </remarks>
        public string TimeStamp
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
                return TimeStamp;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnPingRequest(new IrcMessageEventArgs<PingRequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            TimeStamp = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
