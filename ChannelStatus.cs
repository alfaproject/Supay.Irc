using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc {

  /// <summary>
  ///   The nick prefixes that represent user level status in a channel. </summary>
  [Serializable]
  public sealed class ChannelStatus : IEquatable<ChannelStatus> {

    #region Enumeration values

    /// <summary>
    ///   Gets the <see cref="ChannelStatus"/> representing the operator status level. </summary>
    public static readonly ChannelStatus Operator;

    /// <summary>
    ///   Gets the <see cref="ChannelStatus"/> representing the half-operator status level. </summary>
    public static readonly ChannelStatus HalfOperator;

    /// <summary>
    ///   Gets the <see cref="ChannelStatus"/> representing the voiced status level. </summary>
    public static readonly ChannelStatus Voice;
    
    /// <summary>
    ///   Gets the <see cref="ChannelStatus"/> representing the no special status level. </summary>
    public static readonly ChannelStatus None;

    /// <summary>
    ///   Gets a collection of <see cref="ChannelStatus"/> instances representing all built statuses. </summary>
    public static readonly List<ChannelStatus> Values;

    #endregion

    #region Static Constructor

    static ChannelStatus() {
      None = new ChannelStatus("");
      Voice = new ChannelStatus("+");
      HalfOperator = new ChannelStatus("%");
      Operator = new ChannelStatus("@");
      Values = new List<ChannelStatus>(new[] { None, Voice, HalfOperator, Operator });
    }

    #endregion

    #region Static Methods

    /// <summary>
    ///   Gets a ChannelStatus instance with the given symbol. </summary>
    /// <remarks>
    ///   If the given status is not defined already, a new status is created.
    ///   This same new status is used for all future calls to GetInstance. </remarks>
    public static ChannelStatus GetInstance(string symbol) {
      foreach (ChannelStatus channelStatus in Values) {
        if (channelStatus.Symbol == symbol) {
          return channelStatus;
        }
      }

      ChannelStatus newChannelStatus = new ChannelStatus(symbol);
      Values.Add(newChannelStatus);
      return newChannelStatus;
    }

    /// <summary>
    ///   Determines if the given symbol is any of the known channel statuses. </summary>
    public static bool IsDefined(string symbol) {
      return Values.Any(channelStatus => channelStatus.Symbol == symbol);
    }

    #endregion

    #region Constructor

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelStatus"/> class. </summary>
    private ChannelStatus(string symbol) {
      Symbol = symbol;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the string representation of the status. </summary>
    public string Symbol {
      get;
      private set;
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Creates a representation of the message in irc format. </summary>
    public override string ToString() {
      return Symbol;
    }

    #endregion

    #region Equality Members

    /// <summary>
    ///   Indicates whether the current object is equal to another object of the same type. </summary>
    /// <returns>
    ///   true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false. </returns>
    /// <param name="other">
    ///   An object to compare with this object. </param>
    public bool Equals(ChannelStatus other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return Symbol == other.Symbol;
    }

    /// <summary>
    ///   Determines whether the specified <see cref="Object"/> is equal to the current <see cref="ChannelStatus"/>. </summary>
    /// <returns>
    ///   true if the specified <see cref="Object"/> is equal to the current <see cref="ChannelStatus"/>;
    ///   otherwise, false. </returns>
    /// <param name="obj">
    ///   The <see cref="Object"/> to compare with the current <see cref="ChannelStatus"/>. </param>
    public override bool Equals(object obj) {
      return Equals(obj as ChannelStatus);
    }

    /// <summary>
    ///   Serves as a hash function for a particular type. </summary>
    /// <returns>
    ///   A hash code for the current <see cref="ChannelStatus"/>. </returns>
    public override int GetHashCode() {
      return (Symbol != null ? Symbol.GetHashCode() : 0);
    }

    public static bool operator ==(ChannelStatus leftOperand, ChannelStatus rightOperand) {
      if (ReferenceEquals(null, leftOperand)) {
        return ReferenceEquals(null, rightOperand);
      }
      return leftOperand.Equals(rightOperand);
    }

    public static bool operator !=(ChannelStatus leftOperand, ChannelStatus rightOperand) {
      return !(leftOperand == rightOperand);
    }

    #endregion

  } //class ChannelStatus
} //namespace Supay.Irc