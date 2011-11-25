using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Returned by the server to indicate that the client must be registered before the server will allow it to be parsed in detail.
  /// </summary>
  [Serializable]
  public class NotRegisteredMessage : ErrorMessage
  {
    /// <summary>
    ///   Creates a new instances of the <see cref="NotRegisteredMessage" /> class.
    /// </summary>
    public NotRegisteredMessage()
      : base(451)
    {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add("You have not registered");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNotRegistered(new IrcMessageEventArgs<NotRegisteredMessage>(this));
    }
  }
}
