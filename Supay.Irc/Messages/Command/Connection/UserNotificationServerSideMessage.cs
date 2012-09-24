using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The UserNotificationServerSideMessage is passed between servers to notify of a new user on the network.
    /// </summary>
    [Serializable]
    public class UserNotificationServerSideMessage : CommandMessage
    {
        public UserNotificationServerSideMessage()
        {
            UserName = string.Empty;
            RealName = string.Empty;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "USER";
            }
        }

        /// <summary>
        ///   Gets or sets the UserName of client.
        /// </summary>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the user's host.
        /// </summary>
        public string HostName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the name of the server which the user is on.
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the real name of the client.
        /// </summary>
        public string RealName
        {
            get;
            set;
        }

        public override bool CanParse(string unparsedMessage)
        {
            if (!base.CanParse(unparsedMessage))
            {
                return false;
            }
            IList<string> p = MessageUtil.GetParameters(unparsedMessage);
            return p.Count == 4 && p[2] != "*";
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.UserName);
            parameters.Add(this.HostName);
            parameters.Add(this.ServerName);
            parameters.Add(this.RealName);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count >= 4)
            {
                this.UserName = parameters[0];
                this.HostName = parameters[1];
                this.ServerName = parameters[2];
                this.RealName = parameters[3];
            }
            else
            {
                this.UserName = string.Empty;
                this.HostName = string.Empty;
                this.ServerName = string.Empty;
                this.RealName = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUserNotificationServerSide(new IrcMessageEventArgs<UserNotificationServerSideMessage>(this));
        }
    }
}
