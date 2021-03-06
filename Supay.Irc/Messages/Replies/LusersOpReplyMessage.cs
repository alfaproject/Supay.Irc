using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   One of the responses to the <see cref="LusersMessage" /> query.
    /// </summary>
    [Serializable]
    public class LusersOpReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="LusersOpReplyMessage" /> class
        /// </summary>
        public LusersOpReplyMessage()
            : base(252)
        {
            OpCount = -1;
            Info = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the number of IRC operators connected to the server.
        /// </summary>
        public int OpCount
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets any additional information about the operators connected.
        /// </summary>
        public string Info
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
            parameters.Add(this.OpCount.ToString(CultureInfo.InvariantCulture));
            parameters.Add(this.Info);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            if (parameters.Count > 2)
            {
                this.OpCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
                this.Info = parameters[2];
            }
            else
            {
                this.OpCount = -1;
                this.Info = string.Empty;
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnLusersOpReply(new IrcMessageEventArgs<LusersOpReplyMessage>(this));
        }
    }
}
