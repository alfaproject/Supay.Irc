using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The IsOnMessage provides a quick and efficient means to get a response about whether a given nickname is currently on IRC.
  /// </summary>
  /// <remarks>
  ///   The server will reply with a <see cref="IsOnReplyMessage" />.
  /// </remarks>
  [Serializable]
  public class IsOnMessage : CommandMessage
  {
    private readonly List<string> nicks = new List<string>();

    /// <summary>
    ///   Creates a new instance of the IsOnMessage class.
    /// </summary>
    public IsOnMessage()
    {
    }

    /// <summary>
    ///   Creates a new instance of the IsOnMessage class with the given nicks.
    /// </summary>
    /// <param name="nicks"></param>
    public IsOnMessage(params string[] nicks)
    {
      this.nicks.AddRange(nicks);
    }

    /// <summary>
    ///   Gets the collection of nicks to query for.
    /// </summary>
    public List<string> Nicks
    {
      get
      {
        return this.nicks;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "ISON";
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(string.Join(" ", this.Nicks));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      if (parameters.Count > 0)
      {
        string nickParam = parameters[0];
        this.Nicks.AddRange(nickParam.Split(' '));
      }
      else
      {
        this.Nicks.Clear();
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnIsOn(new IrcMessageEventArgs<IsOnMessage>(this));
    }
  }
}