using System;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is much like the <see cref="ChatMessage" /> message, 
    ///   except the intent is to describe an "action" that the sender is doing.
    /// </summary>
    [Serializable]
    public class ActionRequestMessage : CtcpRequestMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class with the given text and target.
        /// </summary>
        /// <param name="text">The text of the action.</param>
        /// <param name="target">The target of the action.</param>
        public ActionRequestMessage(string text, string target)
        {
            this.InternalCommand = "ACTION";
            Text = text;
            this.Target = target;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class with the given text.
        /// </summary>
        /// <param name="text">The text of the action.</param>
        public ActionRequestMessage(string text)
            : this(text, string.Empty)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class.
        /// </summary>
        public ActionRequestMessage()
            : this(string.Empty)
        {
        }

        /// <summary>
        ///   Gets or sets the communicated text of this <see cref="ActionRequestMessage" />.s
        /// </summary>
        public string Text
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
                return this.Text;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnActionRequest(new IrcMessageEventArgs<ActionRequestMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            this.Text = CtcpUtil.GetExtendedData(unparsedMessage);
        }
    }
}
