using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// With the KnockMessage, clients can request an invite to a invitation-only channel.
  /// </summary>
  [Serializable]
  public class KnockMessage : CommandMessage, IChannelTargetedMessage {

    /// <summary>
    /// Creates a new instance of the KnockMessage class.
    /// </summary>
    public KnockMessage()
      : base() {
    }

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "KNOCK";
      }
    }

    /// <summary>
    /// Gets or sets the channel being targeted.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }
    private string channel = "";

    /// <summary>
    /// Validates this message's properties according to the given <see cref="ServerSupport"/>.
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport == null) {
        return;
      }
      this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
      if (!serverSupport.Knock) {
        System.Diagnostics.Trace.WriteLine("Knock Is Not Supported On This Server");
      }
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.Channel);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      if (parameters.Count > 0) {
        this.Channel = parameters[0];
      } else {
        this.Channel = "";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnKnock(new IrcMessageEventArgs<KnockMessage>(this));
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