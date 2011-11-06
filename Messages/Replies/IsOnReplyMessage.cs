using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The server reply to an <see cref="IsOnMessage" />.
  /// </summary>
  [Serializable]
  public class IsOnReplyMessage : NumericMessage {
    private readonly List<string> nicks = new List<string>();

    /// <summary>
    ///   Creates a new instance of the <see cref="IsOnReplyMessage" /> class.
    /// </summary>
    public IsOnReplyMessage()
      : base(303) {
    }

    /// <summary>
    ///   Gets the list of nicks of people who are known to be online.
    /// </summary>
    public virtual List<string> Nicks {
      get {
        return nicks;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Nicks, " "));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Nicks.Clear();
      Nicks.AddRange(parameters[parameters.Count - 1].Split(' '));
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnIsOnReply(new IrcMessageEventArgs<IsOnReplyMessage>(this));
    }
  }
}
