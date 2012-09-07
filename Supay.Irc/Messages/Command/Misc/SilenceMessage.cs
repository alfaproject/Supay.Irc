using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   With the SilenceMessage, clients can tell a server to never send messages to them from a given user. This, effectively, is a server-side ignore command.
    /// </summary>
    [Serializable]
    public class SilenceMessage : CommandMessage
    {
        private ModeAction action = ModeAction.Add;
        private User silencedUser = new User();

        /// <summary>
        ///   Creates a new instance of the SilenceMessage class.
        /// </summary>
        public SilenceMessage()
        {
        }

        /// <summary>
        ///   Creates a new instance of the SilenceMessage class with the <see cref="User" />.
        /// </summary>
        public SilenceMessage(User silencedUser)
        {
            this.silencedUser = silencedUser;
        }

        /// <summary>
        ///   Creates a new instance of the SilenceMessage class with the given mask.
        /// </summary>
        public SilenceMessage(string userMask)
            : this(new User(userMask))
        {
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "SILENCE";
            }
        }

        /// <summary>
        ///   Gets or sets the user being silenced.
        /// </summary>
        public virtual User SilencedUser
        {
            get
            {
                return this.silencedUser;
            }
            set
            {
                this.silencedUser = value;
            }
        }

        /// <summary>
        ///   Gets or sets the action being applied to the silenced user on the list.
        /// </summary>
        public virtual ModeAction Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            // SILENCE [{{+|-}<user>@<host>}]
            var parameters = base.GetTokens();
            if (this.SilencedUser != null && !string.IsNullOrEmpty(this.SilencedUser.ToString()))
            {
                parameters.Add((this.Action == ModeAction.Add ? "+" : "-") + this.SilencedUser);
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 0)
            {
                string target = parameters[0];
                switch (target[0])
                {
                    case '+':
                        this.Action = ModeAction.Add;
                        target = target.Substring(1);
                        break;
                    case '-':
                        this.Action = ModeAction.Remove;
                        target = target.Substring(1);
                        break;
                    default:
                        this.Action = ModeAction.Add;
                        break;
                }
                this.SilencedUser = new User(target);
            }
            else
            {
                this.SilencedUser = new User();
                this.Action = ModeAction.Add;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSilence(new IrcMessageEventArgs<SilenceMessage>(this));
        }
    }
}
