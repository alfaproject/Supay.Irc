using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The UserModeMessage allows users to have their mode changed.
  /// </summary>
  /// <remarks>
  /// Modes include such things as invisibility and IRC operator.
  /// This message wraps the MODE command.
  /// </remarks>
  [Serializable]
  public class UserModeMessage : CommandMessage {
    /// <summary>
    /// Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "MODE";
      }
    }

    /// <summary>
    /// Gets or sets the affected user.
    /// </summary>
    public virtual string User {
      get {
        return this.user;
      }
      set {
        this.user = value;
      }
    }
    private string user = string.Empty;

    /// <summary>
    /// Gets or sets the mode changes being applied.
    /// </summary>
    /// <remarks>
    /// An example ModeChanges might look like "-w".
    /// This example means turning off the receipt of wallop message from the server.
    /// </remarks>
    public virtual string ModeChanges {
      get {
        return this.modeChanges;
      }
      set {
        this.modeChanges = value;
      }
    }
    private string modeChanges = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(User);
      parameters.Add(ModeChanges);
      return parameters;
    }

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }

      Collection<string> p = MessageUtil.GetParameters(unparsedMessage);
      if (p.Count >= 1) {
        if (!MessageUtil.HasValidChannelPrefix(p[0])) {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.User = parameters[0];
        this.ModeChanges = parameters[1];
      } else {
        this.User = string.Empty;
        this.ModeChanges = string.Empty;
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUserMode(new IrcMessageEventArgs<UserModeMessage>(this));
    }

  }

}