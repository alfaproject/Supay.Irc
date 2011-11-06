using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The notification to the channel that a user has knocked on their channel.
  /// </summary>
  [Serializable]
  public class KnockRequestMessage : NumericMessage {
    private string channel = string.Empty;
    private User knocker = new User();

    /// <summary>
    ///   Creates a new instance of the <see cref="KnockRequestMessage" />.
    /// </summary>
    public KnockRequestMessage()
      : base(710) {
    }

    /// <summary>
    ///   Gets or sets the channel that was knocked on.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user which knocked on the channel.
    /// </summary>
    public virtual User Knocker {
      get {
        return knocker;
      }
      set {
        knocker = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Knocker.IrcMask);
      parameters.Add("has asked for an invite.");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        Channel = parameters[1];
      } else {
        Channel = string.Empty;
      }
      if (parameters.Count > 2) {
        Knocker = new User(parameters[2]);
      } else {
        Knocker = new User();
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnKnockRequest(new IrcMessageEventArgs<KnockRequestMessage>(this));
    }
  }
}
