using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   An SPR Jukebox message that notifies the recipient of the senders available mp3 file.
    /// </summary>
    [Serializable]
    public class Mp3RequestMessage : CtcpRequestMessage
    {
        private string filename;

        /// <summary>
        ///   Creates a new instance of the <see cref="Mp3RequestMessage" /> class.
        /// </summary>
        public Mp3RequestMessage()
        {
            this.InternalCommand = "MP3";
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class with the given text and target.
        /// </summary>
        /// <param name="target">The target of the action.</param>
        public Mp3RequestMessage(string target)
            : this()
        {
            this.Target = target;
        }

        /// <summary>
        ///   Gets or sets the file name of the mp3 being shared.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.filename;
            }
            set
            {
                this.filename = value;
            }
        }

        /// <summary>
        ///   Gets the data payload of the CTCP request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                return this.FileName;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMp3Request(new IrcMessageEventArgs<Mp3RequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.FileName = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
