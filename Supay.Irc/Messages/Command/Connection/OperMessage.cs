using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   OperMessage is used by a normal user to obtain IRC operator privileges.
    ///   ( This does not refer to channel ops )
    ///   The correct combination of <see cref="Name" /> and <see cref="Password" /> are required to gain Operator privileges.
    /// </summary>
    [Serializable]
    public class OperMessage : CommandMessage
    {
        /// <summary>
        ///   Creates a new instance of the OperMessage class with the given name and password.
        /// </summary>
        public OperMessage(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        /// <summary>
        ///   Creates a new instance of the OperMessage class.
        /// </summary>
        public OperMessage()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "OPER";
            }
        }

        /// <summary>
        ///   Gets or sets the password for the sender.
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name for the sender.
        /// </summary>
        public string Name
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
            parameters.Add(this.Name);
            parameters.Add(this.Password);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 2)
            {
                this.Name = parameters[0];
                this.Password = parameters[1];
            }
            else
            {
                this.Name = string.Empty;
                this.Password = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnOper(new IrcMessageEventArgs<OperMessage>(this));
        }
    }
}
