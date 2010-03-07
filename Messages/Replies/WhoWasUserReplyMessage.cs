using System;
using System.Collections.ObjectModel;
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
      : base(314) {
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
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(User.Nickname);
      parameters.Add(User.Username);
      parameters.Add(User.Host);
      parameters.Add("*");
      parameters.Add(User.Name);
      return parameters;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.user = new User();
      if (parameters.Count > 5) {
        user.Nickname = parameters[1];
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