namespace Supay.Irc.Messages.Modes {

  /// <summary>
  /// This mode defines an exception for any <see cref="BanMode"/> masks set for the channel.
  /// </summary>
  public class BanExceptionMode : AccessControlMode {

    /// <summary>
    /// Creates a new instance of the <see cref="BanExceptionMode"/> class.
    /// </summary>
    public BanExceptionMode() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BanExceptionMode"/> class with the given <see cref="ModeAction"/>.
    /// </summary>
    public BanExceptionMode(ModeAction action) {
      this.Action = action;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BanExceptionMode"/> class 
    /// with the given <see cref="ModeAction"/> and <see cref="User"/>.
    /// </summary>
    public BanExceptionMode(ModeAction action, User mask) {
      this.Action = action;
      this.Mask = mask;
    }

    /// <summary>
    /// Gets the IRC string representation of the mode being changed or applied.
    /// </summary>
    protected override string Symbol {
      get {
        return "e";
      }
    }
  }

}