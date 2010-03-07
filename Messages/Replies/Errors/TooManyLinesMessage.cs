using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The <see cref="ErrorMessage"/> sent when a command would result in too many lines in the reply.
  /// </summary>
  [Serializable]
  public class TooManyLinesMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="TooManyLinesMessage"/> class.
    /// </summary>
    public TooManyLinesMessage()
      : base(416) {
    }

    /// <summary>
    /// Gets or sets the command which caused the error.
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
      parameters.Add("Too many lines in the output, restrict your query");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Command = string.Empty;
      if (parameters.Count > 1) {
        this.Command = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnTooManyLines(new IrcMessageEventArgs<TooManyLinesMessage>(this));
    }

  }

}