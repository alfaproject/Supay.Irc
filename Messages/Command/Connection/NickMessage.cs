using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   NickMessage is used to give a user a nickname or change the previous one. </summary>
  [Serializable]
  public class NickMessage : CommandMessage {

    /// <summary>
    ///   Creates a new instance of the <see cref="NickMessage"/> class. </summary>
    public NickMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="NickMessage"/> class with the given nick. </summary>
    public NickMessage(string newNick)
      : this() {
      NewNick = newNick;
    }

    /// <summary>
    ///   Gets the IRC command associated with this message. </summary>
    protected override string Command {
      get {
        return "NICK";
      }
    }

    /// <summary>
    ///   Gets or sets the nick requested by the sender. </summary>
    /// <remarks>
    ///   Some servers limit you to 9 characters in you nick, while others allow more. Some servers
    ///   will send a <see cref="SupportMessage"/> telling you the maximum nick length allowed. </remarks>
    public string NewNick {
      get;
      set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      if (!string.IsNullOrEmpty(NewNick)) {
        parameters.Add(NewNick);
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message. </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 1) {
        NewNick = parameters[0];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the
    ///   current <see cref="IrcMessage"/> subclass. </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnNickChange(new IrcMessageEventArgs<NickMessage>(this));
    }

  } //class NickMessage
} //namespace Supay.Irc.Messages