using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply to a <see cref="WhoIsMessage" /> that contains 
    ///   basic information about the user in question.
    /// </summary>
    [Serializable]
    public class WhoIsUserReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsUserReplyMessage" /> class.
        /// </summary>
        public WhoIsUserReplyMessage()
            : base(311)
        {
            User = new User();
        }

        /// <summary>
        ///   Gets the information about the user in question.
        /// </summary>
        public User User
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
            parameters.Add(this.User.Nickname);
            parameters.Add(this.User.Username);
            parameters.Add(this.User.Host);
            parameters.Add("*");
            parameters.Add(this.User.Name);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 5)
            {
                this.User = new User {
                    Nickname = parameters[1],
                    Username = parameters[2],
                    Host = parameters[3],
                    Name = parameters[5]
                };
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoIsUserReply(new IrcMessageEventArgs<WhoIsUserReplyMessage>(this));
        }
    }
}
