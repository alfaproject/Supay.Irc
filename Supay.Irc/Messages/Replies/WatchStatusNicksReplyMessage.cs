using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A reply for the <see cref="WatchListRequestMessage" /> query stating the users on your watch list.
    /// </summary>
    /// <remarks>
    ///   You may receive more than 1 of these in response to the request.
    /// </remarks>
    [Serializable]
    public class WatchStatusNicksReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WatchStatusNicksReplyMessage" />.
        /// </summary>
        public WatchStatusNicksReplyMessage()
            : base(606)
        {
            Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets the collection of nicks of the users on the watch list.
        /// </summary>
        public ICollection<string> Nicks
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(string.Join(" ", this.Nicks));
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            Nicks.Clear();
            foreach (var nick in parameters[parameters.Count - 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Nicks.Add(nick);
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWatchStatusNicksReply(new IrcMessageEventArgs<WatchStatusNicksReplyMessage>(this));
        }
    }
}
