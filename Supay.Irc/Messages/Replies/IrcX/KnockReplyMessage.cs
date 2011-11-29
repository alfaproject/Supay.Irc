using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A reply to a <see cref="KnockMessage" />.
  /// </summary>
  [Serializable]
  public class KnockReplyMessage : NumericMessage
  {
    private string channel = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="KnockReplyMessage" />.
    /// </summary>
    public KnockReplyMessage()
      : base(711)
    {
    }

    /// <summary>
    ///   Gets or sets the channel that was knocked on.
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
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.Channel);
        parameters.Add("Your KNOCK has been delivered.");
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Channel = parameters.Count > 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnKnockReply(new IrcMessageEventArgs<KnockReplyMessage>(this));
    }
  }
}
