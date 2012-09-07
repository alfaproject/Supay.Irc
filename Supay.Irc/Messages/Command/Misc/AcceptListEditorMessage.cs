using System;
using System.Collections.Generic;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A Message that edits the list of users on your accept list.
    /// </summary>
    [Serializable]
    public class AcceptListEditorMessage : CommandMessage
    {
        public AcceptListEditorMessage()
        {
            RemovedNicks = new List<string>();
            AddedNicks = new List<string>();
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "ACCEPT";
            }
        }

        /// <summary>
        ///   Validates this message against the given server support
        /// </summary>
        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            if (serverSupport != null && !serverSupport.CallerId)
            {
                throw new InvalidMessageException(Resources.ServerDoesNotSupportAccept);
            }
        }


        #region Properties

        /// <summary>
        ///   Gets the collection of nicks being added to the accept list.
        /// </summary>
        public ICollection<string> AddedNicks
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the collection of nicks being removed from the accept list.
        /// </summary>
        public ICollection<string> RemovedNicks
        {
            get;
            private set;
        }

        #endregion


        #region Parsing

        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            return base.CanParse(unparsedMessage)
                && MessageUtil.GetParameters(unparsedMessage).First() != "*";
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            foreach (string nick in parameters[0].Split(','))
            {
                if (nick.StartsWith("-", StringComparison.Ordinal))
                {
                    this.RemovedNicks.Add(nick.Substring(1));
                }
                else
                {
                    this.AddedNicks.Add(nick);
                }
            }
        }

        #endregion


        #region Formatting

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            var nicks = RemovedNicks.Select(nick => "-" + nick).Concat(AddedNicks);
            parameters.Add(string.Join(",", nicks));
            return parameters;
        }

        #endregion


        #region Events

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnAcceptListEditor(new IrcMessageEventArgs<AcceptListEditorMessage>(this));
        }

        #endregion
    }
}
