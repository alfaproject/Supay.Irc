using System;

namespace Supay.Irc.Messages.Modes {

  /// <summary>
  /// This mode signifies that the user will receive wallop messages.
  /// </summary>
  public class ReceiveServerNoticesMode : UserMode {

    /// <summary>
    /// Creates a new instance of the <see cref="ReceiveServerNoticesMode"/> class.
    /// </summary>
    public ReceiveServerNoticesMode() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ReceiveServerNoticesMode"/> class with the given <see cref="ModeAction"/>.
    /// </summary>
    public ReceiveServerNoticesMode(ModeAction action) {
      this.Action = action;
    }

    /// <summary>
    /// Gets the irc string representation of the mode being changed or applied.
    /// </summary>
    protected override String Symbol {
      get {
        return "s";
      }
    }

  }

}