namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   A channel mode sent in a <see cref="ChannelModeMessage" /> which is not known.
    /// </summary>
    public class UnknownChannelMode : ChannelMode
    {
        private readonly string _symbol;

        /// <summary>
        ///   Creates a new instance of the <see cref="UnknownChannelMode" /> class with the given <see cref="ModeAction" /> and value.
        /// </summary>
        public UnknownChannelMode(ModeAction action, string symbol)
        {
            Parameter = string.Empty;
            this.Action = action;
            this._symbol = symbol;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="UnknownChannelMode" /> class with the given <see cref="ModeAction" />, value, and parameter.
        /// </summary>
        public UnknownChannelMode(ModeAction action, string symbol, string parameter)
        {
            this.Action = action;
            this._symbol = symbol;
            this.Parameter = parameter;
        }

        /// <summary>
        ///   Gets or sets the parameter passed with this mode.
        /// </summary>
        public string Parameter
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC string representation of the mode being changed or applied.
        /// </summary>
        protected override string Symbol
        {
            get
            {
                return this._symbol;
            }
        }

        /// <summary>
        ///   Applies this mode to the ModeArguments property of the given <see cref="ChannelModeMessage" />.
        /// </summary>
        /// <param name="msg">The message which will be modified to include this mode.</param>
        protected override void AddParameter(ChannelModeMessage msg)
        {
            if (this.Parameter.Length != 0)
            {
                msg.ModeArguments.Add(this.Parameter);
            }
        }
    }
}
