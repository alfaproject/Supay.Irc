using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Returned to a client which attempts to register with a server which has not been set up to allow connections from which the host attempted connection.
  /// </summary>
  [Serializable]
  public class NoHostPermissionMessage : ErrorMessage
  {
    /// <summary>
    ///   Creates a new instances of the <see cref="NoHostPermissionMessage" /> class.
    /// </summary>
    public NoHostPermissionMessage()
      : base(463)
    {
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add("Your host isn't among the privileged");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNoHostPermission(new IrcMessageEventArgs<NoHostPermissionMessage>(this));
    }
  }
}
