using System;
using System.Globalization;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message is a request to resume sending a file previously, but not completely sent to the
    ///   requester.
    /// </summary>
    [Serializable]
    public class DccResumeRequestMessage : CtcpRequestMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="DccResumeRequestMessage" /> class.
        /// </summary>
        public DccResumeRequestMessage()
        {
            this.Position = -1;
            this.Port = -1;
            this.FileName = string.Empty;
            this.InternalCommand = "DCC";
        }

        /// <summary>
        ///   Gets the data payload of the CTCP request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                return string.Join(" ", DccCommand, this.FileName, this.Port.ToString(CultureInfo.InvariantCulture), this.Position.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        ///   Gets the DCC sub-command.
        /// </summary>
        protected static string DccCommand
        {
            get
            {
                return "RESUME";
            }
        }

        /// <summary>
        ///   Gets or sets the name of the file being sent.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the port the connection should be on.
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the position in the file at which to resume sending.
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            return base.CanParse(unparsedMessage) && this.CanParseDccCommand(DccUtil.GetCommand(unparsedMessage));
        }

        /// <summary>
        ///   Determines if the message's DCC command is compatible with this message.
        /// </summary>
        public bool CanParseDccCommand(string command)
        {
            return command != null && DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.FileName = DccUtil.GetArgument(unparsedMessage);
            var p = DccUtil.GetParameters(unparsedMessage);
            this.Port = Convert.ToInt32(p[2], CultureInfo.InvariantCulture);
            this.Position = Convert.ToInt32(p[3], CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
        ///   current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnDccResumeRequest(new IrcMessageEventArgs<DccResumeRequestMessage>(this));
        }
    }
}
