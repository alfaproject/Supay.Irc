using System;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   This message is the standard communication message for IRC. </summary>
  [Serializable]
  public class ChatMessage : TextMessage {

    /// <summary>
    ///   Creates a new instance of the <see cref="ChatMessage"/> class. </summary>
    public ChatMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChatMessage"/> class with the given text string. </summary>
    public ChatMessage(string text)
      : base(text) {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChatMessage"/> class with the given text string
    ///   and target channel or user. </summary>
    public ChatMessage(string text, string target)
      : this(text) {
      Targets.Add(target);
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ChatMessage"/> class with the given text string
    ///   and target channels or users. </summary>
    public ChatMessage(string text, params string[] targets)
      : this(text) {
      Targets.AddRange(targets);
    }

    /// <summary>
    ///   Gets the IRC command associated with this message. </summary>
    protected override string Command {
      get {
        return "PRIVMSG";
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    ///   current <see cref="IrcMessage"/> subclass. </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChat(new IrcMessageEventArgs<TextMessage>(this));
    }

  } //class ChatMessage
} //namespace Supay.Irc.Messages