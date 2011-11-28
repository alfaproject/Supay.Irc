using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   A Message which carries a CTCP command.
  /// </summary>
  [Serializable]
  public abstract class CtcpMessage : IrcMessage, IChannelTargetedMessage, IQueryTargetedMessage
  {
    private string internalCommand = string.Empty;
    private string target = string.Empty;

    /// <summary>
    ///   Gets the targets of this <see cref="CtcpMessage" />.
    /// </summary>
    public string Target
    {
      get
      {
        return this.target;
      }
      set
      {
        this.target = value;
      }
    }

    /// <summary>
    ///   Gets the CTCP Command requested.
    /// </summary>
    protected string InternalCommand
    {
      get
      {
        return this.internalCommand;
      }
      set
      {
        this.internalCommand = value;
      }
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected abstract string ExtendedData
    {
      get;
    }

    /// <summary>
    ///   Gets the IRC command used to send the CTCP command to another user.
    /// </summary>
    protected abstract string TransportCommand
    {
      get;
    }

    #region IChannelTargetedMessage Members

    bool IChannelTargetedMessage.IsTargetedAtChannel(string channelName)
    {
      return this.IsTargetedAtChannel(channelName);
    }

    #endregion

    #region IQueryTargetedMessage Members

    bool IQueryTargetedMessage.IsQueryToUser(User user)
    {
      return this.IsQueryToUser(user);
    }

    #endregion

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = new Collection<string> {
        this.TransportCommand,
        this.Target
      };
      string extendedData = CtcpUtil.Escape(this.ExtendedData);
      if (extendedData.Length != 0)
      {
        extendedData = " " + extendedData;
      }
      string payLoad = CtcpUtil.EXTENDED_DATA_MARKER + this.InternalCommand + extendedData + CtcpUtil.EXTENDED_DATA_MARKER;
      parameters.Add(payLoad);
      return parameters;
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage)
    {
      return CtcpUtil.IsCtcpMessage(unparsedMessage)
        && this.TransportCommand == CtcpUtil.GetTransportCommand(unparsedMessage)
        && (this.InternalCommand.Length == 0 || this.InternalCommand == CtcpUtil.GetInternalCommand(unparsedMessage));
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Target = parameters.Count > 0 ? parameters[0] : string.Empty;
    }

    /// <summary>
    ///   Determines if the the current message is targeted at the given channel.
    /// </summary>
    protected virtual bool IsTargetedAtChannel(string channelName)
    {
      return this.Target.Equals(channelName, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Determines if the current message is targeted at a query to the given user.
    /// </summary>
    protected virtual bool IsQueryToUser(User user)
    {
      return user.Nickname.Equals(this.target);
    }
  }
}
