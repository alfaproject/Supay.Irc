namespace Supay.Irc.Messages.Modes {
  /// <summary>
  ///   This mode is only available on channels which name begins with the character '!' 
  ///   and may only be toggled by the "channel creator".
  /// </summary>
  public class ServerReopMode : FlagMode {
    /// <summary>
    ///   Creates a new instance of the <see cref="ServerReopMode" /> class.
    /// </summary>
    public ServerReopMode() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ServerReopMode" /> class with the given <see cref="ModeAction" />.
    /// </summary>
    public ServerReopMode(ModeAction action) {
      Action = action;
    }

    /// <summary>
    ///   Gets the IRC string representation of the mode being changed or applied.
    /// </summary>
    protected override string Symbol {
      get {
        return "r";
      }
    }
  }
}
