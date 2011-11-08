using System;
using System.Collections.Generic;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A CallerId/Accept system message that requests the nicks of the users on your accept list.
  /// </summary>
  [Serializable]
  public class AcceptListRequestMessage : CommandMessage {
    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "ACCEPT";
      }
    }

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport != null && !serverSupport.CallerId) {
        throw new InvalidMessageException(Resources.ServerDoesNotSupportAccept);
      }
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      string firstParam = MessageUtil.GetParameter(unparsedMessage, 0);
      return (firstParam == "*");
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add("*");
      return parameters;
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnAcceptListRequest(new IrcMessageEventArgs<AcceptListRequestMessage>(this));
    }
  }
}
