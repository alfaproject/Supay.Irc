using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The error received when a message containing target parameters has too many targets specified.
  /// </summary>
  [Serializable]
  public class TooManyTargetsMessage : ErrorMessage {
    private string abortMessage = string.Empty;
    private string errorCode = string.Empty;
    private string invalidTarget = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="TooManyTargetsMessage" /> class.
    /// </summary>
    public TooManyTargetsMessage()
      : base(407) {
    }

    /// <summary>
    ///   Gets or sets the target which was invalid.
    /// </summary>
    public virtual string InvalidTarget {
      get {
        return invalidTarget;
      }
      set {
        invalidTarget = value;
      }
    }

    /// <summary>
    ///   Gets or sets the error code
    /// </summary>
    /// <remarks>
    ///   An example error code might be, "Duplicate"
    /// </remarks>
    public virtual string ErrorCode {
      get {
        return errorCode;
      }
      set {
        errorCode = value;
      }
    }

    /// <summary>
    ///   Gets or sets the message explaining what was done about the error.
    /// </summary>
    public virtual string AbortMessage {
      get {
        return abortMessage;
      }
      set {
        abortMessage = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(InvalidTarget);
      parameters.Add(ErrorCode + " recipients. " + AbortMessage);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      InvalidTarget = string.Empty;
      ErrorCode = string.Empty;
      AbortMessage = string.Empty;

      if (parameters.Count > 1) {
        InvalidTarget = parameters[1];
        if (parameters.Count > 2) {
          string[] messagePieces = Regex.Split(parameters[2], " recipients.");
          if (messagePieces.Length == 2) {
            ErrorCode = messagePieces[0];
            AbortMessage = messagePieces[1];
          }
        }
      }
    }

    // "<target> :<error code> recipients. <abort message>"

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnTooManyTargets(new IrcMessageEventArgs<TooManyTargetsMessage>(this));
    }
  }
}
