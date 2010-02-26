using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The reply to a <see cref="WhoWasMessage"/> query.
  /// </summary>
  [Serializable]
  public class WhoWasUserReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="WhoWasUserReplyMessage"/> class.
    /// </summary>
    public WhoWasUserReplyMessage()
      : base() {
      this.InternalNumeric = 314;
    }

    /// <summary>
    /// Gets or sets the User being examined.
    /// </summary>
    public virtual User User {
      get {
        return user;
      }
      set {
        user = value;
      }
    }

    private User user = new User();


    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.User.Nick);
      writer.AddParameter(this.User.Username);
      writer.AddParameter(this.User.Host);
      writer.AddParameter("*");
      writer.AddParameter(this.User.Name);
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.user = new User();
      if (parameters.Count > 5) {
        user.Nick = parameters[1];
        user.Username = parameters[2];
        user.Host = parameters[3];
        user.Name = parameters[5];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnWhoWasUserReply(new IrcMessageEventArgs<WhoWasUserReplyMessage>(this));
    }

  }

}