using System;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is a request to start a DCC chat.
  /// </summary>
  public class DccChatRequestMessage : DccRequestMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="DccChatRequestMessage" /> class.
    /// </summary>
    public DccChatRequestMessage()
    {
      this.Secure = false;
    }

    /// <summary>
    ///   Gets the DCC sub-command.
    /// </summary>
    protected override string DccCommand
    {
      get
      {
        return this.Secure ? "SCHAT" : "CHAT";
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command's argument.
    /// </summary>
    protected override string DccArgument
    {
      get
      {
        return "chat";
      }
    }

    /// <summary>
    ///   Gets or sets if the DCC connection should use SSL.
    /// </summary>
    public bool Secure
    {
      get;
      set;
    }

    /// <summary>
    ///   Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public override bool CanParseDccCommand(string command)
    {
      return !string.IsNullOrEmpty(command) && command.EndsWith("CHAT", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      this.Secure = DccUtil.GetCommand(unparsedMessage).StartsWith("S", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnDccChatRequest(new IrcMessageEventArgs<DccChatRequestMessage>(this));
    }
  }
}
