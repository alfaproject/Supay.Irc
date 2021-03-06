using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Supay.Irc
{
    /// <summary>
    ///   The nick prefixes that represent user level status in a channel.
    /// </summary>
    [Serializable]
    public sealed class ChannelStatus : IEquatable<ChannelStatus>
    {
        #region Enumeration values

        /// <summary>
        ///   Gets the <see cref="ChannelStatus" /> representing the operator status level.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "ChannelStatus.Operator is Immutable.")]
        public static readonly ChannelStatus Operator = new ChannelStatus("@");

        /// <summary>
        ///   Gets the <see cref="ChannelStatus" /> representing the half-operator status level.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "ChannelStatus.HalfOperator is Immutable.")]
        public static readonly ChannelStatus HalfOperator = new ChannelStatus("%");

        /// <summary>
        ///   Gets the <see cref="ChannelStatus" /> representing the voiced status level.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "ChannelStatus.Voice is Immutable.")]
        public static readonly ChannelStatus Voice = new ChannelStatus("+");

        /// <summary>
        ///   Gets the <see cref="ChannelStatus" /> representing the no special status level.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "ChannelStatus.None is Immutable.")]
        public static readonly ChannelStatus None = new ChannelStatus(string.Empty);

        /// <summary>
        ///   Gets a collection of <see cref="ChannelStatus" /> instances representing all built
        ///   statuses.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "ChannelStatus.Values is Immutable.")]
        public static readonly ICollection<ChannelStatus> Values = new List<ChannelStatus> {
            None,
            Voice,
            HalfOperator,
            Operator
        };

        #endregion


        #region Static Methods

        /// <summary>
        ///   Gets a <see cref="ChannelStatus" /> instance with the given symbol.
        /// </summary>
        /// <remarks>
        ///   If the given status is not defined already, a new status is created. This same new status
        ///   is used for all future calls to GetInstance.
        /// </remarks>
        public static ChannelStatus GetInstance(string symbol)
        {
            foreach (var channelStatus in Values.Where(channelStatus => channelStatus.Symbol == symbol))
            {
                return channelStatus;
            }

            var newChannelStatus = new ChannelStatus(symbol);
            Values.Add(newChannelStatus);
            return newChannelStatus;
        }

        /// <summary>
        ///   Determines if the given symbol is any of the known channel statuses.
        /// </summary>
        public static bool IsDefined(string symbol)
        {
            return Values.Any(channelStatus => channelStatus.Symbol == symbol);
        }

        #endregion


        #region Constructor

        /// <summary>
        ///   Creates a new instance of the <see cref="ChannelStatus" /> class.
        /// </summary>
        private ChannelStatus(string symbol)
        {
            this.Symbol = symbol;
        }

        #endregion


        #region Properties

        /// <summary>
        ///   Gets the string representation of the status.
        /// </summary>
        public string Symbol
        {
            get;
            private set;
        }

        #endregion


        #region Methods

        /// <summary>
        ///   Creates a representation of the message in IRC format.
        /// </summary>
        public override string ToString()
        {
            return this.Symbol;
        }

        #endregion


        #region IEquatable<ChannelStatus> Members

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(ChannelStatus other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || this.Symbol == other.Symbol);
        }

        #endregion


        /// <summary>
        ///   Determines whether the specified <see cref="Object" /> is equal to the current
        ///   <see cref="ChannelStatus" />.
        /// </summary>
        /// <param name="obj">The <see cref="Object" /> to compare with the current <see cref="ChannelStatus" />.</param>
        /// <returns>true if the specified <see cref="Object" /> is equal to the current <see cref="ChannelStatus" />; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ChannelStatus);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="ChannelStatus" />.</returns>
        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        public static bool operator ==(ChannelStatus leftOperand, ChannelStatus rightOperand)
        {
            return ReferenceEquals(null, leftOperand) ? ReferenceEquals(null, rightOperand) : leftOperand.Equals(rightOperand);
        }

        public static bool operator !=(ChannelStatus leftOperand, ChannelStatus rightOperand)
        {
            return !(leftOperand == rightOperand);
        }
    }
}
