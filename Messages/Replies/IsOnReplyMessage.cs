using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The server reply to an <see cref="IsOnMessage"/>.
  /// </summary>
  [Serializable]
  public class IsOnReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="IsOnReplyMessage"/> class.
    /// </summary>
    public IsOnReplyMessage()
      : base() {
      this.InternalNumeric = 303;
    }

    /// <summary>
    /// Gets the list of nicks of people who are known to be online.
    /// </summary>
    public virtual StringCollection Nicks {
      get {
        return this.nicks;
      }
    }
    private StringCollection nicks = new StringCollection();

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
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.Nicks.Clear();
      this.Nicks.AddRange(parameters[parameters.Count - 1].Split(' '));
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnIsOnReply(new IrcMessageEventArgs<IsOnReplyMessage>(this));
    }

  }

}