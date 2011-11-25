using System;
using System.Globalization;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is a request to send a file directly from the sender of the request to the
  ///   receiver.
  /// </summary>
  [Serializable]
  public class DccSendRequestMessage : DccRequestMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="DccSendRequestMessage" /> class.
    /// </summary>
    public DccSendRequestMessage()
    {
      this.Secure = false;
      this.TurboMode = false;
      this.Size = -1;
      this.FileName = string.Empty;
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        return base.ExtendedData + " " + this.Size.ToString(CultureInfo.InvariantCulture);
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command.
    /// </summary>
    protected override string DccCommand
    {
      get
      {
        string result = "SEND";
        if (this.Secure)
        {
          result = "S" + result;
        }
        if (this.TurboMode)
        {
          result = "T" + result;
        }
        return result;
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command's argument.
    /// </summary>
    protected override string DccArgument
    {
      get
      {
        return this.FileName;
      }
    }

    /// <summary>
    ///   Gets or sets the name of the file being sent.
    /// </summary>
    public string FileName
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the size of the file being sent.
    /// </summary>
    public int Size
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets if the DCC connection should use turbo mode.
    /// </summary>
    public bool TurboMode
    {
      get;
      set;
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
      return command != null && command.EndsWith("SEND", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      this.FileName = DccUtil.GetArgument(unparsedMessage);
      this.Size = Convert.ToInt32(DccUtil.GetParameters(unparsedMessage)[4], CultureInfo.InvariantCulture);
      string unparsedCommand = DccUtil.GetCommand(unparsedMessage).ToUpperInvariant();
      string commandExtenstion = unparsedCommand.Substring(0, unparsedCommand.Length - 4);
      this.TurboMode = commandExtenstion.IndexOf('T') >= 0;
      this.Secure = commandExtenstion.IndexOf('S') >= 0;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnDccSendRequest(new IrcMessageEventArgs<DccSendRequestMessage>(this));
    }
  }
}
