using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The astract base class for all irc messages.
  /// </summary>
  [Serializable]
  public abstract class IrcMessage {

    /// <summary>
    /// Creates an instance of whatever type is deriving from IrcMessage.
    /// This is not meant to be used from application code.
    /// </summary>
    public virtual IrcMessage CreateInstance() {
      return (IrcMessage)Activator.CreateInstance(this.GetType());
    }

    /// <summary>
    /// Generates a string representation of the message.
    /// </summary>
    public override String ToString() {
      using (System.IO.StringWriter target = new System.IO.StringWriter(CultureInfo.InvariantCulture))
      using (IrcMessageWriter writer = new IrcMessageWriter(target)) {
        writer.AppendNewLine = false;
        this.Format(writer);
        return target.ToString();
      }
    }

    /// <summary>
    /// Outputs message content to a provided <see cref="IrcMessageWriter"/> object.
    /// </summary>
    /// <param name="writer">The <see cref="IrcMessageWriter"/> object that receives the message content. </param>
    public virtual void Format(IrcMessageWriter writer) {
      if (writer == null) {
        return;
      }
      if (!string.IsNullOrEmpty(this.Sender.Nick)) {
        writer.Sender = this.Sender.Mask;
      }
      this.AddParametersToFormat(writer);
      writer.Write();
    }

    /// <summary>
    /// Adds parameters to the given <see cref="IrcMessageWriter"/> for formatting of the message.
    /// </summary>
    /// <remarks>
    /// When deriving from IrcMessage, override this method to add parameters to the formatted output of the message.
    /// </remarks>
    /// <param name="writer">The <see cref="IrcMessageWriter"/> object that receives the message content. </param>
    protected virtual void AddParametersToFormat(IrcMessageWriter writer) {

    }

    /// <summary>
    /// Validates this message against the given server support
    /// </summary>
    public virtual void Validate(Supay.Irc.ServerSupport serverSupport) {

    }

    /// <summary>
    /// Parses the given string to populate this <see cref="IrcMessage"/>.
    /// </summary>
    public virtual void Parse(String unparsedMessage) {
      this.Sender = new User(MessageUtil.GetPrefix(unparsedMessage));
      this.ParseCommand(MessageUtil.GetCommand(unparsedMessage));
      this.ParseParameters(MessageUtil.GetParameters(unparsedMessage));
    }

    /// <summary>
    /// Parses the command portion of the message.
    /// </summary>
    protected virtual void ParseCommand(String command) {

    }

    /// <summary>
    /// Parses the parameter portion of the message.
    /// </summary>
    protected virtual void ParseParameters(StringCollection parameters) {

    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public abstract void Notify(Supay.Irc.Messages.MessageConduit conduit);

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public abstract bool CanParse(String unparsedMessage);

    /// <summary>
    /// The computer or user who sent the current message.
    /// </summary>
    /// <remarks>
    /// In the case of a server message, the Sender.Nick is the the name that the server calls itself, usually its address.
    /// In the case of a user message, the Sender is a User containing the Nick, UserName, and HostName..
    /// </remarks>
    public User Sender {
      get {
        return this.sender;
      }
      set {
        sender = value;
      }
    }
    private User sender = new User();

  }

}