using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when a command is sent which doesn't contain all the required parameters
    /// </summary>
    [Serializable]
    public class NotEnoughParametersMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="NotEnoughParametersMessage" /> class.
        /// </summary>
        public NotEnoughParametersMessage()
            : base(461)
        {
        }

        /// <summary>
        ///   Gets or sets the command sent
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
            parameters.Add("Not enough parameters");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Command = string.Empty;
            if (parameters.Count > 2)
            {
                this.Command = parameters[1];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnNotEnoughParameters(new IrcMessageEventArgs<NotEnoughParametersMessage>(this));
        }
    }
}
