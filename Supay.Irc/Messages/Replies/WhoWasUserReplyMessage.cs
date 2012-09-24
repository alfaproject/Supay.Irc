using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to a <see cref="WhoWasMessage" /> query.
    /// </summary>
    [Serializable]
    public class WhoWasUserReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoWasUserReplyMessage" /> class.
        /// </summary>
        public WhoWasUserReplyMessage()
            : base(314)
        {
            User = new User();
        }

        /// <summary>
        ///   Gets or sets the User being examined.
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
            this.User = new User();
            if (parameters.Count > 5)
            {
                this.User.Nickname = parameters[1];
                this.User.Username = parameters[2];
                this.User.Host = parameters[3];
                this.User.Name = parameters[5];
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoWasUserReply(new IrcMessageEventArgs<WhoWasUserReplyMessage>(this));
        }
    }
}
