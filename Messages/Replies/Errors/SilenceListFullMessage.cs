using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage received when a user's silence list is full, and a SilenceMessage is sent adding a User to the list.
  /// </summary>
  [Serializable]
  public class SilenceListFullMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="SilenceListFullMessage"/> class.
    /// </summary>
    public SilenceListFullMessage()
      : base() {
      this.InternalNumeric = 511;
    }

    /// <summary>
    ///   Gets or sets the mask of the user being silenced. </summary>
    public Mask SilenceMask {
      get {
        return silenceMask;
      }
      set {
        silenceMask = value;
      }
    }
    private Mask silenceMask;


    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(SilenceMask.ToString());
      parameters.Add("Your silence list is full");
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 1) {
        this.SilenceMask = new Mask(parameters[1]);
      } else {
        this.SilenceMask = new Mask();
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnSilenceListFull(new IrcMessageEventArgs<SilenceListFullMessage>(this));
    }

  }

}