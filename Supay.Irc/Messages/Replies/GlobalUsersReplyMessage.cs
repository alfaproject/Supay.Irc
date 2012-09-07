using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message indicates the number of network-wide users.
    /// </summary>
    [Serializable]
    public class GlobalUsersReplyMessage : NumericMessage
    {
        private static readonly Regex globalUsersRegex = new Regex(@"Current [gG]lobal [uU]sers: (\d+)  ?Max: (\d+)");

        private const string CURRENT_GLOBAL_USERS = "Current global users: ";
        private const string MAX = " Max: ";

        private int userCount = -1;
        private int userLimit = -1;

        /// <summary>
        ///   Creates a new instance of the <see cref="GlobalUsersReplyMessage" /> class.
        /// </summary>
        public GlobalUsersReplyMessage()
            : base(266)
        {
        }

        /// <summary>
        ///   Gets or sets the number of global users.
        /// </summary>
        public virtual int UserCount
        {
            get
            {
                return this.userCount;
            }
            set
            {
                this.userCount = value;
            }
        }

        /// <summary>
        ///   Gets or sets the maximum number of users for the network.
        /// </summary>
        public virtual int UserLimit
        {
            get
            {
                return this.userLimit;
            }
            set
            {
                this.userLimit = value;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            // we only write the official version of this message, although other versions exist,
            // thus the message may not be the same raw as parsed.
            var parameters = base.GetTokens();
            parameters.Add(CURRENT_GLOBAL_USERS + this.UserCount + MAX + this.UserLimit);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            switch (parameters.Count)
            {
                case 2:
                    var globalUsersMatch = globalUsersRegex.Match(parameters[1]);
                    if (globalUsersMatch.Success)
                    {
                        this.UserCount = int.Parse(globalUsersMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                        this.UserLimit = int.Parse(globalUsersMatch.Groups[2].Value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        this.UserCount = -1;
                        this.UserLimit = -1;
                    }
                    break;
                case 4:
                    this.UserCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
                    this.UserLimit = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
                    break;
                default:
                    this.UserCount = -1;
                    this.UserLimit = -1;
                    break;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnGlobalUsersReply(new IrcMessageEventArgs<GlobalUsersReplyMessage>(this));
        }
    }
}
