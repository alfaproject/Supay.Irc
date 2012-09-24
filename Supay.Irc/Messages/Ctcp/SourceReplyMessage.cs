using System;
using System.Collections.Generic;
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
        /// <summary>
        ///   Creates a new instance of the <see cref="SourceReplyMessage" /> class.
        /// </summary>
        public SourceReplyMessage()
        {
            this.InternalCommand = "SOURCE";
            Server = string.Empty;
            Folder = string.Empty;
            Files = new List<string>();
        }

        /// <summary>
        ///   Gets or sets the server that hosts the client's distribution.
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the folder path to the client's distribution.
        /// </summary>
        public string Folder
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the list of files that must be downloaded.
        /// </summary>
        public ICollection<string> Files
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the data payload of the Ctcp request.
        /// </summary>
        protected override string ExtendedData
        {
            get
            {
                var result = new StringBuilder();
                result.Append(this.Server);
                result.Append(":");
                result.Append(this.Folder);
                if (this.Files.Count > 0)
                {
                    result.Append(":");
                    result.Append(string.Join(" ", this.Files));
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
                        var fs = MessageUtil.GetParameters(p[2]);
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
