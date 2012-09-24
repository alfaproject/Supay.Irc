using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Requests information about the given user or users.
    /// </summary>
    [Serializable]
    public class WhoMessage : CommandMessage
    {
        public WhoMessage()
        {
            Mask = new User();
        }

        /// <summary>
        ///   Gets or sets the mask which is matched for users to return information about.
        /// </summary>
        public User Mask
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets if the results should only contain IRC operators.
        /// </summary>
        public bool RestrictToOps
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "WHO";
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Mask.ToString());
            if (this.RestrictToOps)
            {
                parameters.Add("o");
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Mask = new User();
            if (parameters.Count >= 1)
            {
                this.Mask.Nickname = parameters[0];
                this.RestrictToOps = parameters.Count > 1 && parameters[1] == "o";
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWho(new IrcMessageEventArgs<WhoMessage>(this));
        }
    }
}
