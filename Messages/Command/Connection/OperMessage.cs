using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   OperMessage is used by a normal user to obtain IRC operator privileges.
  ///   ( This does not refer to channel ops )
  ///   The correct combination of <see cref="Name" /> and <see cref="Password" /> are required to gain Operator privileges.
  /// </summary>
  [Serializable]
  public class OperMessage : CommandMessage {
    private string name = string.Empty;
    private string password = string.Empty;

    /// <summary>
    ///   Creates a new instance of the OperMessage class.
    /// </summary>
    public OperMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the OperMessage class with the given name and password.
    /// </summary>
    public OperMessage(string name, string password) {
      this.name = name;
      this.password = password;
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "OPER";
      }
    }

    /// <summary>
    ///   Gets or sets the password for the sender.
    /// </summary>
    public virtual string Password {
      get {
        return password;
      }
      set {
        password = value;
      }
    }

    /// <summary>
    ///   Gets or sets the name for the sender.
    /// </summary>
    public virtual string Name {
      get {
        return name;
      }
      set {
        name = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Name);
      parameters.Add(Password);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 2) {
        Name = parameters[0];
        Password = parameters[1];
      } else {
        Name = string.Empty;
        Password = string.Empty;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnOper(new IrcMessageEventArgs<OperMessage>(this));
    }
  }
}
