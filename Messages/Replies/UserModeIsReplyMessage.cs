using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This is the reply to an empty <see cref="UserModeMessage" />.
  /// </summary>
  [Serializable]
  public class UserModeIsReplyMessage : NumericMessage {
    private string modes = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="UserModeIsReplyMessage" /> class.
    /// </summary>
    public UserModeIsReplyMessage()
      : base(221) {
    }

    /// <summary>
    ///   Gets or sets the modes in effect.
    /// </summary>
    /// <remarks>
    ///   An example Modes might look like "+i".
    /// </remarks>
    public virtual string Modes {
      get {
        return modes;
      }
      set {
        modes = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Modes);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Modes = parameters.Count >= 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnUserModeIsReply(new IrcMessageEventArgs<UserModeIsReplyMessage>(this));
    }
  }
}
