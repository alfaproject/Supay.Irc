using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Monitor system notification that contains a list of nicks.
  /// </summary>
  [Serializable]
  public abstract class MonitoredNicksListMessage : NumericMessage
  {
    protected MonitoredNicksListMessage(int number)
      : base(number)
    {
      this.Nicks = new Collection<string>();
    }

    /// <summary>
    ///   Gets the collection of nicks of users for the message.
    /// </summary>
    public Collection<string> Nicks
    {
      get;
      private set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(string.Join(",", this.Nicks));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Nicks.Clear();
      if (parameters.Count > 1)
      {
        string userListParam = parameters[1];
        var userList = userListParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string nick in userList)
        {
          this.Nicks.Add(nick);
        }
      }
    }
  }
}
