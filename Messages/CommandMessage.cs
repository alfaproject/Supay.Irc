using System;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   The base for all messages which send a text command. </summary>
  [Serializable]
  public abstract class CommandMessage : IrcMessage {

    /// <summary>
    ///   Gets the Irc command associated with this message. </summary>
    protected abstract string Command {
      get;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.AddParametersToFormat"/>. </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      writer.AddParameter(this.Command);
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type. </summary>
    public override bool CanParse(string unparsedMessage) {
      string messageCommand = MessageUtil.GetCommand(unparsedMessage);
      return messageCommand.EqualsI(this.Command);
    }

  } //class CommandMessage
} //namespace Supay.Irc.Messages