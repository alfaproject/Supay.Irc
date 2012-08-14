using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A request that a client plays a local sound.
    /// </summary>
    [Serializable]
    public class SoundRequestMessage : CtcpRequestMessage
    {
        private string soundFile = string.Empty;
        private string text = string.Empty;

        /// <summary>
        ///   Creates a new instance of the <see cref="SoundRequestMessage" /> class.
        /// </summary>
        public SoundRequestMessage()
        {
            this.InternalCommand = "SOUND";
        }

        /// <summary>
        ///   Gets or sets an optional additional test message going along with the request.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        /// <summary>
        ///   Gets or sets the name of the requested sound file to be played.
        /// </summary>
        public virtual string SoundFile
        {
            get
            {
                return this.soundFile;
            }
            set
            {
                this.soundFile = value;
            }
        }

        /// <summary>
        ///   Gets the data payload of the Ctcp request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                return this.SoundFile + " " + this.Text;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSoundRequest(new IrcMessageEventArgs<SoundRequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            string eData = CtcpUtil.GetExtendedData(unparsedMessage);
            if (eData.Length > 0)
            {
                IList<string> p = MessageUtil.GetParameters(eData);
                this.SoundFile = p[0];
                if (p.Count > 1)
                {
                    this.Text = p[1];
                }
            }
        }
    }
}
