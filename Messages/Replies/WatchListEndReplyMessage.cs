using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Signals the end of replies to a Watch system query.
  /// </summary>
  [Serializable]
  public class WatchListEndReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="WatchListEndReplyMessage" />.
    /// </summary>
    public WatchListEndReplyMessage()
      : base(607) {
    }

    /// <summary>
    ///   Gets or sets the type of the list which was sent.
    /// </summary>
    public string ListType {
      get {
        return listType;
      }
      set {
        listType = value;
      }
    }

    private string listType = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add("End of WATCH" + ListType);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      string lastParam = parameters[parameters.Count - 1];
      ListType = lastParam.Substring(lastParam.Length - 1);
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWatchListEndReply(new IrcMessageEventArgs<WatchListEndReplyMessage>(this));
    }
  }
}
