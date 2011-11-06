using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// The <see cref="ErrorMessage"/> sent when a command is sent which doesn't contain all the required parameters
  /// </summary>
  [Serializable]
  public class NotEnoughParametersMessage : ErrorMessage {
    /// <summary>
    /// Creates a new instances of the <see cref="NotEnoughParametersMessage"/> class.
    /// </summary>
    public NotEnoughParametersMessage()
      : base(461) {
    }

    /// <summary>
    /// Gets or sets the command sent
    /// </summary>
    public string Command {
      get {
        return command;
      }
      set {
        command = value;
      }
    }

    private string command;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Command);
      parameters.Add("Not enough parameters");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Command = string.Empty;
      if (parameters.Count > 2) {
        this.Command = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNotEnoughParameters(new IrcMessageEventArgs<NotEnoughParametersMessage>(this));
    }
  }
}
