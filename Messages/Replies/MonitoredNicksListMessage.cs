using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   A Monitor system notification that contains a list of nicks. </summary>
  [Serializable]
  public abstract class MonitoredNicksListMessage : NumericMessage {

    protected MonitoredNicksListMessage(int number)
      : base(number) {
      Nicks = new StringCollection();
    }

    /// <summary>
    ///   Gets the collection of nicks of users for the message. </summary>
    public StringCollection Nicks {
      get;
      private set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Nicks, ","));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      Nicks.Clear();
      if (parameters.Count > 1) {
        string userListParam = parameters[1];
        string[] userList = userListParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string nick in userList) {
          Nicks.Add(nick);
        }
      }
    }

  } //class MonitoredNicksListMessage
} //namespace Supay.Irc.Messages