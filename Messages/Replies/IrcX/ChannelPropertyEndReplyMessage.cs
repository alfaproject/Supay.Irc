using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Marks the end of the replies to a <see cref="ChannelPropertyMessage" /> designed to read one or all channel properties.
  /// </summary>
  [Serializable]
  public class ChannelPropertyEndReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelPropertyEndReplyMessage" /> class.
    /// </summary>
    public ChannelPropertyEndReplyMessage()
      : base(819) {
    }

    /// <summary>
    ///   Gets or sets channel being referenced.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("End of properties");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        Channel = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelPropertyEndReply(new IrcMessageEventArgs<ChannelPropertyEndReplyMessage>(this));
    }
  }
}
