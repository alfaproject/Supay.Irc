using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The base for DCC request messages.
  /// </summary>
  [Serializable]
  public abstract class DccRequestMessage : CtcpRequestMessage
  {
    /// <summary>
    ///   Creates a new instance of the <see cref="DccRequestMessage" /> class.
    /// </summary>
    protected DccRequestMessage()
    {
      this.Port = -1;
      this.Address = IPAddress.None;
      this.InternalCommand = "DCC";
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        return MessageUtil.ParametersToString(false, this.DccCommand, this.DccArgument, TransportAddressFromAddress(this.Address), this.Port.ToString(CultureInfo.InvariantCulture));
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command.
    /// </summary>
    protected abstract string DccCommand
    {
      get;
    }

    /// <summary>
    ///   Gets the DCC sub-command's argument.
    /// </summary>
    protected abstract string DccArgument
    {
      get;
    }

    /// <summary>
    ///   Gets or sets the host address on which the initiator expects the connection.
    /// </summary>
    public IPAddress Address
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the port on which the initiator expects the connection.
    /// </summary>
    public int Port
    {
      get;
      set;
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage)
    {
      return base.CanParse(unparsedMessage) && this.CanParseDccCommand(DccUtil.GetCommand(unparsedMessage));
    }

    /// <summary>
    ///   Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public virtual bool CanParseDccCommand(string command)
    {
      return !string.IsNullOrEmpty(command) && this.DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);

      this.Address = AddressFromTransportAddress(DccUtil.GetAddress(unparsedMessage));
      this.Port = Convert.ToInt32(DccUtil.GetPort(unparsedMessage), CultureInfo.InvariantCulture);
    }

    private static IPAddress AddressFromTransportAddress(string transportAddress)
    {
      double theAddress;
      if (double.TryParse(transportAddress, NumberStyles.Integer, null, out theAddress))
      {
        var backwards = new IPAddress(Convert.ToInt64(theAddress));
        if (backwards.AddressFamily == AddressFamily.InterNetwork)
        {
          var addy = backwards.ToString().Split('.');
          Array.Reverse(addy);
          return IPAddress.Parse(string.Join(".", addy));
        }
        return backwards;
      }
      return IPAddress.Parse(transportAddress);
    }

    private static string TransportAddressFromAddress(IPAddress address)
    {
      if (address.AddressFamily == AddressFamily.InterNetwork)
      {
        var octets = address.GetAddressBytes();
        Array.Reverse(octets);

        var backwards = BitConverter.ToUInt32(octets, 0);
        return backwards.ToString(CultureInfo.InvariantCulture);
      }
      return address.ToString();
    }
  }
}
