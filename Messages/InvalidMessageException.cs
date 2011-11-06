using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Exception thrown when a message parsed from a string is invalid.
  /// </summary>
  [Serializable]
  public class InvalidMessageException : Exception {
    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class.
    /// </summary>
    public InvalidMessageException() {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given message.
    /// </summary>
    public InvalidMessageException(string message)
      : base(message) {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given message
    ///   and inner exception.
    /// </summary>
    public InvalidMessageException(string message, Exception innerException)
      : base(message, innerException) {
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given Message
    ///   and <see cref="ReceivedMessage" />.
    /// </summary>
    /// <param name="message">Message explaining the exception.</param>
    /// <param name="receivedMessage">The Message received which was invalid.</param>
    public InvalidMessageException(string message, string receivedMessage)
      : this(message) {
      ReceivedMessage = receivedMessage;
    }

    /// <summary>
    ///   Initializes a new instance of the InvalidMessageException class with the given Message
    ///   and <see cref="ReceivedMessage" />.
    /// </summary>
    /// <param name="message">Message explaining the exception.</param>
    /// <param name="receivedMessage">The Message received which was invalid.</param>
    /// <param name="innerException">The exception that exists as the child exception to this one.</param>
    public InvalidMessageException(string message, string receivedMessage, Exception innerException)
      : this(message, innerException) {
      ReceivedMessage = receivedMessage;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets the message string that caused the exception.
    /// </summary>
    public string ReceivedMessage {
      get;
      private set;
    }

    /// <summary>
    ///   Gets the string content of the invalid message.
    /// </summary>
    public override string Message {
      get {
        return base.Message + Environment.NewLine + "ReceivedMessage: " + ReceivedMessage;
      }
    }

    #endregion

    #region ISerializable

    /// <summary>
    ///   Sets the <see cref="SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic).</exception>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);
      info.AddValue("ReceivedMessage", ReceivedMessage);
    }

    #endregion
  }
}
