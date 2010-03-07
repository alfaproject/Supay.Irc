namespace Supay.Irc.Messages.Modes {

  /// <summary>
  ///   This mode signifies that the user is away. </summary>
  public class AwayMode : UserMode {

    /// <summary>
    ///   Creates a new instance of the <see cref="AwayMode"/> class. </summary>
    public AwayMode() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="AwayMode"/> class with the given <see cref="ModeAction"/>. </summary>
    public AwayMode(ModeAction action)
      : base(action) {
    }

    /// <summary>
    ///   Gets the IRC string representation of the mode being changed or applied. </summary>
    protected override string Symbol {
      get {
        return "a";
      }
    }

  } //class AwayMode
} //namespace Supay.Irc.Messages.Modes