using System;
using System.Collections.Generic;
using System.Linq;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Requests information from the server about the users specified.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Possible reply messages include:
  ///     <see cref="NoSuchServerMessage" />
  ///     <see cref="NoNickGivenMessage" />
  ///     <see cref="NoSuchNickMessage" />
  /// 
  ///     <see cref="WhoIsUserReplyMessage" />
  ///     <see cref="WhoIsChannelsReplyMessage" />
  ///     <see cref="WhoIsServerReplyMessage" />
  ///     <see cref="WhoIsOperReplyMessage" />
  ///     <see cref="WhoIsIdleReplyMessage" />
  /// 
  ///     <see cref="UserAwayMessage" />
  ///     <see cref="WhoIsEndReplyMessage" />
  ///   </para>
  /// </remarks>
  [Serializable]
  public class WhoIsMessage : CommandMessage
  {
    private readonly UserCollection masks = new UserCollection();
    private string server = string.Empty;

    /// <summary>
    ///   Gets the collection of users that information is requested for.
    /// </summary>
    public virtual UserCollection Masks
    {
      get
      {
        return this.masks;
      }
    }

    /// <summary>
    ///   Gets or sets the server which should return the information.
    /// </summary>
    public virtual string Server
    {
      get
      {
        return this.server;
      }
      set
      {
        this.server = value;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "WHOIS";
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
        parameters.Add(this.Server);
        parameters.Add(string.Join(",", this.Masks));
        return parameters;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Masks.Clear();
      this.Server = string.Empty;
      if (parameters.Count >= 1)
      {
        if (parameters.Count > 1)
        {
          this.Server = parameters[0];
        }
        foreach (var user in parameters[parameters.Count - 1].Split(',').Select(mask => new User(mask)))
        {
          this.Masks.Add(user.Nickname, user);
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnWhoIs(new IrcMessageEventArgs<WhoIsMessage>(this));
    }
  }
}
