using System;
using System.Collections.Specialized;
using System.Globalization;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages {

  /// <summary>
  /// This message is a request to resume sending a file previously, but not completely sent to the requestor.
  /// </summary>
  [Serializable]
  public class DccResumeRequestMessage : CtcpRequestMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="DccResumeRequestMessage"/> class.
    /// </summary>
    public DccResumeRequestMessage()
      : base() {
      this.InternalCommand = "DCC";
    }

    /// <summary>
    /// Gets the data payload of the Ctcp request.
    /// </summary>
    protected override String ExtendedData {
      get {
        return MessageUtil.ParametersToString(false, this.DccCommand, this.FileName, this.Port.ToString(CultureInfo.InvariantCulture), this.Position.ToString(CultureInfo.InvariantCulture));
      }
    }

    /// <summary>
    /// Gets the dcc sub-command.
    /// </summary>
    protected virtual String DccCommand {
      get {
        return "RESUME";
      }
    }

    /// <summary>
    /// Gets or sets the name of the file being sent.
    /// </summary>
    public virtual String FileName {
      get {
        return fileName;
      }
      set {
        fileName = value;
      }
    }

    /// <summary>
    /// Gets or sets the port the connection should be on.
    /// </summary>
    public virtual int Port {
      get {
        return port;
      }
      set {
        port = value;
      }
    }

    /// <summary>
    /// Gets or sets the position in the file at which to resume sending.
    /// </summary>
    public virtual int Position {
      get {
        return position;
      }
      set {
        position = value;
      }
    }

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(String unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }

      return CanParseDccCommand(DccUtil.GetCommand(unparsedMessage));
    }

    /// <summary>
    /// Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public virtual bool CanParseDccCommand(String command) {
      if (command == null) {
        return false;
      }
      return this.DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parses the given string to populate this <see cref="IrcMessage"/>.
    /// </summary>
    public override void Parse(String unparsedMessage) {
      base.Parse(unparsedMessage);
      this.FileName = DccUtil.GetArgument(unparsedMessage);
      StringCollection p = DccUtil.GetParameters(unparsedMessage);
      this.Port = Convert.ToInt32(p[2], CultureInfo.InvariantCulture);
      this.Position = Convert.ToInt32(p[3], CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnDccResumeRequest(new IrcMessageEventArgs<DccResumeRequestMessage>(this));
    }

    private String fileName = "";
    private int port = -1;
    private int position = -1;

  }

}