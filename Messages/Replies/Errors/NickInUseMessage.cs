using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Returned when a <see cref="NickMessage" /> is processed that results in an attempt to change to a currently existing nickname.
  /// </summary>
  [Serializable]
  public class NickInUseMessage : ErrorMessage
  {
    private string nick = string.Empty;

    /// <summary>
    ///   Creates a new instances of the <see cref="NickInUseMessage" /> class.
    /// </summary>
    public NickInUseMessage()
      : base(433)
    {
    }

    /// <summary>
    ///   Gets or sets the nick which was already taken.
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
      parameters.Add("Nickname is already in use.");
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
      conduit.OnNickInUse(new IrcMessageEventArgs<NickInUseMessage>(this));
    }
  }
}
