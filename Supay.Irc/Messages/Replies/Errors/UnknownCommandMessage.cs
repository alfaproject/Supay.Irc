using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when a command is sent to a server which didn't recognize it.
    /// </summary>
    [Serializable]
    public class UnknownCommandMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="TooManyLinesMessage" /> class.
        /// </summary>
        public UnknownCommandMessage()
            : base(421)
        {
        }

        /// <summary>
        ///   Gets or sets the command which caused the error.
        /// </summary>
        public string Command
        {
            get;
            set;
        }

        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Command);
            parameters.Add("Unknown command");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Command = string.Empty;
            if (parameters.Count > 1)
            {
                this.Command = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUnknownCommand(new IrcMessageEventArgs<UnknownCommandMessage>(this));
        }
    }
}
