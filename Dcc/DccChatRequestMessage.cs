using System;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message is a request to start a dcc chat.
  /// </summary>
  public class DccChatRequestMessage : DccRequestMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="DccChatRequestMessage"/> class.
    /// </summary>
    public DccChatRequestMessage()
      : base() {
    }

    /// <summary>
    /// Gets the dcc sub-command.
    /// </summary>
    protected override String DccCommand {
      get {
        if (this.Secure) {
          return "SCHAT";
        } else {
          return "CHAT";
        }
      }
    }

    /// <summary>
    /// Gets the dcc sub-command's argument.
    /// </summary>
    protected override String DccArgument {
      get {
        return "chat";
      }
    }

    /// <summary>
    /// Gets or sets if the dcc connection should use SSL.
    /// </summary>
    public virtual bool Secure {
      get {
        return secure;
      }
      set {
        secure = value;
      }
    }

    /// <summary>
    /// Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public override bool CanParseDccCommand(String command) {
      if (String.IsNullOrEmpty(command)) {
        return false;
      }
      return command.ToUpperInvariant().EndsWith("CHAT", StringComparison.Ordinal);
    }

    /// <summary>
    /// Parses the given string to populate this <see cref="IrcMessage"/>.
    /// </summary>
    public override void Parse(String unparsedMessage) {
      base.Parse(unparsedMessage);

      this.Secure = DccUtil.GetCommand(unparsedMessage).ToUpperInvariant().StartsWith("S", StringComparison.Ordinal);
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnDccChatRequest(new IrcMessageEventArgs<DccChatRequestMessage>(this));
    }

    private bool secure = false;

  }

}