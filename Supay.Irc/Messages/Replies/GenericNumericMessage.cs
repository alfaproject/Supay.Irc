using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Represents a message with a numeric command that is either unparsable or unimplemented.
  /// </summary>
  [Serializable]
  public class GenericNumericMessage : NumericMessage
  {
    private IList<string> data = new Collection<string>();

    /// <summary>
    ///   Gets the Numeric command of the Message
    /// </summary>
    public virtual int Command
    {
      get
      {
        return this.InternalNumeric;
      }
      set
      {
        this.InternalNumeric = value;
      }
    }

    /// <summary>
    ///   Gets the text of the Message
    /// </summary>
    public virtual IEnumerable<string> Data
    {
      get
      {
        return this.data;
      }
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        foreach (string datum in this.Data)
        {
          parameters.Add(datum);
        }
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the command portion of the message.
    /// </summary>
    protected override void ParseCommand(string command)
    {
      base.ParseCommand(command);
      this.Command = Convert.ToInt32(command, CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.data = parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnGenericNumericMessage(new IrcMessageEventArgs<GenericNumericMessage>(this));
    }
  }
}
