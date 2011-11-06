using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
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
  public class WhoIsMessage : CommandMessage {
    /// <summary>
    ///   Gets the collection of users that information is requested for.
    /// </summary>
    public virtual UserCollection Masks {
      get {
        return this.masks;
      }
    }

    private UserCollection masks = new UserCollection();

    /// <summary>
    ///   Gets or sets the server which should return the information.
    /// </summary>
    public virtual string Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "WHOIS";
      }
    }

    private string server = string.Empty;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Server);
      parameters.Add(MessageUtil.CreateList(Masks, ","));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Masks.Clear();
      this.Server = string.Empty;
      if (parameters.Count >= 1) {
        if (parameters.Count > 1) {
          this.Server = parameters[0];
        }
        foreach (string maskString in parameters[parameters.Count - 1].Split(',')) {
          this.Masks.Add(new User(maskString));
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoIs(new IrcMessageEventArgs<WhoIsMessage>(this));
    }
  }
}
