using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Sent to a user who is trying to send control codes to a channel that is set +c.
  /// </summary>
  [Serializable]
  public class CannotUseColorsMessage : ErrorMessage, IChannelTargetedMessage {
    /// <summary>
    ///   Creates a new instances of the <see cref="CannotUseColorsMessage" /> class.
    /// </summary>
    public CannotUseColorsMessage()
      : base(408) {
    }

    /// <summary>
    ///   Gets or sets the channel to which the message can't be sent.
    /// </summary>
    public virtual string Channel {
      get {
        return channel;
      }
      set {
        channel = value;
      }
    }

    private string channel = string.Empty;

    /// <summary>
    ///   Gets or sets the text which wasn't sent to the channel.
    /// </summary>
    public string Text {
      get {
        return _text;
      }
      set {
        _text = value;
      }
    }

    private string _text;

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(Channel);
      parameters.Add("You cannot use colours on this channel. Not sent: ");
      parameters.Add(Text);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.Channel = string.Empty;
      this.Text = string.Empty;

      if (parameters.Count > 1) {
        this.Channel = parameters[1];
        if (parameters.Count == 3) {
          string freeText = parameters[2];
          this.Text = freeText.Substring(freeText.IndexOf(": ", StringComparison.Ordinal) + 2);
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnCannotUseColors(new IrcMessageEventArgs<CannotUseColorsMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Channel.EqualsI(channelName);
    }

    #endregion
  }
}
