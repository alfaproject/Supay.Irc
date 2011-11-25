using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is a request that the target of the message reply with a human-readable
  ///   list stating what Ctcp commands they support.
  /// </summary>
  [Serializable]
  public class ClientInfoRequestMessage : CtcpRequestMessage
  {
    private readonly ICollection<string> parameters = new Collection<string>();

    /// <summary>
    ///   Creates a new instance of the <see cref="ClientInfoRequestMessage" /> class
    /// </summary>
    public ClientInfoRequestMessage()
    {
      this.InternalCommand = "CLIENTINFO";
    }

    /// <summary>
    ///   Gets the list of parameters which signify interest in a specific command or subcommand.
    /// </summary>
    /// <remarks>
    ///   To specificly ask about support for the "TIME" command, add "TIME" as the first parameter.
    /// </remarks>
    public virtual ICollection<string> Parameters
    {
      get
      {
        return this.parameters;
      }
    }

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        return string.Join(" ", this.Parameters);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnClientInfoRequest(new IrcMessageEventArgs<ClientInfoRequestMessage>(this));
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      this.Parameters.Clear();
      string paramsList = CtcpUtil.GetExtendedData(unparsedMessage);
      foreach (var param in paramsList.Split(' '))
      {
        this.Parameters.Add(param);
      }
    }
  }
}
