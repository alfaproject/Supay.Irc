using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   One of the possible replies to a <see cref="WhoIsMessage" /> message.
  /// </summary>
  [Serializable]
  public class WhoIsRegisteredNickReplyMessage : NumericMessage
  {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoIsRegisteredNickReplyMessage" /> class.
    /// </summary>
    public WhoIsRegisteredNickReplyMessage()
      : base(307)
    {
    }

    /// <summary>
    ///   Gets or sets the nick for the user examined.
    /// </summary>
    public virtual string Nick
    {
      get
      {
        return this.nick;
      }
      set
      {
        this.nick = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Nick);
      parameters.Add("has identified for this nick");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      if (parameters.Count == 3)
      {
        this.Nick = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWhoIsRegisteredNickReply(new IrcMessageEventArgs<WhoIsRegisteredNickReplyMessage>(this));
    }
  }
}
