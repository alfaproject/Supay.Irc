using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {
  
  /// <summary>
  /// Signifies the start of the MOTD sent by the server.
  /// </summary>
  [Serializable]
  public class MotdStartReplyMessage : NumericMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="MotdStartReplyMessage"/> class.
    /// </summary>
    public MotdStartReplyMessage()
      : base() {
      this.InternalNumeric = 375;
    }

    private string info;

    /// <summary>
    /// Gets or sets the info included in the message.
    /// </summary>
    public string Info {
      get {
        return info;
      }
      set {
        info = value;
      }
    }


    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Info);
      return parameters;
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.ParseParameters"/>
    /// </summary>
    /// <param name="parameters"></param>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.info = parameters[parameters.Count - 1];
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnMotdStartReply(new IrcMessageEventArgs<MotdStartReplyMessage>(this));
    }

  }

}