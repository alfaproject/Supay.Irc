using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The <see cref="ErrorMessage"/> received when the client attempts to add a nick to his accept list
  /// when that nick is already on the list.
  /// </summary>
  [Serializable]
  public class AcceptAlreadyExistsMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="AcceptAlreadyExistsMessage"/> class.
    /// </summary>
    public AcceptAlreadyExistsMessage()
      : base(457) {
    }

    /// <summary>
    /// Gets or sets the nick which wasn't added
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
      parameters.Add("is already on your accept list");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(StringCollection parameters) {
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
      conduit.OnAcceptAlreadyExists(new IrcMessageEventArgs<AcceptAlreadyExistsMessage>(this));
    }

  }

}