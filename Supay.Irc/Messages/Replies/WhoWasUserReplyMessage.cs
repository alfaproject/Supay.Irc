using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The reply to a <see cref="WhoWasMessage" /> query.
  /// </summary>
  [Serializable]
  public class WhoWasUserReplyMessage : NumericMessage
  {
    private User user = new User();

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoWasUserReplyMessage" /> class.
    /// </summary>
    public WhoWasUserReplyMessage()
      : base(314)
    {
    }

    /// <summary>
    ///   Gets or sets the User being examined.
    /// </summary>
    public virtual User User
    {
      get
      {
        return this.user;
      }
      set
      {
        this.user = value;
      }
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.Tokens"/>.
    /// </summary>
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.User.Nickname);
        parameters.Add(this.User.Username);
        parameters.Add(this.User.Host);
        parameters.Add("*");
        parameters.Add(this.User.Name);
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.user = new User();
      if (parameters.Count > 5)
      {
        this.user.Nickname = parameters[1];
        this.user.Username = parameters[2];
        this.user.Host = parameters[3];
        this.user.Name = parameters[5];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWhoWasUserReply(new IrcMessageEventArgs<WhoWasUserReplyMessage>(this));
    }
  }
}
