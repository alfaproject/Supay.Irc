using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Returned after receiving a <see cref="NickMessage" /> which contains characters which do not fall in the defined set.
  /// </summary>
  [Serializable]
  public class ErroneousNickMessage : ErrorMessage {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="ErroneousNickMessage" /> class.
    /// </summary>
    public ErroneousNickMessage()
      : base(432) {
    }

    /// <summary>
    ///   Gets or sets the nick which wasn't accepted.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("Erroneous nickname");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Nick = parameters.Count > 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnErroneousNick(new IrcMessageEventArgs<ErroneousNickMessage>(this));
    }
  }
}
