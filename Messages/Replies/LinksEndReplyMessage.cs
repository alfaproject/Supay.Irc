using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Marks the end of the replies to the <see cref="LinksMessage" /> query.
  /// </summary>
  [Serializable]
  public class LinksEndReplyMessage : NumericMessage
  {
    private string mask = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="LinksEndReplyMessage" /> class.
    /// </summary>
    public LinksEndReplyMessage()
      : base(365)
    {
    }

    /// <summary>
    ///   Gets or sets the server mask that the links list used.
    /// </summary>
    public virtual string Mask
    {
      get
      {
        return this.mask;
      }
      set
      {
        this.mask = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Mask);
      parameters.Add("End of /LINKS list");
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Mask = parameters.Count == 3 ? parameters[1] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnLinksEndReply(new IrcMessageEventArgs<LinksEndReplyMessage>(this));
    }
  }
}
