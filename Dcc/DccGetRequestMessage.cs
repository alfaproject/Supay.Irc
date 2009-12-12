using System;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Sends a request for the transfer of the given file.
  /// </summary>
  public class DccGetRequestMessage : CtcpRequestMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="DccGetRequestMessage"/> class.
    /// </summary>
    public DccGetRequestMessage()
      : base() {
      this.InternalCommand = "DCC";
    }


    /// <summary>
    /// Gets the data payload of the Ctcp request.
    /// </summary>
    protected override String ExtendedData {
      get {
        return MessageUtil.ParametersToString(false, this.DccCommand, this.FileName);
      }
    }

    /// <summary>
    /// Gets the dcc sub-command.
    /// </summary>

    protected virtual String DccCommand {
      get {
        String result = "GET";
        if (this.Secure) {
          result = "S" + result;
        }
        if (this.TurboMode) {
          result = "T" + result;
        }
        return result;
      }
    }

    /// <summary>
    /// Gets or sets the name of the file being requested.
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
    /// Gets or sets if the dcc connection should use turbo mode.
    /// </summary>
    public virtual bool TurboMode {
      get {
        return turboMode;
      }
      set {
        turboMode = value;
      }
    }

    /// <summary>
    /// Gets or sets if the dcc connection should use SSL.
    /// </summary>
    public virtual bool Secure {
      get {
        return secure;
      }
      set {
        secure = value;
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
      if (String.IsNullOrEmpty(command)) {
        return false;
      }
      return (this.DccCommand.ToUpperInvariant().EndsWith(command.ToUpperInvariant(), StringComparison.Ordinal));
    }

    /// <summary>
    /// Parses the given string to populate this <see cref="IrcMessage"/>.
    /// </summary>
    public override void Parse(String unparsedMessage) {
      base.Parse(unparsedMessage);
      this.FileName = DccUtil.GetArgument(unparsedMessage);
      String unparsedCommand = DccUtil.GetCommand(unparsedMessage).ToUpperInvariant();
      String commandExtenstion = unparsedCommand.Substring(0, unparsedCommand.Length - 3);
      this.TurboMode = commandExtenstion.IndexOf("T", StringComparison.Ordinal) >= 0;
      this.Secure = commandExtenstion.IndexOf("S", StringComparison.Ordinal) >= 0;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnDccGetRequest(new IrcMessageEventArgs<DccGetRequestMessage>(this));
    }

    private String fileName = "";
    private bool turboMode = false;
    private bool secure = false;

  }

}