using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when a ChannelModeMessage was sent with a ChannelMode which the server didn't recognize.
  /// </summary>
  [Serializable]
  public class UnknownChannelModeMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="UnknownChannelModeMessage"/> class.
    /// </summary>
    public UnknownChannelModeMessage()
      : base() {
      this.InternalNumeric = 472;
    }

    /// <summary>
    /// Gets or sets the mode which the server didn't recognize
    /// </summary>
    public string UnknownMode {
      get {
        return unknownMode;
      }
      set {
        unknownMode = value;
      }
    }
    private string unknownMode;

    /// <exclude />
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.UnknownMode);
      writer.AddParameter("is unknown mode char to me");
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      this.UnknownMode = "";
      if (parameters.Count > 2) {
        this.UnknownMode = parameters[1];
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnUnknownChannelMode(new IrcMessageEventArgs<UnknownChannelModeMessage>(this));
    }

  }

}