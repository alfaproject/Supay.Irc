using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Returned by a server to a client when it detects a nickname collision.
  /// </summary>
  [Serializable]
  public class NickCollisionMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NickCollisionMessage"/> class.
    /// </summary>
    public NickCollisionMessage()
      : base(436) {
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
      parameters.Add("Nickname collision KILL");
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
      conduit.OnNickCollision(new IrcMessageEventArgs<NickCollisionMessage>(this));
    }

  }

}