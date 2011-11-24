using System;
using System.Collections.Generic;
using System.Text;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The abstract base class for all IRC messages.
  /// </summary>
  [Serializable]
  public abstract class IrcMessage
  {
    /// <summary>
    ///   The computer or user who sent the current message.
    /// </summary>
    /// <remarks>
    ///   In the case of a server message, the Sender.Nick is the the name that the server calls
    ///   itself, usually its address. In the case of a user message, the Sender is a User
    ///   containing the Nick, UserName, and HostName...
    /// </remarks>
    public User Sender
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets the parameters needed to rebuild this message.
    /// </summary>
    /// <remarks>
    ///   When deriving from <see cref="IrcMessage" />, override this method to add the needed
    ///   parameters for proper message rebuilding.
    /// </remarks>
    protected abstract IList<string> GetParameters();

    /// <summary>
    ///   Validates this message against the given server support.
    /// </summary>
    public virtual void Validate(ServerSupport serverSupport)
    {
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public virtual void Parse(string unparsedMessage)
    {
      this.Sender = new User(MessageUtil.GetPrefix(unparsedMessage));
      this.ParseCommand(MessageUtil.GetCommand(unparsedMessage));
      this.ParseParameters(MessageUtil.GetParameters(unparsedMessage));
    }

    /// <summary>
    ///   Parses the command portion of the message.
    /// </summary>
    protected virtual void ParseCommand(string command)
    {
    }

    /// <summary>
    ///   Parses the parameter portion of the message.
    /// </summary>
    protected virtual void ParseParameters(IList<string> parameters)
    {
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public abstract void Notify(MessageConduit conduit);

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public abstract bool CanParse(string unparsedMessage);

    /// <summary>
    ///   Generates a string representation of the message.
    /// </summary>
    public override string ToString()
    {
      var sb = new StringBuilder(512);
      if (this.Sender != null && !string.IsNullOrEmpty(this.Sender.Nickname))
      {
        sb.Append(':');
        sb.Append(this.Sender);
        sb.Append(' ');
      }

      var parameters = this.GetParameters();
      for (int i = 0; i < parameters.Count - 1; i++)
      {
        sb.Append(parameters[i]);
        sb.Append(' ');
      }
      string lastParameter = parameters[parameters.Count - 1];
      if (lastParameter.IndexOf(' ') > 0)
      {
        sb.Append(':');
      }
      sb.Append(lastParameter);

      return sb.ToString();
    }
  }
}
