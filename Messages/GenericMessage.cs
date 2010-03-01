using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Represents a single generic RFC1459 IRC message to or from an IRC server. </summary>
  [Serializable]
  public class GenericMessage : IrcMessage {

    #region Constructor

    public GenericMessage() {
      _command = string.Empty;
      this.Parameters = new StringCollection();
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the message's command. </summary>
    public string Command {
      get {
        return _command;
      }
      set {
        if (value == null) {
          throw new ArgumentNullException("value");
        }
        _command = value;
      }
    }
    private string _command;

    /// <summary>
    ///   Gets the message's parameters after the command. </summary>
    public StringCollection Parameters {
      get;
      private set;
    }

    #endregion

    #region IrcMessage Methods

    /// <summary>
    ///   This is not meant to be used from your code. </summary>
    /// <remarks>
    ///   The conduit calls Notify on messages to have the message raise the appropriate event on the conduit.
    ///   This is done automaticly by your <see cref="Client"/> after message are recieved and parsed. </remarks>
    public override void Notify(MessageConduit conduit) {
      conduit.OnGenericMessage(new IrcMessageEventArgs<GenericMessage>(this));
    }

    /// <summary>
    ///   Overrides <see cref="AddParametersToFormat"/>. </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      writer.AddParameter(this.Command);
      foreach (string param in this.Parameters) {
        writer.AddParameter(param);
      }
    }

    /// <summary>
    ///   Determines if the given string is parsable by this <see cref="IrcMessage"/> subclass. </summary>
    /// <remarks>
    ///   <see cref="GenericMessage"/> always returns true. </remarks>
    public override bool CanParse(string unparsedMessage) {
      return true;
    }

    /// <summary>
    ///   Parses the command portion of the message. </summary>
    protected override void ParseCommand(string command) {
      base.ParseCommand(command);
      this.Command = command;
    }

    /// <summary>
    ///   Parses the parameter portion of the message. </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Parameters = parameters;
    }

    #endregion

  } //class GenericMessage
} //namespace Supay.Irc.Messages