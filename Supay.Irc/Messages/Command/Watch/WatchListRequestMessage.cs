using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Watch system message that requests the list of nicks currently being watched.
  /// </summary>
  [Serializable]
  public class WatchListRequestMessage : WatchMessage
  {
    #region Properties

    private bool onlineOnly;

    /// <summary>
    ///   Gets or sets if the message requests that only online contacts are in the list.
    /// </summary>
    public bool OnlineOnly
    {
      get
      {
        return this.onlineOnly;
      }
      set
      {
        this.onlineOnly = value;
      }
    }

    #endregion

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
      return param.Count == 0 || (param.Count == 1 && (param[0] == "l" || param[0] == "L"));
    }

    /// <summary>
    ///   Overrides <see href = "IrcMessage.ParseParameters" />
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.OnlineOnly = parameters.Count == 0 || parameters[0] == "l";
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
        parameters.Add(this.OnlineOnly ? "l" : "L");
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
      conduit.OnWatchListRequest(new IrcMessageEventArgs<WatchListRequestMessage>(this));
    }

    #endregion
  }
}
