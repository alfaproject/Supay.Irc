using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The ErrorMessage sent when a user tries to connect with an ident containing invalid characters
  /// </summary>
  /// <remarks>
  /// Not all networks will send this message, some will silently change your ident,
  /// while others will simply disconnect you.
  /// </remarks>
  [Serializable]
  public class IdentChangedMessage : ErrorMessage {

    /// <summary>
    /// Creates a new instances of the <see cref="IdentChangedMessage"/> class.
    /// </summary>
    public IdentChangedMessage()
      : base() {
      this.InternalNumeric = 455;
    }

    /// <summary>
    /// Gets or sets the ident that was attempted
    /// </summary>
    public String Ident {
      get {
        return ident;
      }
      set {
        ident = value;
      }
    }
    private String ident;

    /// <summary>
    /// Gets or sets the characters in the attempted ident which were invalid
    /// </summary>
    public String InvalidCharacters {
      get {
        return invalidCharacters;
      }
      set {
        invalidCharacters = value;
      }
    }
    private String invalidCharacters;

    /// <summary>
    /// Gets or sets the new ident being assigned
    /// </summary>
    public String NewIdent {
      get {
        return newIdent;
      }
      set {
        newIdent = value;
      }
    }
    private String newIdent;

    /// <exclude />
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      String param = String.Format(CultureInfo.InvariantCulture, "Your username {0} contained the invalid character(s) {1} and has been changed to {2}. Please use only the characters 0-9 a-z A-Z _ - or . in your username. Your username is the part before the @ in your email address.", this.Ident, this.InvalidCharacters, this.NewIdent);
      writer.AddParameter(param);
    }

    /// <exclude />
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      String param = parameters[1];
      this.Ident = MessageUtil.StringBetweenStrings(param, "Your username ", " contained the invalid ");
      this.InvalidCharacters = MessageUtil.StringBetweenStrings(param, "invalid character(s) ", " and has ");
      this.NewIdent = MessageUtil.StringBetweenStrings(param, "has been changed to ", ". Please");
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnIdentChanged(new IrcMessageEventArgs<IdentChangedMessage>(this));
    }

  }

}