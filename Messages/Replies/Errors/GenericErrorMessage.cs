using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Represents an error message with a numeric command that is either unparsable or unimplemented.
  /// </summary>
  [Serializable]
  public class GenericErrorMessage : ErrorMessage {


    /// <summary>
    /// Gets or sets the Numeric command of the Message
    /// </summary>
    public virtual int Command {
      get {
        return InternalNumeric;
      }
      set {
        InternalNumeric = value;
      }
    }

    /// <summary>
    /// Gets the text of the Message
    /// </summary>
    public virtual Collection<string> Data {
      get {
        return data;
      }
    }
    private Collection<string> data = new Collection<string>();


    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Data, " "));
      return parameters;
    }

    /// <summary>
    /// Parses the command portion of the message.
    /// </summary>
    protected override void ParseCommand(string command) {
      base.ParseCommand(command);
      this.Command = Convert.ToInt32(command, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.data = parameters;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnGenericErrorMessage(new IrcMessageEventArgs<GenericErrorMessage>(this));
    }

  }

}