using System;
using System.Globalization;
using System.Text;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The reply to the <see cref="FingerRequestMessage" />, containing the user's name and idle time.
    /// </summary>
    [Serializable]
    public class FingerReplyMessage : CtcpReplyMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="FingerReplyMessage" /> class.
        /// </summary>
        public FingerReplyMessage()
        {
            this.InternalCommand = "FINGER";
            RealName = string.Empty;
            LoginName = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the real name of the user.
        /// </summary>
        public string RealName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the login name of the user.
        /// </summary>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of seconds that the user has been idle.
        /// </summary>
        public double IdleSeconds
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the data payload of the Ctcp request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                var result = new StringBuilder();
                result.Append(":");
                result.Append(this.RealName);
                result.Append(" (");
                result.Append(this.LoginName);
                result.Append(") - Idle ");
                result.Append(this.IdleSeconds.ToString(CultureInfo.InvariantCulture));
                result.Append(" seconds");
                return result.ToString();
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnFingerReply(new IrcMessageEventArgs<FingerReplyMessage>(this));
        }

        /// <summary>
        ///   Parses the given string to populate this <see cref="IrcMessage" />.
        /// </summary>
        public override void Parse(string unparsedMessage)
        {
            base.Parse(unparsedMessage);
            string payload = CtcpUtil.GetExtendedData(unparsedMessage);
            if (payload.StartsWith(":", StringComparison.Ordinal))
            {
                payload = payload.Substring(1);
            }
            this.RealName = payload.Substring(0, payload.IndexOf(" ", StringComparison.Ordinal));

            int startOfLoginName = payload.IndexOf(" (", StringComparison.Ordinal);
            int endOfLoginName = payload.IndexOf(")", StringComparison.Ordinal);
            if (startOfLoginName > 0)
            {
                startOfLoginName += 2;
                this.LoginName = payload.Substring(startOfLoginName, endOfLoginName - startOfLoginName);

                int startOfIdle = payload.IndexOf("- Idle ", StringComparison.Ordinal);
                if (startOfIdle > 0)
                {
                    startOfIdle += 6;
                    string idleSecs = payload.Substring(startOfIdle, payload.Length - startOfIdle - 8);
                    double foo;
                    if (double.TryParse(idleSecs, NumberStyles.Any, null, out foo))
                    {
                        this.IdleSeconds = foo;
                    }
                    else
                    {
                        this.IdleSeconds = -1;
                    }
                }
            }
        }
    }
}
