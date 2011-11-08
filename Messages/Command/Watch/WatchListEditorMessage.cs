using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Message that edits the list of users on your watch list.
  /// </summary>
  [Serializable]
  public class WatchListEditorMessage : WatchMessage {
    #region Properties

    private IList<string> addedNicks;

    private IList<string> removedNicks;

    /// <summary>
    ///   Gets the collection of nicks being added to the watch list.
    /// </summary>
    public IList<string> AddedNicks {
      get {
        return addedNicks ?? (addedNicks = new Collection<string>());
      }
    }

    /// <summary>
    ///   Gets the collection of nicks being removed from the watch list.
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
      return (firstParam.StartsWith("+", StringComparison.Ordinal) || firstParam.StartsWith("-", StringComparison.Ordinal));
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);
      foreach (string param in parameters) {
        if (param.StartsWith("+", StringComparison.Ordinal)) {
          AddedNicks.Add(param.Substring(1));
        }
        if (param.StartsWith("-", StringComparison.Ordinal)) {
          RemovedNicks.Add(param.Substring(1));
        }
      }
    }

    #endregion

    #region Formatting

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      if (AddedNicks != null) {
        foreach (string addedNick in AddedNicks) {
          parameters.Add("+" + addedNick);
        }
      }
      if (RemovedNicks != null) {
        foreach (string removedNick in RemovedNicks) {
          parameters.Add("-" + removedNick);
        }
      }
      return parameters;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWatchListEditor(new IrcMessageEventArgs<WatchListEditorMessage>(this));
    }

    #endregion
  }
}
