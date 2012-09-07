using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A client session is ended with a QuitMessage.
    /// </summary>
    /// <remarks>
    ///   The server must close the connection to a client which sends a QuitMessage. If a
    ///   <see cref="QuitMessage.Reason" /> is given, this will be sent instead of the default
    ///   message, the nickname.
    /// </remarks>
    [Serializable]
    public class QuitMessage : CommandMessage
    {
        /// <summary>
        ///   Creates a new instance of the QuitMessage class.
        /// </summary>
        public QuitMessage()
        {
        }

        /// <summary>
        ///   Creates a new instance of the QuitMessage class with the given reason.
        /// </summary>
        public QuitMessage(string reason)
            : this()
        {
            this.Reason = reason;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "QUIT";
            }
        }

        /// <summary>
        ///   Gets or sets the reason for quitting.
        /// </summary>
        public string Reason
        {
            get;
            set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            if (!string.IsNullOrEmpty(this.Reason))
            {
                parameters.Add(this.Reason);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 1)
            {
                this.Reason = parameters[0];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
        ///   current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnQuit(new IrcMessageEventArgs<QuitMessage>(this));
        }
    }
}
