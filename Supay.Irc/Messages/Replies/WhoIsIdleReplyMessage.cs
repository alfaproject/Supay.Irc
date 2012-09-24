using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   As a reply to the <see cref="WhoIsMessage" /> message,
    ///   carries information about idle time and such.
    /// </summary>
    [Serializable]
    public class WhoIsIdleReplyMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="WhoIsIdleReplyMessage" /> class.
        /// </summary>
        public WhoIsIdleReplyMessage()
            : base(317)
        {
            Nick = string.Empty;
            SignOnTime = DateTime.Now;
            Info = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the nick of the user who is being examined.
        /// </summary>
        public string Nick
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of seconds the user has been idle.
        /// </summary>
        public int IdleLength
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the time the user signed on to their current server.
        /// </summary>
        public DateTime SignOnTime
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets some additional info about the user being examined.
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
            parameters.Add(this.Nick);
            parameters.Add(this.IdleLength.ToString(CultureInfo.InvariantCulture));
            parameters.Add(MessageUtil.ConvertToUnixTime(this.SignOnTime).ToString(CultureInfo.InvariantCulture));
            parameters.Add(this.Info);
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Nick = string.Empty;
            this.IdleLength = 0;
            this.SignOnTime = DateTime.Now;
            this.Info = string.Empty;

            if (parameters.Count > 2)
            {
                this.Nick = parameters[1];
                this.IdleLength = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);

                if (parameters.Count == 5)
                {
                    this.SignOnTime = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture));
                    this.Info = parameters[4];
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnWhoIsIdleReply(new IrcMessageEventArgs<WhoIsIdleReplyMessage>(this));
        }
    }
}
