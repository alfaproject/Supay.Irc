using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message is the standard communication message for irc.
  /// </summary>
  [Serializable]
  public class ChatMessage : TextMessage {
    /// <summary>
    /// Creates a new instance of the <see cref="ChatMessage"/> class.
    /// </summary>
    public override IrcMessage CreateInstance() {
      return new ChatMessage();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatMessage"/> class.
    /// </summary>
    public ChatMessage()
      : base() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatMessage"/> class with the given text string.
    /// </summary>
    public ChatMessage(String text)
      : base() {
      this.Text = text;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatMessage"/> class with the given text string and target channel or user.
    /// </summary>
    public ChatMessage(String text, String target)
      : this(text) {
      this.Targets.Add(target);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ChatMessage"/> class with the given text string and target channels or users.
    /// </summary>
    public ChatMessage(String text, params String[] targets)
      : this(text) {
      this.Targets.AddRange(targets);
    }

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override String Command {
      get {
        return "PRIVMSG";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChat(new IrcMessageEventArgs<TextMessage>(this));
    }
  }

}