using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply to a <see cref="KnockMessage" />.
  /// </summary>
  [Serializable]
  public class KnockReplyMessage : NumericMessage {
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
        return this.channel;
      }
      set {
        this.channel = value;
      }
    }

    private string channel = string.Empty;

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
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      } else {
        this.Channel = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnKnockReply(new IrcMessageEventArgs<KnockReplyMessage>(this));
    }
  }
}
