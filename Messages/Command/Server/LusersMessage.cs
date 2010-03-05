using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// Requests that the server send information about the size of the IRC network.
  /// </summary>
  [Serializable]
  public class LusersMessage : ServerQueryBase {

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "LUSERS";
      }
    }

    /// <summary>
    /// Gets or sets the mask that limits the servers which information will be returned.
    /// </summary>
    public virtual string Mask {
      get {
        return mask;
      }
      set {
        mask = value;
      }
    }
    private string mask = "";

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      if (!string.IsNullOrEmpty(Mask)) {
        parameters.Add(Mask);
        parameters.Add(Target);
      }
      return parameters;
    }

    /// <summary>
    /// Gets the index of the parameter which holds the server which should respond to the query.
    /// </summary>
    protected override int TargetParsingPosition {
      get {
        return 1;
      }
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 1) {
        this.Mask = parameters[0];
      } else {
        this.Mask = "";
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnLusers(new IrcMessageEventArgs<LusersMessage>(this));
    }

  }

}