using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The base for all messages which send a text command.
  /// </summary>
  [Serializable]
  public abstract class CommandMessage : IrcMessage {
    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected abstract string Command {
      get;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      return new Collection<string> {
        Command
      };
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      string messageCommand = MessageUtil.GetCommand(unparsedMessage);
      return messageCommand.EqualsI(Command);
    }
  }
}
