using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Requests information about the given user or users.
  /// </summary>
  [Serializable]
  public class WhoMessage : CommandMessage {
    /// <summary>
    ///   Gets or sets the mask which is matched for users to return information about.
    /// </summary>
    public virtual User Mask {
      get {
        return mask;
      }
      set {
        mask = value;
      }
    }

    private User mask = new User();

    /// <summary>
    ///   Gets or sets if the results should only contain IRC operators.
    /// </summary>
    public virtual bool RestrictToOps {
      get {
        return restrictToOps;
      }
      set {
        restrictToOps = value;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "WHO";
      }
    }

    private bool restrictToOps = false;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Mask.ToString());
      if (RestrictToOps) {
        parameters.Add("o");
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Mask = new User();
      if (parameters.Count >= 1) {
        Mask.Nickname = parameters[0];
        RestrictToOps = (parameters.Count > 1 && parameters[1] == "o");
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWho(new IrcMessageEventArgs<WhoMessage>(this));
    }
  }
}
