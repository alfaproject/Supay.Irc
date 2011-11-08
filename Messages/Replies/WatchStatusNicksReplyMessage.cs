using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply for the <see cref="WatchListRequestMessage" /> query stating the users on your watch list.
  /// </summary>
  /// <remarks>
  ///   You may receive more than 1 of these in response to the request.
  /// </remarks>
  [Serializable]
  public class WatchStatusNicksReplyMessage : NumericMessage {
    private List<string> nicks;

    /// <summary>
    ///   Creates a new instance of the <see cref="WatchStatusNicksReplyMessage" />.
    /// </summary>
    public WatchStatusNicksReplyMessage()
      : base(606) {
    }

    /// <summary>
    ///   Gets the collection of nicks of the users on the watch list.
    /// </summary>
    public List<string> Nicks {
      get {
        return nicks ?? (nicks = new List<string>());
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Nicks, " "));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);

      Nicks.Clear();
      string lastParam = parameters[parameters.Count - 1];
      if (!string.IsNullOrEmpty(lastParam)) {
        Nicks.AddRange(lastParam.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWatchStatusNicksReply(new IrcMessageEventArgs<WatchStatusNicksReplyMessage>(this));
    }
  }
}
