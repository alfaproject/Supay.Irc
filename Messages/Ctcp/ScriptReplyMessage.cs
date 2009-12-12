using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The reply to a <see cref="ScriptRequestMessage"/>.
  /// </summary>
  [Serializable]
  public class ScriptReplyMessage : CtcpReplyMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ScriptReplyMessage"/> class.
    /// </summary>
    public ScriptReplyMessage()
      : base() {
      this.InternalCommand = "SCRIPT";
    }

    /// <summary>
    /// Gets or sets the script name that the client is using
    /// </summary>
    public virtual String Response {
      get {
        return this.response;
      }
      set {
        this.response = value;
      }
    }
    private String response = "";


    /// <summary>
    /// Gets the data payload of the Ctcp request.
    /// </summary>
    protected override String ExtendedData {
      get {
        return this.response;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnScriptReply(new IrcMessageEventArgs<ScriptReplyMessage>(this));
    }

    /// <summary>
    /// Parses the given string to populate this <see cref="IrcMessage"/>.
    /// </summary>
    public override void Parse(String unparsedMessage) {
      base.Parse(unparsedMessage);
      this.Response = CtcpUtil.GetExtendedData(unparsedMessage);
    }

  }

}