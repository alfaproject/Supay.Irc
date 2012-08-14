using System;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Sends a request for the transfer of the given file.
    /// </summary>
    public class DccGetRequestMessage : CtcpRequestMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="DccGetRequestMessage" /> class.
        /// </summary>
        public DccGetRequestMessage()
        {
            this.Secure = false;
            this.TurboMode = false;
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
                return string.Join(" ", this.DccCommand, this.FileName);
            }
        }

        /// <summary>
        ///   Gets the DCC sub-command.
        /// </summary>
        protected string DccCommand
        {
            get
            {
                string result = "GET";
                if (this.Secure)
                {
                    result = "S" + result;
                }
                if (this.TurboMode)
                {
                    result = "T" + result;
                }
                return result;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the file being requested.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the DCC connection should use turbo mode.
        /// </summary>
        public bool TurboMode
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the DCC connection should use SSL.
        /// </summary>
        public bool Secure
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
            return !string.IsNullOrEmpty(command) && this.DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.FileName = DccUtil.GetArgument(unparsedMessage);
            string unparsedCommand = DccUtil.GetCommand(unparsedMessage).ToUpperInvariant();
            string commandExtenstion = unparsedCommand.Substring(0, unparsedCommand.Length - 3);
            this.TurboMode = commandExtenstion.IndexOf("T", StringComparison.Ordinal) >= 0;
            this.Secure = commandExtenstion.IndexOf("S", StringComparison.Ordinal) >= 0;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
        ///   current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnDccGetRequest(new IrcMessageEventArgs<DccGetRequestMessage>(this));
        }
    }
}
