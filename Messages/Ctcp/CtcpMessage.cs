using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Message which carries a CTCP command.
  /// </summary>
  [Serializable]
  public abstract class CtcpMessage : IrcMessage, IChannelTargetedMessage, IQueryTargetedMessage {

    /// <summary>
    /// Gets the targets of this <see cref="CtcpMessage"/>.
    /// </summary>
    public string Target {
      get {
        return this.target;
      }
      set {
        this.target = value;
      }
    }
    private string target = string.Empty;

    /// <summary>
    /// Gets the CTCP Command requested.
    /// </summary>
    protected string InternalCommand {
      get {
        return internalCommand;
      }
      set {
        this.internalCommand = value;
      }
    }
    private string internalCommand = string.Empty;

    /// <summary>
    /// Gets the data payload of the CTCP request.
    /// </summary>
    protected abstract string ExtendedData {
      get;
    }

    /// <summary>
    /// Gets the IRC command used to send the CTCP command to another user.
    /// </summary>
    protected abstract string TransportCommand {
      get;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters"/>. </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = new Collection<string> {
        TransportCommand,
        Target
      };
      string extendedData = CtcpUtil.Escape(ExtendedData);
      if (extendedData.Length != 0) {
        extendedData = " " + extendedData;
      }
      string payLoad = CtcpUtil.ExtendedDataMarker + InternalCommand + extendedData + CtcpUtil.ExtendedDataMarker;
      parameters.Add(payLoad);
      return parameters;
    }

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!CtcpUtil.IsCtcpMessage(unparsedMessage)) {
        return false;
      }

      if (this.TransportCommand != CtcpUtil.GetTransportCommand(unparsedMessage)) {
        return false;
      }

      if (this.InternalCommand.Length != 0 && this.InternalCommand != CtcpUtil.GetInternalCommand(unparsedMessage)) {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count > 0) {
        this.Target = parameters[0];
      } else {
        this.Target = string.Empty;
      }
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel. </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return this.Target.EqualsI(channelName);
    }

    #endregion

    #region IQueryTargetedMessage Members

    bool IQueryTargetedMessage.IsQueryToUser(User user) {
      return IsQueryToUser(user);
    }

    /// <summary>
    ///   Determines if the current message is targeted at a query to the given user. </summary>
    protected virtual bool IsQueryToUser(User user) {
      return user.Nickname.Equals(target);
    }

    #endregion
  }

}