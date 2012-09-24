namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   A user mode sent in a <see cref="UserModeMessage" /> which is not known.
    /// </summary>
    public class UnknownUserMode : UserMode
    {
        private readonly string _symbol;

        /// <summary>
        ///   Creates a new instance of the <see cref="UnknownUserMode" /> class with the given <see cref="ModeAction" /> and value.
        /// </summary>
        public UnknownUserMode(ModeAction action, string symbol)
            : base(action)
        {
            _symbol = symbol;
        }

        /// <summary>
        ///   Gets the IRC string representation of the mode being changed or applied.
        /// </summary>
        protected override string Symbol
        {
            get
            {
                return _symbol;
            }
        }
    }
}
