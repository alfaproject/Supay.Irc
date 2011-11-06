using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   One of the responses to the <see cref="LusersMessage" /> query.
  /// </summary>
  [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Op")]
  [Serializable]
  public class LusersOpReplyMessage : NumericMessage {
    private string info = string.Empty;
    private int opCount = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="LusersOpReplyMessage" /> class
    /// </summary>
    public LusersOpReplyMessage()
      : base(252) {
    }

    /// <summary>
    ///   Gets or sets the number of IRC operators connected to the server.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Op")]
    public virtual int OpCount {
      get {
        return opCount;
      }
      set {
        opCount = value;
      }
    }

    /// <summary>
    ///   Gets or sets any additional information about the operators connected.
    /// </summary>
    public virtual string Info {
      get {
        return info;
      }
      set {
        info = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(OpCount.ToString(CultureInfo.InvariantCulture));
      parameters.Add(Info);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 2) {
        OpCount = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
        Info = parameters[2];
      } else {
        OpCount = -1;
        Info = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnLusersOpReply(new IrcMessageEventArgs<LusersOpReplyMessage>(this));
    }
  }
}
