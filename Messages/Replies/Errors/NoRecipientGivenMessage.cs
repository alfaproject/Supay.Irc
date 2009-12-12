using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Error message received primarily when a TextMessage is sent without any Targets.
  /// </summary>
  /// <remarks>
  /// Some other commands may also send this when no recipients are specified.
  /// </remarks>
  [Serializable]
  public class NoRecipientGivenMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NoRecipientGivenMessage"/> class.
    /// </summary>
    public NoRecipientGivenMessage()
      : base() {
      this.InternalNumeric = 411;
    }

    /// <summary>
    /// Gets or sets the command of the message which was invalid.
    /// </summary>
    public virtual String Command {
      get {
        return command;
      }
      set {
        command = value;
      }
    }
    private String command = "";

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(String.Format(CultureInfo.InvariantCulture, "No recipient given ({0})", this.Command));
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Command = "";
      if (parameters.Count > 1) {
        this.Command = MessageUtil.StringBetweenStrings(parameters[1], "No recipient given (", ")");
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNoRecipientGiven(new IrcMessageEventArgs<NoRecipientGivenMessage>(this));
    }

  }

}