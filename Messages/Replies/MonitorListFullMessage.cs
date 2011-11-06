using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Monitor system error message informing the user which nicks couldn't be added 
  ///   to their monitor list because it is full.
  /// </summary>
  [Serializable]
  public class MonitorListFullMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="MonitorListFullMessage" />.
    /// </summary>
    public MonitorListFullMessage()
      : base(734) {
    }

    /// <summary>
    ///   Gets or sets the limit of monitor lists on the server.
    /// </summary>
    public int Limit {
      get {
        return limit;
      }
      set {
        limit = value;
      }
    }

    private int limit;

    /// <summary>
    ///   Gets the collection of nicks which couldn't be added to the monitor list.
    /// </summary>
    public Collection<string> Nicks {
      get {
        if (nicks == null) {
          nicks = new Collection<string>();
        }
        return nicks;
      }
    }

    private Collection<string> nicks;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Limit.ToString(CultureInfo.InvariantCulture));
      parameters.Add(MessageUtil.CreateList(Nicks, ","));
      parameters.Add("Monitor list is full.");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Limit = -1;
      Nicks.Clear();

      if (parameters.Count > 2) {
        Limit = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);

        string userListParam = parameters[2];
        string[] userList = userListParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string nick in userList) {
          Nicks.Add(nick);
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnMonitorListFull(new IrcMessageEventArgs<MonitorListFullMessage>(this));
    }
  }
}
