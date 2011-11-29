using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Monitor system message that adds users to your monitor list.
  /// </summary>
  [Serializable]
  public class MonitorAddUsersMessage : MonitorMessage
  {
    private IList<string> nicks;

    /// <summary>
    ///   Gets the collection of nicks being added to the monitor list.
    /// </summary>
    public IList<string> Nicks
    {
      get
      {
        return this.nicks ?? (this.nicks = new Collection<string>());
      }
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage)
    {
      if (!base.CanParse(unparsedMessage))
      {
        return false;
      }
      string firstParam = MessageUtil.GetParameter(unparsedMessage, 0);
      return firstParam.StartsWith("+", StringComparison.Ordinal);
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      string nicksParam = parameters[parameters.Count - 1];
      var splitNicksParam = nicksParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
      foreach (string nick in splitNicksParam)
      {
        this.Nicks.Add(nick);
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
        parameters.Add("+");
        if (this.Nicks != null && this.Nicks.Count != 0)
        {
          parameters.Add(string.Join(",", this.Nicks));
        }
        return parameters;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnMonitorAddUsers(new IrcMessageEventArgs<MonitorAddUsersMessage>(this));
    }
  }
}
