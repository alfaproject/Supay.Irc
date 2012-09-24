namespace Supay.Irc.Messages.Modes
{
    /// <summary>
    ///   The modes in this category are used to define properties which affects how channels operate.
    /// </summary>
    public abstract class FlagMode : ChannelMode
    {
        protected FlagMode(ModeAction action)
            : base(action)
        {
        }

        protected FlagMode()
        {
        }
    }
}
