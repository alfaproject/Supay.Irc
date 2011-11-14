using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The reply to a <see cref="SourceRequestMessage" />, 
  ///   telling the requestor where to download this client.
  /// </summary>
  [Serializable]
  public class SourceReplyMessage : CtcpReplyMessage
  {
    private readonly Collection<string> files = new Collection<string>();
    private string folder = string.Empty;
    private string server = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="SourceReplyMessage" /> class.
    /// </summary>
    public SourceReplyMessage()
    {
      this.InternalCommand = "SOURCE";
    }

    /// <summary>
    ///   Gets or sets the server that hosts the client's distribution.
    /// </summary>
    public virtual string Server
    {
      get
      {
        return this.server;
      }
      set
      {
        this.server = value;
      }
    }

    /// <summary>
    ///   Gets or sets the folder path to the client's distribution.
    /// </summary>
    public virtual string Folder
    {
      get
      {
        return this.folder;
      }
      set
      {
        this.folder = value;
      }
    }

    /// <summary>
    ///   Gets the list of files that must be downloaded.
    /// </summary>
    public virtual Collection<string> Files
    {
      get
      {
        return this.files;
      }
    }

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        StringBuilder result = new StringBuilder();
        result.Append(this.Server);
        result.Append(":");
        result.Append(this.Folder);
        if (this.Files.Count > 0)
        {
          result.Append(":");
          result.Append(MessageUtil.CreateList(this.Files, " "));
        }
        return result.ToString();
      }
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      string eData = CtcpUtil.GetExtendedData(unparsedMessage);
      var p = eData.Split(':');
      if (p.Length > 0)
      {
        this.Server = p[0];
        if (p.Length > 1)
        {
          this.Folder = p[1];
          if (p.Length == 3)
          {
            ICollection fs = MessageUtil.GetParameters(p[2]);
            foreach (string f in fs)
            {
              this.Files.Add(f);
            }
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnSourceReply(new IrcMessageEventArgs<SourceReplyMessage>(this));
    }
  }
}
