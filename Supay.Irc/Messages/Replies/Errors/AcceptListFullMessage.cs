using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a user adds too many users to his accept list.
  /// </summary>
  [Serializable]
  public class AcceptListFullMessage : ErrorMessage
  {
    /// <summary>
    ///   Creates a new instances of the <see cref="BanListFullMessage" /> class.
    /// </summary>
    public AcceptListFullMessage()
      : base(456)
    {
    }

    /// <exclude />
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add("Accept list is full");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnAcceptListFull(new IrcMessageEventArgs<AcceptListFullMessage>(this));
    }
  }
}
