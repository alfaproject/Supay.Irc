using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The reply to the <see cref="SilenceMessage" /> query.
  /// </summary>
  [Serializable]
  public class SilenceReplyMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="SilenceReplyMessage" />.
    /// </summary>
    public SilenceReplyMessage()
      : base(271) {
    }

    /// <summary>
    ///   Gets or sets the user being silenced.
    /// </summary>
    public virtual User SilencedUser {
      get {
        return silencedUser;
      }
      set {
        silencedUser = value;
      }
    }

    private User silencedUser = new User();

    /// <summary>
    ///   Gets or sets the nick of the owner of the silence list
    /// </summary>
    public virtual string SilenceListOwner {
      get {
        return silenceListOwner;
      }
      set {
        silenceListOwner = value;
      }
    }

    private string silenceListOwner = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(SilenceListOwner);
      parameters.Add(SilencedUser.IrcMask);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 2) {
        SilenceListOwner = parameters[1];
        SilencedUser = new User(parameters[2]);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnSilenceReply(new IrcMessageEventArgs<SilenceReplyMessage>(this));
    }
  }
}
