using System;

namespace Supay.Irc.Messages.Modes
{
  /// <summary>
  ///   The list of known user modes sent in a <see cref="UserModeMessage" /> in its
  ///   <see cref="UserModeMessage.ModeChanges" /> property.
  /// </summary>
  public abstract class UserMode
  {
    protected UserMode()
      : this(ModeAction.Add)
    {
    }

    protected UserMode(ModeAction action)
    {
      this.Action = action;
    }

    /// <summary>
    ///   Gets the IRC string representation of the mode being changed or applied.
    /// </summary>
    protected abstract string Symbol
    {
      get;
    }

    /// <summary>
    ///   Gets or sets the <see cref="ModeAction" /> applied.
    /// </summary>
    public ModeAction Action
    {
      get;
      set;
    }

    /// <summary>
    ///   A string representation of the mode.
    /// </summary>
    public override string ToString()
    {
      return (this.Action == ModeAction.Add ? "+" : "-") + this.Symbol;
    }

    /// <summary>
    ///   Applies the mode to the given <see cref="UserModeMessage" />.
    /// </summary>
    /// <param name="msg">The message which will be modified to include this mode.</param>
    /// <param name="includeAction">Specifies if the action modifier should be applied.</param>
    public void ApplyTo(UserModeMessage msg, bool includeAction)
    {
      if (msg == null)
      {
        throw new ArgumentNullException("msg");
      }

      if (includeAction)
      {
        msg.ModeChanges += this.Action == ModeAction.Add ? "+" : "-";
      }
      msg.ModeChanges += this.Symbol;
    }
  }
}
