using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   With the KnockMessage, clients can request an invite to a invitation-only channel.
  /// </summary>
  [Serializable]
  public class KnockMessage : CommandMessage, IChannelTargetedMessage
  {
    private string channel = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "KNOCK";
      }
    }

    /// <summary>
    ///   Gets or sets the channel being targeted.
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

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    /// <summary>
    ///   Validates this message's properties according to the given <see cref="ServerSupport" />.
    /// </summary>
    public override void Validate(ServerSupport serverSupport)
    {
      base.Validate(serverSupport);
      if (serverSupport == null)
      {
        return;
      }
      this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
      if (!serverSupport.Knock)
      {
        Trace.WriteLine("Knock Is Not Supported On This Server");
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Channel);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Channel = parameters.Count > 0 ? parameters[0] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnKnock(new IrcMessageEventArgs<KnockMessage>(this));
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Channel.Equals(channelName, StringComparison.OrdinalIgnoreCase);
    }
  }
}
