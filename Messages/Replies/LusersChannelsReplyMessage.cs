using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   One of the responses to the <see cref="LusersMessage" /> query.
  /// </summary>
  [Serializable]
  public class LusersChannelsReplyMessage : NumericMessage {
    private const string channelsFormed = "channels formed";
    private int channelCount = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="LusersChannelsReplyMessage" /> class.
    /// </summary>
    public LusersChannelsReplyMessage()
      : base(254) {
    }

    /// <summary>
    ///   Gets or sets the number of channels available.
    /// </summary>
    public virtual int ChannelCount {
      get {
        return channelCount;
      }
      set {
        channelCount = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(ChannelCount.ToString(CultureInfo.InvariantCulture));
      parameters.Add(channelsFormed);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      ChannelCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnLusersChannelsReply(new IrcMessageEventArgs<LusersChannelsReplyMessage>(this));
    }
  }
}
