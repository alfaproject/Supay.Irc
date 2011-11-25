using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  /// ERR_ERRONEUSNICKNAME
  /// --------------------
  /// Returned after receiving a <see cref="NickMessage" /> which contains characters which do not fall in the defined set.
  /// </summary>
  [Serializable]
  public class ErroneusNickMessage : ErrorMessage
  {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="ErroneusNickMessage" /> class.
    /// </summary>
    public ErroneusNickMessage()
      : base(432)
    {
    }

    /// <summary>
    ///   Gets or sets the nick which wasn't accepted.
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
      parameters.Add("Erroneus Nickname");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Nick = parameters.Count > 1 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnErroneusNick(new IrcMessageEventArgs<ErroneusNickMessage>(this));
    }
  }
}
