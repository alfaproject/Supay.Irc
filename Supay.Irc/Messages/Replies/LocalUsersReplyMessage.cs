using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This message indicates the number of local-server users.
    /// </summary>
    [Serializable]
    public class LocalUsersReplyMessage : NumericMessage
    {
        private static readonly Regex localUsersRegex = new Regex(@"Current [lL]ocal [uU]sers: (\d+)  ?Max: (\d+)");

        private const string CURRENT_LOCAL_USERS = "Current local users: ";
        private const string MAX = " Max: ";

        /// <summary>
        ///   Creates a new instance of the <see cref="LocalUsersReplyMessage" /> class.
        /// </summary>
        public LocalUsersReplyMessage()
            : base(265)
        {
            UserCount = -1;
            UserLimit = -1;
        }

        /// <summary>
        ///   Gets or sets the number of local users.
        /// </summary>
        public int UserCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the maximum number of users for the server.
        /// </summary>
        public int UserLimit
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
            parameters.Add(CURRENT_LOCAL_USERS + this.UserCount + MAX + this.UserLimit);
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
                    var localUsersMatch = localUsersRegex.Match(parameters[1]);
                    if (localUsersMatch.Success)
                    {
                        this.UserCount = int.Parse(localUsersMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                        this.UserLimit = int.Parse(localUsersMatch.Groups[2].Value, CultureInfo.InvariantCulture);
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
            conduit.OnLocalUsersReply(new IrcMessageEventArgs<LocalUsersReplyMessage>(this));
        }
    }
}
