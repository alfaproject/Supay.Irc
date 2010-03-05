using System;
using System.Collections.Specialized;

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
      : base() {
      this.InternalNumeric = 606;
    }

    /// <summary>
    /// Gets the collection of nicks of the users on the watch list.
    /// </summary>
    public StringCollection Nicks {
      get {
        if (nicks == null) {
          nicks = new StringCollection();
        }
        return nicks;
      }
    }
    private StringCollection nicks;

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    public override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddList(this.Nicks, " ", false);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
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