using System;
using System.Collections.Generic;
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

    private IList<string> addedNicks;

    private IList<string> removedNicks;

    /// <summary>
    ///   Gets the collection of nicks being added to the accept list.
    /// </summary>
    public IList<string> AddedNicks {
      get {
        return addedNicks ?? (addedNicks = new Collection<string>());
      }
    }

    /// <summary>
    ///   Gets the collection of nicks being removed from the accept list.
    /// </summary>
    public IList<string> RemovedNicks {
      get {
        return removedNicks ?? (removedNicks = new Collection<string>());
      }
    }

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
      return firstParam != "*";
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);

      foreach (string nick in parameters[0].Split(',')) {
        if (nick.StartsWith("-", StringComparison.Ordinal)) {
          RemovedNicks.Add(nick.Substring(1));
        } else {
          AddedNicks.Add(nick);
        }
      }
    }

    #endregion

    #region Formatting

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      var allNicks = new Collection<string>();
      foreach (string removedNick in RemovedNicks) {
        allNicks.Add("-" + removedNick);
      }
      foreach (string addedNick in AddedNicks) {
        allNicks.Add(addedNick);
      }
      IList<string> parameters = base.GetParameters();
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
