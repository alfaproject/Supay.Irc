using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This reply should be sent whenever a client receives a <see cref="CtcpRequestMessage" /> that is not understood or is malformed.
  /// </summary>
  [Serializable]
  public class ErrorReplyMessage : CtcpReplyMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="ErrorReplyMessage" /> class.
    /// </summary>
    public ErrorReplyMessage() {
      InternalCommand = "ERRMSG";
    }

    /// <summary>
    ///   Gets or sets the text of the query which couldn't be processed.
    /// </summary>
    public virtual string Query {
      get {
        return query;
      }
      set {
        query = value;
      }
    }

    private string query = string.Empty;

    /// <summary>
    ///   Gets or sets the reason the request couldn't be processed.
    /// </summary>
    public virtual string Reason {
      get {
        return reason;
      }
      set {
        reason = value;
      }
    }

    private string reason = string.Empty;

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData {
      get {
        return query + " " + reason;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnErrorReply(new IrcMessageEventArgs<ErrorReplyMessage>(this));
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage) {
      base.Parse(unparsedMessage);
      string eData = CtcpUtil.GetExtendedData(unparsedMessage);
      Collection<string> p = MessageUtil.GetParameters(eData);
      if (p.Count == 2) {
        Query = p[0];
        Reason = p[1];
      }
    }
  }
}
