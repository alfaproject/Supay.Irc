using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// Returned when a <see cref="NickMessage"/> is processed that results in an attempt to change to a currently existing nickname. 
  /// </summary>
  [Serializable]
  public class NickInUseMessage : ErrorMessage {
    /// <summary>
    /// Creates a new instances of the <see cref="NickInUseMessage"/> class.
    /// </summary>
    public NickInUseMessage()
      : base(433) {
    }

    /// <summary>
    /// Gets or sets the nick which was already taken.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    private string nick = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add("Nickname is already in use.");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Nick = parameters[1];
      } else {
        this.Nick = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnNickInUse(new IrcMessageEventArgs<NickInUseMessage>(this));
    }
  }
}
