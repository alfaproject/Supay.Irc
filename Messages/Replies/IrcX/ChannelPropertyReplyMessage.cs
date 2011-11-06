using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply to a <see cref="ChannelPropertyMessage" /> designed to read one or all channel properties.
  /// </summary>
  [Serializable]
  public class ChannelPropertyReplyMessage : NumericMessage, IChannelTargetedMessage {
    private string channel = string.Empty;
    private string propValue = string.Empty;
    private string property = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelPropertyReplyMessage" />.
    /// </summary>
    public ChannelPropertyReplyMessage()
      : base(818) {
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

    /// <summary>
    ///   Gets or sets the name of the channel property being referenced.
    /// </summary>
    public virtual string Prop {
      get {
        return property;
      }
      set {
        property = value;
      }
    }

    /// <summary>
    ///   Gets or sets the value of the channel property.
    /// </summary>
    public virtual string Value {
      get {
        return propValue;
      }
      set {
        propValue = value;
      }
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Prop);
      parameters.Add(Value);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Channel = string.Empty;
      Prop = string.Empty;
      Value = string.Empty;

      if (parameters.Count > 1) {
        Channel = parameters[1];
        if (parameters.Count > 2) {
          Prop = parameters[2];
          if (parameters.Count > 3) {
            Value = parameters[3];
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnChannelPropertyReply(new IrcMessageEventArgs<ChannelPropertyReplyMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Channel.EqualsI(channelName);
    }
  }
}
