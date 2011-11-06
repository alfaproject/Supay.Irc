using System;
using System.Collections.ObjectModel;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Message that edits the list of users on your accept list.
  /// </summary>
  [Serializable]
  public class AcceptListEditorMessage : CommandMessage {
    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "ACCEPT";
      }
    }

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport != null && !serverSupport.CallerId) {
        throw new InvalidMessageException(Resources.ServerDoesNotSupportAccept);
      }
    }

    #region Properties

    /// <summary>
    ///   Gets the collection of nicks being added to the accept list.
    /// </summary>
    public Collection<string> AddedNicks {
      get {
        if (addedNicks == null) {
          addedNicks = new Collection<string>();
        }
        return addedNicks;
      }
    }

    private Collection<string> addedNicks;

    /// <summary>
    ///   Gets the collection of nicks being removed from the accept list.
    /// </summary>
    public Collection<string> RemovedNicks {
      get {
        if (removedNicks == null) {
          removedNicks = new Collection<string>();
        }
        return removedNicks;
      }
    }

    private Collection<string> removedNicks;

    #endregion

    #region Parsing

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      string firstParam = MessageUtil.GetParameter(unparsedMessage, 0);
      return (firstParam != "*");
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      foreach (string nick in parameters[0].Split(',')) {
        if (nick.StartsWith("-", StringComparison.Ordinal)) {
          this.RemovedNicks.Add(nick.Substring(1));
        } else {
          this.AddedNicks.Add(nick);
        }
      }
    }

    #endregion

    #region Formatting

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> allNicks = new Collection<string>();
      foreach (string removedNick in RemovedNicks) {
        allNicks.Add("-" + removedNick);
      }
      foreach (string addedNick in AddedNicks) {
        allNicks.Add(addedNick);
      }
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(allNicks, ","));
      return parameters;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnAcceptListEditor(new IrcMessageEventArgs<AcceptListEditorMessage>(this));
    }

    #endregion
  }
}
