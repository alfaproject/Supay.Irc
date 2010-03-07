using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A reply to a <see cref="ChannelPropertyMessage"/> designed to read one or all channel properties.
  /// </summary>
  [Serializable]
  public class ChannelPropertyReplyMessage : NumericMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ChannelPropertyReplyMessage"/>.
    /// </summary>
    public ChannelPropertyReplyMessage()
      : base(818) {
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
    /// Gets or sets the name of the channel property being referenced.
    /// </summary>
    public virtual string Prop {
      get {
        return this.property;
      }
      set {
        this.property = value;
      }
    }
    private string property = string.Empty;

    /// <summary>
    /// Gets or sets the value of the channel property.
    /// </summary>
    public virtual string Value {
      get {
        return this.propValue;
      }
      set {
        this.propValue = value;
      }
    }
    private string propValue = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Prop);
      parameters.Add(Value);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      this.Channel = string.Empty;
      this.Prop = string.Empty;
      this.Value = string.Empty;

      if (parameters.Count > 1) {
        this.Channel = parameters[1];
        if (parameters.Count > 2) {
          this.Prop = parameters[2];
          if (parameters.Count > 3) {
            this.Value = parameters[3];
          }
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnChannelPropertyReply(new IrcMessageEventArgs<ChannelPropertyReplyMessage>(this));
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }

}