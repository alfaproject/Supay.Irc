using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Requests information about the nicks supplied in the Nick property.
  /// </summary>
  [Serializable]
  public class UserHostMessage : CommandMessage {

    /// <summary>
    /// Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "USERHOST";
      }
    }

    /// <summary>
    /// Gets the collection of nicks to request information for.
    /// </summary>
    public virtual Collection<string> Nicks {
      get {
        return nicks;
      }
    }

    private Collection<string> nicks = new Collection<string>();

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Nicks, " "));
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Nicks.Clear();
      foreach (string nick in parameters) {
        this.Nicks.Add(nick);
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUserHost(new IrcMessageEventArgs<UserHostMessage>(this));
    }

  }

}