using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message is similar to a <see cref="ChatMessage"/>, 
  /// except that no auto-replies should ever be sent after receiving a <see cref="NoticeMessage"/>.
  /// </summary>
  [Serializable]
  public class NoticeMessage : TextMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="NoticeMessage"/> class.
    /// </summary>
    public NoticeMessage()
      : base() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoticeMessage"/> class with the given text string.
    /// </summary>
    public NoticeMessage(String text)
      : base() {
      this.Text = text;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoticeMessage"/> class with the given text string and target channel or user.
    /// </summary>
    public NoticeMessage(String text, String target)
      : this(text) {
      this.Targets.Add(target);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NoticeMessage"/> class with the given text string and target channels or users.
    /// </summary>
    public NoticeMessage(String text, params String[] targets)
      : this(text) {
      this.Targets.AddRange(targets);
    }

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override String Command {
      get {
        return "NOTICE";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNotice(new IrcMessageEventArgs<TextMessage>(this));
    }
  }

}