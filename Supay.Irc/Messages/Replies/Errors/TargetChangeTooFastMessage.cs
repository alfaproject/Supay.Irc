using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when a user tries to send commands to too many targets in a short amount of time.
    /// </summary>
    /// <remarks>
    ///   The purpose of this error condition is to help stop spammers.
    /// </remarks>
    [Serializable]
    public class TargetChangeTooFastMessage : ErrorMessage, IChannelTargetedMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="TargetChangeTooFastMessage" /> class.
        /// </summary>
        public TargetChangeTooFastMessage()
            : base(439)
        {
        }

        /// <summary>
        ///   Gets or sets the nick or channel which was attempted
        /// </summary>
        public string TargetChanged
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the number of seconds which must be waited before attempting again.
        /// </summary>
        public int Seconds
        {
            get;
            set;
        }


        #region IChannelTargetedMessage Members

        bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
        {
            return this.IsTargetedAtChannel(channelName);
        }

        #endregion


        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.TargetChanged);
            parameters.Add(string.Format(CultureInfo.InvariantCulture, "Target change too fast. Please wait {0} seconds.", this.Seconds));
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.TargetChanged = string.Empty;
            this.Seconds = -1;
            if (parameters.Count > 1)
            {
                this.TargetChanged = parameters[1];
                if (parameters.Count > 2)
                {
                    this.Seconds = Convert.ToInt32(MessageUtil.StringBetweenStrings(parameters[2], "Please wait ", " seconds"), CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnTargetChangeTooFast(new IrcMessageEventArgs<TargetChangeTooFastMessage>(this));
        }

        /// <summary>
        ///   Determines if the the current message is targeted at the given channel.
        /// </summary>
        protected virtual bool IsTargetedAtChannel(string channelName)
        {
            return this.TargetChanged.Equals(channelName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
