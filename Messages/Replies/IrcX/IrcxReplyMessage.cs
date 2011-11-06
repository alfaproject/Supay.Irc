using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A reply to a <see cref="IrcxMessage" /> or a <see cref="IsIrcxMessage" />.
  /// </summary>
  [Serializable]
  public class IrcxReplyMessage : NumericMessage {
    private readonly Collection<string> authenticationPackages = new Collection<string>();
    private string ircxVersion = string.Empty;
    private bool isIrcxClientMode;
    private int maximumMessageLength = -1;
    private string tokens = "*";

    /// <summary>
    ///   Creates a new instance of the <see cref="IrcxReplyMessage" />.
    /// </summary>
    public IrcxReplyMessage()
      : base(800) {
    }

    /// <summary>
    ///   Gets or sets if the server has set the client into IRCX mode.
    /// </summary>
    public virtual bool IsIrcxClientMode {
      get {
        return isIrcxClientMode;
      }
      set {
        isIrcxClientMode = value;
      }
    }

    /// <summary>
    ///   Gets or sets the version of IRCX the server implements.
    /// </summary>
    public virtual string Version {
      get {
        return ircxVersion;
      }
      set {
        ircxVersion = value;
      }
    }

    /// <summary>
    ///   Gets the collection of authentication packages
    /// </summary>
    public virtual Collection<string> AuthenticationPackages {
      get {
        return authenticationPackages;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum message length, in bytes.
    /// </summary>
    public virtual int MaximumMessageLength {
      get {
        return maximumMessageLength;
      }
      set {
        maximumMessageLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets the tokens
    /// </summary>
    /// <remarks>
    ///   There are no known servers that implement this property.
    ///   It is almost always just *.
    /// </remarks>
    public virtual string Tokens {
      get {
        return tokens;
      }
      set {
        tokens = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(IsIrcxClientMode ? "1" : "0");
      parameters.Add(Version);
      parameters.Add(MessageUtil.CreateList(AuthenticationPackages, ","));
      parameters.Add(MaximumMessageLength.ToString(CultureInfo.InvariantCulture));
      parameters.Add(Tokens);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 5) {
        IsIrcxClientMode = (parameters[1] == "1");
        Version = parameters[2];
        AuthenticationPackages.Clear();
        foreach (string package in parameters[3].Split(',')) {
          AuthenticationPackages.Add(package);
        }
        MaximumMessageLength = int.Parse(parameters[4], CultureInfo.InvariantCulture);
        Tokens = parameters.Count == 6 ? parameters[5] : string.Empty;
      } else {
        IsIrcxClientMode = false;
        Version = string.Empty;
        AuthenticationPackages.Clear();
        MaximumMessageLength = -1;
        Tokens = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnIrcxReply(new IrcMessageEventArgs<IrcxReplyMessage>(this));
    }
  }
}
