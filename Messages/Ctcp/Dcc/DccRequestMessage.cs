using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Supay.Irc.Dcc;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The base for DCC request messages.
  /// </summary>
  [Serializable]
  public abstract class DccRequestMessage : CtcpRequestMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="DccRequestMessage" /> class.
    /// </summary>
    protected DccRequestMessage() {
      Port = -1;
      Address = IPAddress.None;
      InternalCommand = "DCC";
    }

    /// <summary>
    ///   Gets the data payload of the CTCP request.
    /// </summary>
    protected override string ExtendedData {
      get {
        return MessageUtil.ParametersToString(false, DccCommand, DccArgument, TransportAddressFromAddress(Address), Port.ToString(CultureInfo.InvariantCulture));
      }
    }

    /// <summary>
    ///   Gets the DCC sub-command.
    /// </summary>
    protected abstract string DccCommand {
      get;
    }

    /// <summary>
    ///   Gets the DCC sub-command's argument.
    /// </summary>
    protected abstract string DccArgument {
      get;
    }

    /// <summary>
    ///   Gets or sets the host address on which the initiator expects the connection.
    /// </summary>
    public IPAddress Address {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the port on which the initiator expects the connection.
    /// </summary>
    public int Port {
      get;
      set;
    }

    /// <summary>
    ///   Determines if the message can be parsed by this type.
    /// </summary>
    public override bool CanParse(string unparsedMessage) {
      return base.CanParse(unparsedMessage) && CanParseDccCommand(DccUtil.GetCommand(unparsedMessage));
    }

    /// <summary>
    ///   Determines if the message's DCC command is compatible with this message.
    /// </summary>
    public virtual bool CanParseDccCommand(string command) {
      return !string.IsNullOrEmpty(command) && DccCommand.EndsWith(command, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    public override void Parse(string unparsedMessage) {
      base.Parse(unparsedMessage);

      Address = AddressFromTransportAddress(DccUtil.GetAddress(unparsedMessage));
      Port = Convert.ToInt32(DccUtil.GetPort(unparsedMessage), CultureInfo.InvariantCulture);
    }

    private static IPAddress AddressFromTransportAddress(string transportAddress) {
      double theAddress;
      if (double.TryParse(transportAddress, NumberStyles.Integer, null, out theAddress)) {
        var backwards = new IPAddress(Convert.ToInt64(theAddress));
        if (backwards.AddressFamily == AddressFamily.InterNetwork) {
          string[] addy = backwards.ToString().Split('.');
          Array.Reverse(addy);
          return IPAddress.Parse(string.Join(".", addy));
        }
        return backwards;
      }
      return IPAddress.Parse(transportAddress);
    }

    private static string TransportAddressFromAddress(IPAddress address) {
      if (address.AddressFamily == AddressFamily.InterNetwork) {
        string[] octets = address.ToString().Split('.');
        Array.Reverse(octets);

        IPAddress backwards = IPAddress.Parse(string.Join(".", octets));
        return backwards.ToString();
      }
      return address.ToString();
    }
  }
}
