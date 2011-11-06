using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Error message received primarily when a <see cref="TextMessage" /> is sent without any Targets.
  /// </summary>
  /// <remarks>
  ///   Some other commands may also send this when no recipients are specified.
  /// </remarks>
  [Serializable]
  public class NoRecipientGivenMessage : ErrorMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="NoRecipientGivenMessage" /> class.
    /// </summary>
    public NoRecipientGivenMessage()
      : base(411) {
    }

    /// <summary>
    ///   Gets or sets the command of the message which was invalid.
    /// </summary>
    public virtual string Command {
      get {
        return command;
      }
      set {
        command = value;
      }
    }

    private string command = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(string.Format(CultureInfo.InvariantCulture, "No recipient given ({0})", Command));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Command = string.Empty;
      if (parameters.Count > 1) {
        Command = MessageUtil.StringBetweenStrings(parameters[1], "No recipient given (", ")");
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnNoRecipientGiven(new IrcMessageEventArgs<NoRecipientGivenMessage>(this));
    }
  }
}
