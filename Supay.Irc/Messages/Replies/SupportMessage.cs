using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   This is a message sent from a server to a client upon connection 
    ///   to tell the client what IRC features the server supports.
    /// </summary>
    [Serializable]
    public class SupportMessage : NumericMessage
    {
        private const string ARE_SUPPORTED = "are supported by this server";
        private readonly NameValueCollection supportedItems = new NameValueCollection();

        /// <summary>
        ///   Creates a new instance of the <see cref="SupportMessage" /> class.
        /// </summary>
        public SupportMessage()
            : base(005)
        {
        }

        /// <summary>
        ///   Gets the list of items supported by the server.
        /// </summary>
        public virtual NameValueCollection SupportedItems
        {
            get
            {
                return this.supportedItems;
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.Tokens"/>.
        /// </summary>
        protected override IList<string> Tokens
        {
            get
            {
                var paramsToString = new Collection<string>();
                foreach (string name in this.SupportedItems.Keys)
                {
                    string value = this.SupportedItems[name];
                    if (value.Length != 0)
                    {
                        paramsToString.Add(name + "=" + this.SupportedItems[name]);
                    }
                    else
                    {
                        paramsToString.Add(name);
                    }
                }

                var parameters = base.Tokens;
                parameters.Add(string.Join(" ", paramsToString));
                parameters.Add(ARE_SUPPORTED);
                return parameters;
            }
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            for (int i = 1; i < parameters.Count - 1; i++)
            {
                string nameValue = parameters[i];
                string name;
                string value;
                int indexOfEquals = nameValue.IndexOf("=", StringComparison.Ordinal);
                if (indexOfEquals > 0)
                {
                    name = nameValue.Substring(0, indexOfEquals);
                    value = nameValue.Substring(indexOfEquals + 1);
                }
                else
                {
                    name = nameValue;
                    value = string.Empty;
                }
                this.SupportedItems[name] = value;
            }
        }

        /// <summary>
        ///   Determines if the message can be parsed by this type.
        /// </summary>
        public override bool CanParse(string unparsedMessage)
        {
            if (unparsedMessage == null)
            {
                return false;
            }
            return base.CanParse(unparsedMessage) && unparsedMessage.IndexOf(ARE_SUPPORTED, StringComparison.Ordinal) > 0;
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSupport(new IrcMessageEventArgs<SupportMessage>(this));
        }
    }
}
