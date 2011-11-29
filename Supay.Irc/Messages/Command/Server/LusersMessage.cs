using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Requests that the server send information about the size of the IRC network.
  /// </summary>
  [Serializable]
  public class LusersMessage : ServerQueryBase
  {
    private string mask = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "LUSERS";
      }
    }

    /// <summary>
    ///   Gets or sets the mask that limits the servers which information will be returned.
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
    ///   Gets the index of the parameter which holds the server which should respond to the query.
    /// </summary>
    protected override int TargetParsingPosition
    {
      get
      {
        return 1;
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
        if (!string.IsNullOrEmpty(this.Mask))
        {
          parameters.Add(this.Mask);
          parameters.Add(this.Target);
        }
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Mask = parameters.Count >= 1 ? parameters[0] : string.Empty;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnLusers(new IrcMessageEventArgs<LusersMessage>(this));
    }
  }
}
