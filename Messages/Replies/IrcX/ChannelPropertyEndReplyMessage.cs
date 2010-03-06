using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Marks the end of the replies to a <see cref="ChannelPropertyMessage"/> designed to read one or all channel properties.
  /// </summary>
  [Serializable]
  public class ChannelPropertyEndReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelPropertyEndReplyMessage"/> class.
    /// </summary>
    public ChannelPropertyEndReplyMessage()
      : base() {
      this.InternalNumeric = 819;
    }

    /// <summary>
    /// Gets or sets channel being referenced.
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
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("End of properties");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelPropertyEndReply(new IrcMessageEventArgs<ChannelPropertyEndReplyMessage>(this));
    }

  }

}