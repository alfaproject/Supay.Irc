using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Sent to a user who sends a <see cref="PingMessage" /> which doesn't have a valid origin.
  /// </summary>
  [Serializable]
  public class NoPingOriginSpecifiedMessage : ErrorMessage
  {
    /// <summary>
    ///   Creates a new instances of the <see cref="NoPingOriginSpecifiedMessage" /> class.
    /// </summary>
    public NoPingOriginSpecifiedMessage()
      : base(409)
    {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add("No origin specified");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNoPingOriginSpecified(new IrcMessageEventArgs<NoPingOriginSpecifiedMessage>(this));
    }
  }
}
