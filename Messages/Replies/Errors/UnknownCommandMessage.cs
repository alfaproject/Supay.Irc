using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> sent when a command is sent to a server which didn't recognize it.
  /// </summary>
  [Serializable]
  public class UnknownCommandMessage : ErrorMessage {
    private string command;

    /// <summary>
    ///   Creates a new instances of the <see cref="TooManyLinesMessage" /> class.
    /// </summary>
    public UnknownCommandMessage()
      : base(421) {
    }

    /// <summary>
    ///   Gets or sets the command which caused the error.
    /// </summary>
    public string Command {
      get {
        return command;
      }
      set {
        command = value;
      }
    }

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Command);
      parameters.Add("Unknown command");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Command = string.Empty;
      if (parameters.Count > 1) {
        Command = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnUnknownCommand(new IrcMessageEventArgs<UnknownCommandMessage>(this));
    }
  }
}
