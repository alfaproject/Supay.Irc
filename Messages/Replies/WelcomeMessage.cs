using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  /// The WelcomeMessage is sent from a server to a client as the first message 
  /// once the client is registered.
  /// </summary>
  [Serializable]
  public class WelcomeMessage : NumericMessage {
    /// <summary>
    /// Creates a new instance of the <see cref="WelcomeMessage"/> class.
    /// </summary>
    public WelcomeMessage()
      : base(001) {
    }

    /// <summary>
    /// The content of the welcome message.
    /// </summary>
    public virtual string Text {
      get {
        return text;
      }
      set {
        text = value;
      }
    }

    private string text = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 2) {
        this.Text = parameters[1];
      } else {
        this.Text = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWelcome(new IrcMessageEventArgs<WelcomeMessage>(this));
    }
  }
}
