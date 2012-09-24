namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   This mode sets or unsets a password on a channel.
    /// </summary>
    public class KeyMode : FlagMode
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="KeyMode" /> class 
        ///   with the given <see cref="ModeAction" /> and password.
        /// </summary>
        public KeyMode(ModeAction action, string password)
            : base(action)
        {
            Password = password;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="KeyMode" /> class with the given <see cref="ModeAction" />.
        /// </summary>
        public KeyMode(ModeAction action)
            : this(action, string.Empty)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="KeyMode" /> class 
        ///   with the given password.
        /// </summary>
        public KeyMode(string password)
            : this(ModeAction.Add, password)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="KeyMode" /> class.
        /// </summary>
        public KeyMode()
            : this(ModeAction.Add, string.Empty)
        {
        }

        /// <summary>
        ///   Gets the IRC string representation of the mode being changed or applied.
        /// </summary>
        protected override string Symbol
        {
            get
            {
                return "k";
            }
        }

        /// <summary>
        ///   Gets or sets the password needed to gain access to a channel.
        /// </summary>
        public string Password
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
            if (Password.Length != 0)
            {
                msg.ModeArguments.Add(Password);
            }
        }
    }
}
