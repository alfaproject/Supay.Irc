using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Message that requests the status of the users on your watch list.
  /// </summary>
  [Serializable]
  public class WatchStatusRequestMessage : WatchMessage
  {
    #region Parsing

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage)
    {
      if (!base.CanParse(unparsedMessage))
      {
        return false;
      }
      IList<string> param = MessageUtil.GetParameters(unparsedMessage);
      return param.Count == 1 && param[0].Equals("S", StringComparison.Ordinal);
    }

    #endregion

    #region Formatting

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add("S");
        return parameters;
      }
    }

    #endregion

    #region Events

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWatchStatusRequest(new IrcMessageEventArgs<WatchStatusRequestMessage>(this));
    }

    #endregion
  }
}
