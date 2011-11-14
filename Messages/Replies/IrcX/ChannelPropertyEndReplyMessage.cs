using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Marks the end of the replies to a <see cref="ChannelPropertyMessage" /> designed to read one or all channel properties.
  /// </summary>
  [Serializable]
  public class ChannelPropertyEndReplyMessage : NumericMessage
  {
    private string channel = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ChannelPropertyEndReplyMessage" /> class.
    /// </summary>
    public ChannelPropertyEndReplyMessage()
      : base(819)
    {
    }

    /// <summary>
    ///   Gets or sets channel being referenced.
    /// </summary>
    public virtual string Channel
    {
      get
      {
        return this.channel;
      }
      set
      {
        this.channel = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Channel);
      parameters.Add("End of properties");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      if (parameters.Count > 1)
      {
        this.Channel = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelPropertyEndReply(new IrcMessageEventArgs<ChannelPropertyEndReplyMessage>(this));
    }
  }
}
