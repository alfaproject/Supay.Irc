using System;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Exception thrown when a message parsed from a string is invalid. </summary>
  [Serializable]
  public class InvalidMessageException : Exception {

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class. </summary>
    public InvalidMessageException()
      : base() {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given message. </summary>
    public InvalidMessageException(string message)
      : base(message) {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given message and inner exception. </summary>
    public InvalidMessageException(string message, Exception innerException)
      : base(message, innerException) {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given Message and ReceivedMessage. </summary>
    /// <param name="message">
    ///   Message explaining the exception. </param>
    /// <param name="receivedMessage">
    ///   The Message received which was invalid. </param>
    public InvalidMessageException(string message, string receivedMessage)
      : this(message) {
      this.ReceivedMessage = receivedMessage;
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given Message and RecivedMessage. </summary>
    /// <param name="message">
    ///   Message explaining the exception. </param>
    /// <param name="receivedMessage">
    ///   The Message received which was invalid. </param>
    /// <param name="innerException">
    ///   The exception that exists as the child exception to this one. </param>
    public InvalidMessageException(string message, string receivedMessage, Exception innerException)
      : this(message, innerException) {
      this.ReceivedMessage = receivedMessage;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the message string that caused the exception. </summary>
    public string ReceivedMessage {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the string content of the invalid message. </summary>
    public override string Message {
      get {
        return base.Message + Environment.NewLine + "ReceivedMessage: " + this.ReceivedMessage;
      }
    }

    #endregion

  } //class InvalidMessageException
} //namespace Supay.Irc.Messages