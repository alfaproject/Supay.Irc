using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Returned by a server to a client when it detects a nickname collision.
    /// </summary>
    [Serializable]
    public class NickCollisionMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="NickCollisionMessage" /> class.
        /// </summary>
        public NickCollisionMessage()
            : base(436)
        {
            Nick = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick which was already taken.
        /// </summary>
        public string Nick
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
            parameters.Add(this.Nick);
            parameters.Add("Nickname collision KILL");
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Nick = parameters.Count > 1 ? parameters[1] : string.Empty;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnNickCollision(new IrcMessageEventArgs<NickCollisionMessage>(this));
        }
    }
}
