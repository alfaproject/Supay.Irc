using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   A Message which carries a CTCP command.
  /// </summary>
  [Serializable]
  public abstract class CtcpMessage : IrcMessage, IChannelTargetedMessage, IQueryTargetedMessage {
    private string internalCommand = string.Empty;
    private string target = string.Empty;

    /// <summary>
    ///   Gets the targets of this <see cref="CtcpMessage" />.
    /// </summary>
    public string Target {
      get {
        return target;
      }
      set {
        target = value;
      }
    }

    /// <summary>
    ///   Gets the CTCP Command requested.
    /// </summary>
    protected string InternalCommand {
      get {
        return internalCommand;
      }
      set {
        internalCommand = value;
      }
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected abstract string ExtendedData {
      get;
    }

    /// <summary>
    ///   Gets the IRC command used to send the CTCP command to another user.
    /// </summary>
    protected abstract string TransportCommand {
      get;
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    #endregion

    #region IQueryTargetedMessage Members

    bool IQueryTargetedMessage.IsQueryToUser(User user) {
      return IsQueryToUser(user);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      var parameters = new Collection<string> {
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
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      if (!CtcpUtil.IsCtcpMessage(unparsedMessage)) {
        return false;
      }

      if (TransportCommand != CtcpUtil.GetTransportCommand(unparsedMessage)) {
        return false;
      }

      if (InternalCommand.Length != 0 && InternalCommand != CtcpUtil.GetInternalCommand(unparsedMessage)) {
        return false;
      }

      return true;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      Target = parameters.Count > 0 ? parameters[0] : string.Empty;
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return Target.EqualsI(channelName);
    }

    /// <summary>
    ///   Determines if the current message is targeted at a query to the given user.
    /// </summary>
    protected virtual bool IsQueryToUser(User user) {
      return user.Nickname.Equals(target);
    }
  }
}
