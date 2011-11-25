using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Supay.Irc.Messages
{
  /// <summary>
  /// Represents errors that occur during IRC Message parsing.
  /// </summary>
  [Serializable]
  public class InvalidMessageException : Exception
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class.
    /// </summary>
    public InvalidMessageException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public InvalidMessageException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InvalidMessageException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class with a specified error message
    /// and received IRC Message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="receivedMessage">The IRC Message received which was invalid.</param>
    public InvalidMessageException(string message, string receivedMessage)
      : this(message)
    {
      this.ReceivedMessage = receivedMessage;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class with a specified error message,
    /// received IRC Message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="receivedMessage">The IRC Message received which was invalid.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InvalidMessageException(string message, string receivedMessage, Exception innerException)
      : this(message, innerException)
    {
      this.ReceivedMessage = receivedMessage;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidMessageException" /> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected InvalidMessageException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.ReceivedMessage = info.GetString("ReceivedMessage");
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the IRC Message that caused the exception.
    /// </summary>
    public string ReceivedMessage
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    public override string Message
    {
      get
      {
        return base.Message + Environment.NewLine + "ReceivedMessage: " + this.ReceivedMessage;
      }
    }

    #endregion

    #region ISerializable

    /// <summary>
    /// Sets the <see cref="SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is a null reference.</exception>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("ReceivedMessage", this.ReceivedMessage);
    }

    #endregion
  }
}
