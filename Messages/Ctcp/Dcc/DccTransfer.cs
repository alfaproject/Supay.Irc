using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Supay.Irc.Properties;

namespace Supay.Irc.Dcc {
  /// <summary>
  ///   Handles the networks level communication protocols for sending and receiving files over DCC.
  /// </summary>
  public class DccTransfer {
    private byte[] _buffer;
    private int _bufferSize = 4096;

    #region Constructor

    /// <summary>
    ///   Initializes a new instance of the DccTransfer class.
    /// </summary>
    public DccTransfer() {
      FileSize = -1;
      BytesTransferred = 0;
      SendAhead = true;
      Secure = false;
      TurboMode = false;
      StartPosition = 0;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets a stream to the file being transferred.
    /// </summary>
    public FileStream File {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the start position in the file to transfer the information.
    /// </summary>
    public long StartPosition {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the socket the file transfer will use.
    /// </summary>
    public Socket TransferSocket {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the size of the buffer for transfer of the file.
    /// </summary>
    public int BufferSize {
      get {
        return _bufferSize;
      }
      set {
        if (value > 8192) {
          throw new ArgumentException(Resources.BufferSizeIsLimited, "value");
        }
        _bufferSize = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the transfer uses the "turbo" extension to increase transfer speed.
    /// </summary>
    public bool TurboMode {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets if the transfer uses SSL to secure the transfer.
    /// </summary>
    public bool Secure {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets if the transfer uses the "send ahead" extension to increase transfer speed.
    /// </summary>
    public bool SendAhead {
      get;
      set;
    }

    /// <summary>
    ///   Gets the number of bytes transferred so far.
    /// </summary>
    public long BytesTransferred {
      get;
      private set;
    }

    /// <summary>
    ///   Gets or sets the size of the file being transferred.
    /// </summary>
    public long FileSize {
      get;
      set;
    }

    #endregion

    #region Events

    /// <summary>
    ///   The TransferInterruption event occurs when the file has not completely transferred, but
    ///   the connection has been stopped.
    /// </summary>
    public event EventHandler TransferInterruption;

    /// <summary>
    ///   Raises the <see cref="TransferInterruption" /> event.
    /// </summary>
    protected void OnTransferInterruption(EventArgs e) {
      if (TransferInterruption != null) {
        TransferInterruption(this, e);
      }
    }

    /// <summary>
    ///   The TransferComplete event occurs when the file has been completely transferred.
    /// </summary>
    public event EventHandler TransferComplete;

    /// <summary>
    ///   Raises the <see cref="TransferComplete" /> event.
    /// </summary>
    protected void OnTransferComplete(EventArgs e) {
      if (TransferComplete != null) {
        TransferComplete(this, e);
      }
    }

    #endregion

    #region Internal Methods

    /// <summary>
    ///   Sends the file over the current socket.
    /// </summary>
    internal void Send() {
      if (!File.CanRead) {
        throw new InvalidOperationException(Resources.CannotReadFromFile);
      }

      BytesTransferred = 0;

      _buffer = new byte[BufferSize];
      var acknowledgment = new byte[4];

      int bytesSent;
      while ((bytesSent = File.Read(_buffer, 0, _buffer.Length)) != 0) {
        try {
          TransferSocket.Send(_buffer, bytesSent, SocketFlags.None);
          BytesTransferred += bytesSent;
          if (!TurboMode && !SendAhead) {
            TransferSocket.Receive(acknowledgment);
          }
        } catch {
          OnTransferInterruption(EventArgs.Empty);
        }
      }

      if (!TurboMode) {
        while (!AllAcknowledgementsReceived(acknowledgment)) {
          TransferSocket.Receive(acknowledgment);
        }
      }
      OnTransferComplete(EventArgs.Empty);
    }

    /// <summary>
    ///   Receives the file over the current socket.
    /// </summary>
    internal void Receive() {
      BytesTransferred = 0;

      _buffer = new byte[BufferSize];

      while (!IsTransferComplete) {
        int bytesReceived = TransferSocket.Receive(_buffer);
        if (bytesReceived == 0) {
          OnTransferInterruption(EventArgs.Empty);
          return;
        }
        BytesTransferred += bytesReceived;
        if (File.CanWrite) {
          File.Write(_buffer, 0, bytesReceived);
        }
        SendAcknowledgement();
      }
      File.Flush();
      OnTransferComplete(EventArgs.Empty);
    }

    #endregion

    #region Private Methods

    private bool IsTransferComplete {
      get {
        if (FileSize == -1) {
          return false;
        }
        return StartPosition + BytesTransferred >= FileSize;
      }
    }

    private void SendAcknowledgement() {
      if (TurboMode) {
        return;
      }

      // convert BytesTransfered to a 4 byte array containing the number
      byte[] bytesAck = DccBytesReceivedFormat();

      // send it over the socket
      TransferSocket.Send(bytesAck);
    }

    private bool AllAcknowledgementsReceived(byte[] lastAck) {
      long acknowledgedBytes = DccBytesToLong(lastAck);
      return acknowledgedBytes >= BytesTransferred;
    }

    private byte[] DccBytesReceivedFormat() {
      var size = new byte[4];
      byte[] longBytes = BitConverter.GetBytes(NetworkUnsignedLong(BytesTransferred));
      Array.Copy(longBytes, 0, size, 0, 4);
      return size;
    }

    private static long DccBytesToLong(byte[] received) {
      return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(received, 0));
    }

    private static long NetworkUnsignedLong(long hostOrderLong) {
      long networkLong = IPAddress.HostToNetworkOrder(hostOrderLong);
      return (networkLong >> 32) & 0x00000000ffffffff;
    }

    #endregion
  }
}
