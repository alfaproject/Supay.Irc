using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The WelcomeMessage is sent from a server to a client as the first message 
  ///   once the client is registered.
  /// </summary>
  [Serializable]
  public class WelcomeMessage : NumericMessage
  {
    private string text = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="WelcomeMessage" /> class.
    /// </summary>
    public WelcomeMessage()
      : base(001)
    {
    }

    /// <summary>
    ///   The content of the welcome message.
    /// </summary>
    public virtual string Text
    {
      get
      {
        return this.text;
      }
      set
      {
        this.text = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Text = parameters.Count == 2 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWelcome(new IrcMessageEventArgs<WelcomeMessage>(this));
    }
  }
}
