using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Requests information about a user who is no longer connected to IRC.
  /// </summary>
  [Serializable]
  public class WhoWasMessage : CommandMessage {

    /// <summary>
    /// Gets or sets the nick of the user being examined.
    /// </summary>
    public virtual string Nick {
      get {
        return this.nick;
      }
      set {
        this.nick = value;
      }
    }

    /// <summary>
    /// Gets or sets the server that should search for the information.
    /// </summary>
    public virtual string Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum number of results to receive.
    /// </summary>
    public virtual int MaximumResults {
      get {
        return maximumResults;
      }
      set {
        maximumResults = value;
      }
    }

    /// <summary>
    /// Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "WHOWAS";
      }
    }

    private string nick = string.Empty;
    private string server = string.Empty;
    private int maximumResults = 1;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      if (MaximumResults > 0) {
        parameters.Add(MaximumResults.ToString(CultureInfo.InvariantCulture));
        if (!string.IsNullOrEmpty(Server)) {
          parameters.Add(Server);
        }
      }
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      this.Nick = string.Empty;
      this.Server = string.Empty;
      this.MaximumResults = 1;

      if (parameters.Count > 0) {
        this.Nick = parameters[0];
        if (parameters.Count > 1) {
          this.Server = parameters[1];
          if (parameters.Count > 2) {
            this.MaximumResults = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);
          }
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWhoWas(new IrcMessageEventArgs<WhoWasMessage>(this));
    }
  }

}