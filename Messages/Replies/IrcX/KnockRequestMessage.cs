using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The notification to the channel that a user has knocked on their channel.
  /// </summary>
  [Serializable]
  public class KnockRequestMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="KnockRequestMessage"/>.
    /// </summary>
    public KnockRequestMessage()
      : base() {
      this.InternalNumeric = 710;
    }

    /// <summary>
    /// Gets or sets the channel that was knocked on.
    /// </summary>
    public virtual string Channel {
      get {
        return this.channel;
      }
      set {
        this.channel = value;
      }
    }
    private string channel = string.Empty;

    /// <summary>
    /// Gets or sets the user which knocked on the channel.
    /// </summary>
    public virtual User Knocker {
      get {
        return this.knocker;
      }
      set {
        this.knocker = value;
      }
    }
    private User knocker = new User();

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add(Knocker.IrcMask);
      parameters.Add("has asked for an invite.");
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.Channel = parameters[1];
      } else {
        this.Channel = string.Empty;
      }
      if (parameters.Count > 2) {
        this.Knocker = new User(parameters[2]);
      } else {
        this.Knocker = new User();
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnKnockRequest(new IrcMessageEventArgs<KnockRequestMessage>(this));
    }

  }

}