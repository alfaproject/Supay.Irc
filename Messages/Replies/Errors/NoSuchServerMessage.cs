using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Used to indicate the server name given currently doesn't exist.
  /// </summary>
  [Serializable]
  public class NoSuchServerMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="NoSuchServerMessage"/> class.
    /// </summary>
    public NoSuchServerMessage()
      : base() {
      this.InternalNumeric = 402;
    }

    /// <summary>
    /// Gets or sets the nick which wasn't accepted.
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
      parameters.Add("No such server");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
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
      conduit.OnNoSuchServer(new IrcMessageEventArgs<NoSuchServerMessage>(this));
    }

  }

}