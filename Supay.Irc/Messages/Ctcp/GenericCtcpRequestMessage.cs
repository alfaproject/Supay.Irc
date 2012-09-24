using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   An unknown <see cref="CtcpRequestMessage" />.
    /// </summary>
    [Serializable]
    public class GenericCtcpRequestMessage : CtcpRequestMessage
    {
        public GenericCtcpRequestMessage()
        {
            DataPackage = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the information packaged with the ctcp command.
        /// </summary>
        public string DataPackage
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
                return this.DataPackage;
            }
        }

        /// <summary>
        ///   Gets or sets the Ctcp command.
        /// </summary>
        public virtual string Command
        {
            get
            {
                return this.InternalCommand;
            }
            set
            {
                this.InternalCommand = value;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnGenericCtcpRequest(new IrcMessageEventArgs<GenericCtcpRequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.Command = CtcpUtil.GetInternalCommand(unparsedMessage);
            this.DataPackage = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
