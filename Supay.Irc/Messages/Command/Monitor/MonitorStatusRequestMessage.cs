using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A Monitor system message that requests that status of the users on your monitor list.
    /// </summary>
    [Serializable]
    public class MonitorStatusRequestMessage : MonitorMessage
    {
        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            if (!base.CanParse(unparsedMessage))
            {
                return false;
            }
            IList<string> param = MessageUtil.GetParameters(unparsedMessage);
            return param.Count == 1 && param[0].Equals("S", StringComparison.Ordinal);
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add("S");
            return parameters;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMonitorStatusRequest(new IrcMessageEventArgs<MonitorStatusRequestMessage>(this));
        }
    }
}
