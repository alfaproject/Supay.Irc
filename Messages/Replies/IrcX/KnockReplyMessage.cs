using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply to a <see cref="KnockMessage" />.
  /// </summary>
  [Serializable]
  public class KnockReplyMessage : NumericMessage {
    private string channel = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="KnockReplyMessage" />.
    /// </summary>
    public KnockReplyMessage()
      : base(711) {
    }

    /// <summary>
    ///   Gets or sets the channel that was knocked on.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("Your KNOCK has been delivered.");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Channel = parameters.Count > 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnKnockReply(new IrcMessageEventArgs<KnockReplyMessage>(this));
    }
  }
}
