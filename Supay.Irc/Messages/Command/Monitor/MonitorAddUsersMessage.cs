using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A Monitor system message that adds users to your monitor list.
    /// </summary>
    [Serializable]
    public class MonitorAddUsersMessage : MonitorMessage
    {
        public MonitorAddUsersMessage()
        {
            Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets the collection of nicks being added to the monitor list.
        /// </summary>
        public ICollection<string> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            return base.CanParse(unparsedMessage)
                && MessageUtil.GetParameters(unparsedMessage).First()[0] == '+';
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            string nicksParam = parameters[parameters.Count - 1];
            var splitNicksParam = nicksParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string nick in splitNicksParam)
            {
                this.Nicks.Add(nick);
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add("+");
            if (this.Nicks != null && this.Nicks.Count != 0)
            {
                parameters.Add(string.Join(",", this.Nicks));
            }
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMonitorAddUsers(new IrcMessageEventArgs<MonitorAddUsersMessage>(this));
        }
    }
}
