using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A <see cref="IrcMessage" /> which carries communication from a person to another person or
  ///   channel.
  /// </summary>
  [Serializable]
  public abstract class TextMessage : CommandMessage, IChannelTargetedMessage, IQueryTargetedMessage {
    protected TextMessage()
      : this(string.Empty) {
    }

    protected TextMessage(string text) {
      Text = text;
      Targets = new List<string>();
    }

    /// <summary>
    ///   Gets the target of this <see cref="TextMessage" />.
    /// </summary>
    public List<string> Targets {
      get;
      private set;
    }

    /// <summary>
    ///   Gets or sets the actual text of this <see cref="TextMessage" />.
    /// </summary>
    /// <remarks>
    ///   This property holds the core purpose of IRC itself... sending text communication to
    ///   others.
    /// </remarks>
    public string Text {
      get;
      set;
    }

    #region IChannelTargetedMessage Members

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    public virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.ContainsIgnoreCaseMatch(Targets, channelName);
    }

    #endregion

    #region IQueryTargetedMessage Members

    /// <summary>
    ///   Determines if the current message is targeted at a query to the given user.
    /// </summary>
    public virtual bool IsQueryToUser(User user) {
      return Targets.Any(target => user.Nickname.EqualsI(target));
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(Targets, ","));
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Targets.Clear();
      if (parameters.Count >= 2) {
        Targets.AddRange(parameters[0].Split(','));
        Text = parameters[1];
      }
    }
  }
}
