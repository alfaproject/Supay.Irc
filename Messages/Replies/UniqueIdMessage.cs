using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   This message is sent by the server early during connection, and tells the user the alpha-numeric id the server uses to identify the user.
  /// </summary>
  [Serializable]
  public class UniqueIdMessage : NumericMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="UniqueIdMessage" /> class.
    /// </summary>
    public UniqueIdMessage()
      : base(042) {
    }

    /// <summary>
    ///   Gets or sets the alpha-numeric id the server uses to identify the client.
    /// </summary>
    public string UniqueId {
      get;
      set;
    }

    private const string yourUniqueID = "your unique ID";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Target);
      parameters.Add(UniqueId);
      parameters.Add(yourUniqueID);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count == 4) {
        UniqueId = parameters[2];
      } else {
        UniqueId = string.Empty;
        Trace.WriteLine("Unknown format of UniqueIDMessage parameters: '" + MessageUtil.ParametersToString(parameters) + "'");
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnUniqueId(new IrcMessageEventArgs<UniqueIdMessage>(this));
    }
  }
}
