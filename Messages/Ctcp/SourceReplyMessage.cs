using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The reply to a <see cref="SourceRequestMessage" />, 
  ///   telling the requestor where to download this client.
  /// </summary>
  [Serializable]
  public class SourceReplyMessage : CtcpReplyMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="SourceReplyMessage" /> class.
    /// </summary>
    public SourceReplyMessage()
      : base() {
      InternalCommand = "SOURCE";
    }

    /// <summary>
    ///   Gets or sets the server that hosts the client's distribution.
    /// </summary>
    public virtual string Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    private string server = string.Empty;

    /// <summary>
    ///   Gets or sets the folder path to the client's distribution.
    /// </summary>
    public virtual string Folder {
      get {
        return folder;
      }
      set {
        folder = value;
      }
    }

    private string folder = string.Empty;

    /// <summary>
    ///   Gets the list of files that must be downloaded.
    /// </summary>
    public virtual Collection<string> Files {
      get {
        return files;
      }
    }

    private Collection<string> files = new Collection<string>();

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData {
      get {
        StringBuilder result = new StringBuilder();
        result.Append(Server);
        result.Append(":");
        result.Append(Folder);
        if (Files.Count > 0) {
          result.Append(":");
          result.Append(MessageUtil.CreateList(Files, " "));
        }
        return result.ToString();
      }
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage) {
      base.Parse(unparsedMessage);
      string eData = CtcpUtil.GetExtendedData(unparsedMessage);
      string[] p = eData.Split(':');
      if (p.Length > 0) {
        Server = p[0];
        if (p.Length > 1) {
          Folder = p[1];
          if (p.Length == 3) {
            Collection<string> fs = MessageUtil.GetParameters(p[2]);
            foreach (string f in fs) {
              Files.Add(f);
            }
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnSourceReply(new IrcMessageEventArgs<SourceReplyMessage>(this));
    }
  }
}
