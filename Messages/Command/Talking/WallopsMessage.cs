using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This message is sent to all users with <see cref="Supay.Irc.Messages.Modes.ReceiveWallopsMode" />,
  ///   <see cref="Supay.Irc.Messages.Modes.NetworkOperatorMode" />, or <see cref="Supay.Irc.Messages.Modes.ServerOperatorMode" /> user modes.
  /// </summary>
  [Serializable]
  public class WallopsMessage : CommandMessage {
    private string text = string.Empty;

    /// <summary>
    ///   Gets or sets the text of the <see cref="WallopsMessage" />.
    /// </summary>
    public virtual string Text {
      get {
        return text;
      }
      set {
        text = value;
      }
    }

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
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Text = parameters.Count >= 1 ? parameters[0] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWallops(new IrcMessageEventArgs<WallopsMessage>(this));
    }
  }
}
