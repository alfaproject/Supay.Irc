using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   The abstract base class for all IRC messages. </summary>
  [Serializable]
  public abstract class IrcMessage {

    /// <summary>
    ///   Generates a string representation of the message. </summary>
    public override string ToString() {
      using (IrcMessageWriter writer = new IrcMessageWriter { AppendNewLine = false }) {
        Format(writer);
        return writer.ToString();
      }
    }

    /// <summary>
    ///   Outputs message content to a provided <see cref="IrcMessageWriter"/> object. </summary>
    /// <param name="writer">
    ///   The <see cref="IrcMessageWriter"/> object that receives the message content. </param>
    public void Format(IrcMessageWriter writer) {
      if (writer == null) {
        return;
      }
      if (Sender != null && !string.IsNullOrEmpty(Sender.Nickname)) {
        writer.Sender = Sender.IrcMask;
      }
      AddParametersToFormat(writer);
      writer.Write();
    }

    /// <summary>
    ///   Adds parameters to the given <see cref="IrcMessageWriter"/> for formatting of the message. </summary>
    /// <remarks>
    ///   When deriving from <see cref="IrcMessage"/>, override this method to add parameters to
    ///   the formatted output of the message. </remarks>
    /// <param name="writer">
    ///   The <see cref="IrcMessageWriter"/> object that receives the message content. </param>
    protected abstract void AddParametersToFormat(IrcMessageWriter writer);

    /// <summary>
    ///   Validates this message against the given server support. </summary>
    public virtual void Validate(ServerSupport serverSupport) {
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage"/>. </summary>
    public virtual void Parse(string unparsedMessage) {
      Sender = new User(MessageUtil.GetPrefix(unparsedMessage));
      ParseCommand(MessageUtil.GetCommand(unparsedMessage));
      ParseParameters(MessageUtil.GetParameters(unparsedMessage));
    }

    /// <summary>
    ///   Parses the command portion of the message. </summary>
    protected virtual void ParseCommand(string command) {
    }

    /// <summary>
    ///   Parses the parameter portion of the message. </summary>
    protected virtual void ParseParameters(StringCollection parameters) {
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    ///   current <see cref="IrcMessage"/> subclass. </summary>
    public abstract void Notify(MessageConduit conduit);

    /// <summary>
    ///   Determines if the message can be parsed by this type. </summary>
    public abstract bool CanParse(string unparsedMessage);

    /// <summary>
    ///   The computer or user who sent the current message. </summary>
    /// <remarks>
    ///   In the case of a server message, the Sender.Nick is the the name that the server calls
    ///   itself, usually its address. In the case of a user message, the Sender is a User
    ///   containing the Nick, UserName, and HostName... </remarks>
    public User Sender {
      get;
      set;
    }

  } //class IrcMessage
} //namespace Supay.Irc.Messages