using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when an user's silence list is full, and a
  ///   <see cref="SilenceMessage" /> is sent adding an user to the list.
  /// </summary>
  [Serializable]
  public class SilenceListFullMessage : ErrorMessage {
    private Mask silenceMask;

    /// <summary>
    ///   Creates a new instances of the <see cref="SilenceListFullMessage" /> class.
    /// </summary>
    public SilenceListFullMessage()
      : base(511) {
    }

    /// <summary>
    ///   Gets or sets the mask of the user being silenced.
    /// </summary>
    public Mask SilenceMask {
      get {
        return silenceMask;
      }
      set {
        silenceMask = value;
      }
    }

    /// <exclude />
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(SilenceMask.ToString());
      parameters.Add("Your silence list is full");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      SilenceMask = parameters.Count > 1 ? new Mask(parameters[1]) : new Mask();
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnSilenceListFull(new IrcMessageEventArgs<SilenceListFullMessage>(this));
    }
  }
}
