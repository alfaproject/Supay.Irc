using System;
using System.Collections.Generic;
using System.Globalization;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is an acknowledgement to resume sending a file previously, but not completely
  ///   sent to the requester.
  /// </summary>
  [Serializable]
  public class DccAcceptRequestMessage : CtcpRequestMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="DccAcceptRequestMessage" /> class.
    /// </summary>
    public DccAcceptRequestMessage()
    {
      this.Position = -1;
      this.Port = -1;
      this.FileName = string.Empty;
      this.InternalCommand = "DCC";
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        return MessageUtil.ParametersToString(false, DccCommand, this.FileName, this.Port.ToString(CultureInfo.InvariantCulture), this.Position.ToString(CultureInfo.InvariantCulture));
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command.
    /// </summary>
    protected static string DccCommand
    {
      get
      {
        return "ACCEPT";
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
    ///   Gets or sets the port the connection should be on.
    /// </summary>
    public int Port
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the position in the file at which to resume sending.
    /// </summary>
    public int Position
    {
      get;
      set;
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage)
    {
      return base.CanParse(unparsedMessage) && this.CanParseDccCommand(DccUtil.GetCommand(unparsedMessage));
    }

    /// <summary>
    ///   Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public bool CanParseDccCommand(string command)
    {
      return !string.IsNullOrEmpty(command) && DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      this.FileName = DccUtil.GetArgument(unparsedMessage);
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      if (parameters.Count >= 4)
      {
        this.Port = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
        this.Position = Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture);
      }
      else
      {
        this.Port = -1;
        this.Position = -1;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the
    ///   current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnDccAcceptRequest(new IrcMessageEventArgs<DccAcceptRequestMessage>(this));
    }
  }
}
