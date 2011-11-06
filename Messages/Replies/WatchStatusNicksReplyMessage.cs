using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// A reply for the <see cref="WatchListRequestMessage"/> query stating the users on your watch list.
  /// </summary>
  /// <remarks>
  /// You may receive more than 1 of these in response to the request.
  /// </remarks>
  [Serializable]
  public class WatchStatusNicksReplyMessage : NumericMessage {
    /// <summary>
    /// Creates a new instance of the <see cref="WatchStatusNicksReplyMessage"/>.
    /// </summary>
    public WatchStatusNicksReplyMessage()
      : base(606) {
    }

    /// <summary>
    /// Gets the collection of nicks of the users on the watch list.
    /// </summary>
    public List<string> Nicks {
      get {
        if (nicks == null) {
          nicks = new List<string>();
        }
        return nicks;
      }
    }

    private List<string> nicks;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Nicks, " "));
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      this.Nicks.Clear();
      string lastParam = parameters[parameters.Count - 1];
      if (!string.IsNullOrEmpty(lastParam)) {
        this.Nicks.AddRange(lastParam.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWatchStatusNicksReply(new IrcMessageEventArgs<WatchStatusNicksReplyMessage>(this));
    }
  }
}
