using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Possible actions for each mode change in a <see cref="ChannelModeMessage"/>
  ///   or <see cref="UserModeMessage"/> message. </summary>
  [Serializable]
  public sealed class ModeAction : IEquatable<ModeAction> {

    private readonly string _symbol;

    #region Static Instances

    /// <summary>
    ///   Gets the <see cref="ModeAction"/> representing the addition of a mode to a user or channel. </summary>
    public static readonly ModeAction Add = new ModeAction("+");
    /// <summary>
    ///   Gets the <see cref="ModeAction"/> representing the removal of a mode from a user or channel. </summary>
    public static readonly ModeAction Remove = new ModeAction("-");

    /// <summary>
    ///   Gets an array of <see cref="ModeAction"/> instances representing all the possible actions. </summary>
    public static readonly ReadOnlyCollection<ModeAction> Values;

    #endregion

    #region Static Constructor

    static ModeAction() {
      Add = new ModeAction("+");
      Remove = new ModeAction("-");
      Values = new ReadOnlyCollection<ModeAction>(new[] { Add, Remove });
    }

    #endregion

    #region Static Methods

    /// <summary>
    ///   Determines if the given string value is representative of any defined ModeActions. </summary>
    public static bool IsDefined(string value) {
      return Values.Any(modeAction => modeAction._symbol == value);
    }

    /// <summary>
    ///   Returns the correct <see cref="ModeAction"/> for the given string value. </summary>
    /// <param name="value">
    ///   The String to parse. </param>
    public static ModeAction Parse(string value) {
      if (value == null) {
        throw new ArgumentNullException("value");
      }

      foreach (ModeAction modeAction in Values) {
        if (modeAction._symbol == value) {
          return modeAction;
        }
      }
      throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ModeActionDoesNotExist, value), "value");
    }

    #endregion

    #region Constructor

    /// <summary>
    ///   Creates a new instance of the <see cref="ModeAction"/> class. </summary>
    /// <remarks>
    ///   This is private so that only the Enum-like static references can ever be used. </remarks>
    private ModeAction(string symbol) {
      _symbol = symbol;
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Gets a string representing the <see cref="ModeAction"/> in irc format. </summary>
    public override string ToString() {
      return _symbol;
    }

    #endregion

    #region IEquatable<ModeAction> Members

    /// <summary>
    ///   Indicates whether the current object is equal to another object of the same type. </summary>
    /// <returns>
    ///   true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false. </returns>
    /// <param name="other">
    ///   An object to compare with this object. </param>
    public bool Equals(ModeAction other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return _symbol == other._symbol;
    }

    /// <summary>
    ///   Determines whether the specified <see cref="Object"/> is equal to the current <see cref="ModeAction"/>. </summary>
    /// <returns>
    ///   true if the specified <see cref="Object"/> is equal to the current <see cref="ModeAction"/>;
    ///   otherwise, false. </returns>
    /// <param name="obj">
    ///   The <see cref="Object"/> to compare with the current <see cref="ModeAction"/>. </param>
    public override bool Equals(object obj) {
      return Equals(obj as ModeAction);
    }

    /// <summary>
    ///   Serves as a hash function for a particular type. </summary>
    /// <returns>
    ///   A hash code for the current <see cref="ModeAction"/>. </returns>
    public override int GetHashCode() {
      return (_symbol != null ? _symbol.GetHashCode() : 0);
    }

#endregion

    #region Equality Operators

    public static bool operator ==(ModeAction leftOperand, ModeAction rightOperand) {
      if (ReferenceEquals(null, leftOperand)) {
        return ReferenceEquals(null, rightOperand);
      }
      return leftOperand.Equals(rightOperand);
    }

    public static bool operator !=(ModeAction leftOperand, ModeAction rightOperand) {
      return !(leftOperand == rightOperand);
    }

    #endregion

  } //class ModeAction
} //namespace Supay.Irc.Messages