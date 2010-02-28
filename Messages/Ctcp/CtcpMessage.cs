using System;
using System.Collections.Specialized;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A Message which carries a ctcp command.
  /// </summary>
  [Serializable]
  public abstract class CtcpMessage : IrcMessage, IChannelTargetedMessage, IQueryTargetedMessage {

    /// <summary>
    /// Gets the targets of this <see cref="CtcpMessage"/>.
    /// </summary>
    public String Target {
      get {
        return this.target;
      }
      set {
        this.target = value;
      }
    }
    private String target = "";

    /// <summary>
    /// Gets the ctcp Command requested.
    /// </summary>
    protected String InternalCommand {
      get {
        return internalCommand;
      }
      set {
        this.internalCommand = value;
      }
    }
    private String internalCommand = "";

    /// <summary>
    /// Gets the data payload of the Ctcp request.
    /// </summary>
    protected abstract String ExtendedData {
      get;
    }

    /// <summary>
    /// Gets the irc command used to send the ctcp command to another user.
    /// </summary>
    protected abstract String TransportCommand {
      get;
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);
      writer.AddParameter(this.TransportCommand);
      writer.AddParameter(this.Target);
      String extendedData = CtcpUtil.Escape(this.ExtendedData);
      if (extendedData.Length > 0) {
        extendedData = " " + extendedData;
      }
      String payLoad = CtcpUtil.ExtendedDataMarker + this.InternalCommand + extendedData + CtcpUtil.ExtendedDataMarker;
      writer.AddParameter(payLoad);
    }

    /// <summary>
    /// Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(String unparsedMessage) {
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
        this.Target = "";
      }
    }


    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName) {
      return IsTargetedAtChannel(channelName);
    }

    /// <summary>
    /// Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName) {
      return MessageUtil.IsIgnoreCaseMatch(this.Target, channelName);
    }

    #endregion

    #region IQueryTargetedMessage Members

    bool IQueryTargetedMessage.IsQueryToUser(User user) {
      return IsQueryToUser(user);
    }

    /// <summary>
    /// Determines if the current message is targeted at a query to the given user.
    /// </summary>
    protected virtual bool IsQueryToUser(User user) {
      return (MessageUtil.IsIgnoreCaseMatch(user.Nickname, target));
    }

    #endregion
  }

}