using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This message is sent to all users with <see cref="Supay.Irc.Messages.Modes.ReceiveWallopsMode" />,
  ///   <see cref="Supay.Irc.Messages.Modes.NetworkOperatorMode" />, or <see cref="Supay.Irc.Messages.Modes.ServerOperatorMode" /> user modes.
  /// </summary>
  [Serializable]
  public class WallopsMessage : CommandMessage {
    /// <summary>
    ///   Gets or sets the text of the <see cref="WallopsMessage" />.
    /// </summary>
    public virtual string Text {
      get {
        return this.text;
      }
      set {
        this.text = value;
      }
    }

    private string text = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "WALLOPS";
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 1) {
        this.Text = parameters[0];
      } else {
        this.Text = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWallops(new IrcMessageEventArgs<WallopsMessage>(this));
    }
  }
}
