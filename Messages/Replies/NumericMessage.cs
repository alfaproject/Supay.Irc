using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The base class for all numeric messages sent from the server to the client.
  /// </summary>
  [Serializable]
  public abstract class NumericMessage : IrcMessage {
    private int _internalNumeric;

    protected NumericMessage()
      : this(-1) {
    }

    protected NumericMessage(int number) {
      _internalNumeric = number;
      Target = string.Empty;
    }

    /// <summary>
    ///   Gets the Numeric command of the Message.
    /// </summary>
    public virtual int InternalNumeric {
      get {
        return _internalNumeric;
      }
      protected set {
        _internalNumeric = value;
      }
    }

    /// <summary>
    ///   Gets or sets the target of the message.
    /// </summary>
    public string Target {
      get;
      set;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      var parameters = new Collection<string> {
        InternalNumeric.ToString("000", CultureInfo.InvariantCulture)
      };
      if (!string.IsNullOrEmpty(Target)) {
        parameters.Add(Target);
      }
      return parameters;
    }

    /// <summary>
    ///   Determines if the given numeric is an error message.
    /// </summary>
    public static bool IsError(int numeric) {
      const int normalStart = 400;
      const int normalEnd = 599;
      const int ircxStart = 900;
      const int ircxEnd = 998;
      return ((normalStart <= numeric && numeric <= normalEnd) || (ircxStart <= numeric && numeric <= ircxEnd));
    }

    /// <summary>
    ///   Determines if the given numeric is a direct message.
    /// </summary>
    public static bool IsDirect(int numeric) {
      return (0 < numeric && numeric < 100);
    }

    /// <summary>
    ///   Determines if the given numeric is a command reply message.
    /// </summary>
    public static bool IsCommandReply(int numeric) {
      return (!IsError(numeric) && !IsDirect(numeric));
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      int parsedNumeric;
      if (int.TryParse(MessageUtil.GetCommand(unparsedMessage), out parsedNumeric)) {
        if (0 <= parsedNumeric && parsedNumeric < 1000) {
          if (InternalNumeric == -1) {
            return true;
          }
          return (InternalNumeric == parsedNumeric);
        }
      }
      return false;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 0) {
        Target = parameters[0];
      }
    }
  }
}
