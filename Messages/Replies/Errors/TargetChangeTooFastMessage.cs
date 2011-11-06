using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  /// The <see cref="ErrorMessage"/> sent when a user tries to send commands to too many targets in a short amount of time.
  /// </summary>
  /// <remarks>
  /// The purpose of this error condition is to help stop spammers.
  /// </remarks>
  [Serializable]
  public class TargetChangeTooFastMessage : ErrorMessage, IChannelTargetedMessage {
    /// <summary>
    /// Creates a new instances of the <see cref="TargetChangeTooFastMessage"/> class.
    /// </summary>
    public TargetChangeTooFastMessage()
      : base(439) {
    }

    /// <summary>
    /// Gets or sets the nick or channel which was attempted
    /// </summary>
    public string TargetChanged {
      get {
        return target;
      }
      set {
        target = value;
      }
    }

    private string target;

    /// <summary>
    /// Gets or sets the number of seconds which must be waited before attempting again.
    /// </summary>
    public int Seconds {
      get {
        return seconds;
      }
      set {
        seconds = value;
      }
    }

    private int seconds;

    /// <exclude />
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(TargetChanged);
      parameters.Add(string.Format(CultureInfo.InvariantCulture, "Target change too fast. Please wait {0} seconds.", Seconds));
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      this.TargetChanged = string.Empty;
      this.Seconds = -1;
      if (parameters.Count > 1) {
        this.TargetChanged = parameters[1];
        if (parameters.Count > 2) {
          this.Seconds = Convert.ToInt32(MessageUtil.StringBetweenStrings(parameters[2], "Please wait ", " seconds"), CultureInfo.InvariantCulture);
        }
      }
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnTargetChangeTooFast(new IrcMessageEventArgs<TargetChangeTooFastMessage>(this));
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.TargetChanged.EqualsI(channelName);
    }

    #endregion
  }
}
