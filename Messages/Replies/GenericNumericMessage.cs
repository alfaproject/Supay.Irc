using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Represents a message with a numeric command that is either unparsable or unimplemented.
  /// </summary>
  [Serializable]
  public class GenericNumericMessage : NumericMessage {
    /// <summary>
    ///   Gets the Numeric command of the Message
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
    ///   Gets the text of the Message
    /// </summary>
    public virtual IEnumerable<string> Data {
      get {
        return data;
      }
    }

    private Collection<string> data = new Collection<string>();

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      foreach (string datum in Data) {
        parameters.Add(datum);
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the command portion of the message.
    /// </summary>
    protected override void ParseCommand(string command) {
      base.ParseCommand(command);
      this.Command = Convert.ToInt32(command, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.data = parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnGenericNumericMessage(new IrcMessageEventArgs<GenericNumericMessage>(this));
    }
  }
}
