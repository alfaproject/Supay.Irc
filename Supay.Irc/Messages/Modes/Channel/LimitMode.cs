using System.Globalization;

namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   A user limit may be set on channels by using this mode.
    /// </summary>
    public class LimitMode : FlagMode
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="LimitMode" /> class 
        ///   with the given <see cref="ModeAction" /> and user limit.
        /// </summary>
        public LimitMode(ModeAction action, int userLimit)
            : base(action)
        {
            UserLimit = userLimit;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="LimitMode" /> class with the given <see cref="ModeAction" />.
        /// </summary>
        public LimitMode(ModeAction action)
            : this(action, -1)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="LimitMode" /> class 
        ///   with the given user limit.
        /// </summary>
        public LimitMode(int userLimit)
            : this(ModeAction.Add, userLimit)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="LimitMode" /> class.
        /// </summary>
        public LimitMode()
            : this(ModeAction.Add, -1)
        {
        }

        /// <summary>
        ///   Gets the IRC string representation of the mode being changed or applied.
        /// </summary>
        protected override string Symbol
        {
            get
            {
                return "l";
            }
        }

        /// <summary>
        ///   Gets or sets the maximum number of users allowed to join the channel.
        /// </summary>
        public int UserLimit
        {
            get;
            set;
        }

        /// <summary>
        ///   Applies this mode to the ModeArguments property of the given <see cref="ChannelModeMessage" />.
        /// </summary>
        /// <param name="msg">The message which will be modified to include this mode.</param>
        protected override void AddParameter(ChannelModeMessage msg)
        {
            if (UserLimit != -1)
            {
                msg.ModeArguments.Add(UserLimit.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
