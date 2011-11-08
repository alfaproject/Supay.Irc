namespace Supay.Irc.Messages.Modes {
  /// <summary>
  ///   A channel mode sent in a <see cref="ChannelModeMessage" /> in its
  ///   <see cref="ChannelModeMessage.ModeChanges" /> property.
  /// </summary>
  public abstract class ChannelMode {
    protected ChannelMode()
      : this(ModeAction.Add) {
    }

    protected ChannelMode(ModeAction action) {
      Action = action;
    }

    /// <summary>
    ///   Gets the IRC string representation of the mode being changed or applied.
    /// </summary>
    protected abstract string Symbol {
      get;
    }

    /// <summary>
    ///   Gets or sets the <see cref="ModeAction" /> applied.
    /// </summary>
    public ModeAction Action {
      get;
      set;
    }

    /// <summary>
    ///   A string representation of the mode.
    /// </summary>
    public override string ToString() {
      return (Action == ModeAction.Add ? "+" : "-") + Symbol;
    }

    /// <summary>
    ///   Applies the mode to the given <see cref="ChannelModeMessage" />.
    /// </summary>
    /// <param name="msg">The message which will be modified to include this mode.</param>
    /// <param name="includeAction">Specifies if the action modifier should be applied.</param>
    public void ApplyTo(ChannelModeMessage msg, bool includeAction) {
      AddChanges(msg, includeAction);
      AddParameter(msg);
    }

    /// <summary>
    ///   Applies this mode to the ModeChanges property of the given
    ///   <see cref="ChannelModeMessage" />.
    /// </summary>
    /// <param name="msg">The message which will be modified to include this mode.</param>
    /// <param name="includeAction">Specifies if the action modifier should be applied.</param>
    protected void AddChanges(ChannelModeMessage msg, bool includeAction) {
      if (includeAction) {
        msg.ModeChanges += Action == ModeAction.Add ? "+" : "-";
      }
      msg.ModeChanges += Symbol;
    }

    /// <summary>
    ///   Applies this mode to the ModeArguments property of the given
    ///   <see cref="ChannelModeMessage" />.
    /// </summary>
    /// <param name="msg">The message which will be modified to include this mode.</param>
    protected virtual void AddParameter(ChannelModeMessage msg) {
    }
  }
}
