using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The base class for server query messages.
  /// </summary>
  [Serializable]
  public abstract class ServerQueryBase : CommandMessage {
    private string target = string.Empty;

    /// <summary>
    ///   Gets or sets the target server of the query.
    /// </summary>
    public virtual string Target {
      get {
        return target;
      }
      set {
        target = value;
      }
    }

    /// <summary>
    ///   Gets the index of the parameter which holds the server which should respond to the query.
    /// </summary>
    protected virtual int TargetParsingPosition {
      get {
        return 0;
      }
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      Target = parameters.Count >= TargetParsingPosition + 1 ? parameters[TargetParsingPosition] : string.Empty;
    }
  }
}
