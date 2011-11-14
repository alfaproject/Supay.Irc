using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The error received when a message containing target parameters has too many targets specified.
  /// </summary>
  [Serializable]
  public class TooManyTargetsMessage : ErrorMessage
  {
    private string abortMessage = string.Empty;
    private string errorCode = string.Empty;
    private string invalidTarget = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="TooManyTargetsMessage" /> class.
    /// </summary>
    public TooManyTargetsMessage()
      : base(407)
    {
    }

    /// <summary>
    ///   Gets or sets the target which was invalid.
    /// </summary>
    public virtual string InvalidTarget
    {
      get
      {
        return this.invalidTarget;
      }
      set
      {
        this.invalidTarget = value;
      }
    }

    /// <summary>
    ///   Gets or sets the error code
    /// </summary>
    /// <remarks>
    ///   An example error code might be, "Duplicate"
    /// </remarks>
    public virtual string ErrorCode
    {
      get
      {
        return this.errorCode;
      }
      set
      {
        this.errorCode = value;
      }
    }

    /// <summary>
    ///   Gets or sets the message explaining what was done about the error.
    /// </summary>
    public virtual string AbortMessage
    {
      get
      {
        return this.abortMessage;
      }
      set
      {
        this.abortMessage = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.InvalidTarget);
      parameters.Add(this.ErrorCode + " recipients. " + this.AbortMessage);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.InvalidTarget = string.Empty;
      this.ErrorCode = string.Empty;
      this.AbortMessage = string.Empty;

      if (parameters.Count > 1)
      {
        this.InvalidTarget = parameters[1];
        if (parameters.Count > 2)
        {
          var messagePieces = Regex.Split(parameters[2], " recipients.");
          if (messagePieces.Length == 2)
          {
            this.ErrorCode = messagePieces[0];
            this.AbortMessage = messagePieces[1];
          }
        }
      }
    }

    // "<target> :<error code> recipients. <abort message>"

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnTooManyTargets(new IrcMessageEventArgs<TooManyTargetsMessage>(this));
    }
  }
}
