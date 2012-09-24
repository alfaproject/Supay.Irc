using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The UserNotificationMessage is used at the beginning of connection to specify the username, hostname and real name of a new user.
    /// </summary>
    [Serializable]
    public class UserNotificationMessage : CommandMessage
    {
        public UserNotificationMessage()
        {
            UserName = string.Empty;
            InitialInvisibility = true;
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
        ///   Gets or sets if the client is initialized with a user mode of invisible.
        /// </summary>
        public bool InitialInvisibility
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the client is initialized with a user mode of receiving wallops.
        /// </summary>
        public bool InitialWallops
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
            var p = MessageUtil.GetParameters(unparsedMessage);
            int tempInt;
            return p.Count == 4 && int.TryParse(p[1], out tempInt) && p[2] == "*";
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.UserName);
            int modeBitMask = 0;
            if (this.InitialInvisibility)
            {
                modeBitMask += 8;
            }
            if (this.InitialWallops)
            {
                modeBitMask += 4;
            }
            parameters.Add(modeBitMask.ToString(CultureInfo.InvariantCulture));
            parameters.Add("*");
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
                this.RealName = parameters[3];
                int modeBitMask = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
                this.InitialInvisibility = (modeBitMask & 8) == 8;
                this.InitialWallops = (modeBitMask & 4) == 4;
            }
            else
            {
                this.UserName = string.Empty;
                this.RealName = string.Empty;
                this.InitialInvisibility = true;
                this.InitialWallops = false;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnUserNotification(new IrcMessageEventArgs<UserNotificationMessage>(this));
        }
    }
}
