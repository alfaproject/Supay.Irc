using System;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This class of message is sent to a client from a server when something bad happens.
  /// </summary>
  [Serializable]
  public abstract class ErrorMessage : NumericMessage {
    protected ErrorMessage() {
    }

    protected ErrorMessage(int number) {
      setNumeric(number);
    }

    /// <summary>
    ///   Gets the Numeric command of the Message
    /// </summary>
    public override int InternalNumeric {
      get {
        return base.InternalNumeric;
      }
      protected set {
        setNumeric(value);
      }
    }

    private void setNumeric(int number) {
      if (IsError(number)) {
        base.InternalNumeric = number;
      } else {
        throw new ArgumentOutOfRangeException("number", number, Properties.Resources.ErrorMessageNumericsMustBeBetween);
      }
    }
  }
}
