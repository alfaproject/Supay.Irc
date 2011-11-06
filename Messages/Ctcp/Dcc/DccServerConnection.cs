using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;
using Supay.Irc.Messages;
using Supay.Irc.Network;

namespace Supay.Irc.Dcc {
  /// <summary>
  ///   The DccServerConnection is used after sending a <see cref="DccSendRequestMessage" /> to send
  ///   the file to the target.
  /// </summary>
  /// <remarks>
  ///   The nature of DCC sending is such that this class will create a listening server on the
  ///   given port. If the target of the <see cref="DccSendRequestMessage" /> decides to connect to
  ///   the server, this class will send the file.
  ///   Set a <see cref="DccServerConnection.TimeOut" /> to have the server stop after the given
  ///   amount of time, in case the target ignores the initial message.
  /// </remarks>
  [DesignerCategory("Code")]
  public class DccServerConnection : Component {
    private TcpListener _chatListener;
    private Thread _connectionWorker;
    private int _port;
    private Timer _timeoutTimer;
    private readonly object _syncLock = new object();

    private delegate void SyncInvoke();

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="DccServerConnection" /> class.
    /// </summary>
    public DccServerConnection() {
      Status = ConnectionStatus.Disconnected;
      TimeOut = TimeSpan.Zero;
      Transfer = new DccTransfer();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="DccServerConnection" /> class on the given
    ///   port.
    /// </summary>
    /// <param name="port">The port to listen on.</param>
    public DccServerConnection(int port)
      : this() {
      Port = port;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Occurs when starting the connecting sequence to a server.
    /// </summary>
    public event EventHandler Connecting;

    /// <summary>
    ///   Raises the <see cref="DccServerConnection.Connecting" /> event of the
    ///   <see cref="DccServerConnection" /> object.
    /// </summary>
    protected void OnConnecting(EventArgs e) {
      if (Connecting != null) {
        Connecting(this, e);
      }
    }

    /// <summary>
    ///   Occurs after the connecting sequence is successful.
    /// </summary>
    public event EventHandler Connected;

    /// <summary>
    ///   Raises the <see cref="DccServerConnection.Connected" /> event of the
    ///   <see cref="DccServerConnection" /> object.
    /// </summary>
    protected void OnConnected(EventArgs e) {
      if (SynchronizationObject != null && SynchronizationObject.InvokeRequired) {
        SyncInvoke del = () => OnConnected(e);
        SynchronizationObject.Invoke(del, null);
        return;
      }

      if (Connected != null) {
        Connected(this, e);
      }
    }

    /// <summary>
    ///   Occurs when the disconnecting sequence is successful.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> Disconnected;

    /// <summary>
    ///   Raises the <see cref="DccServerConnection.Disconnected" /> event of the
    ///   <see cref="DccServerConnection" /> object.
    /// </summary>
    protected virtual void OnDisconnected(ConnectionDataEventArgs e) {
      if (SynchronizationObject != null && SynchronizationObject.InvokeRequired) {
        SyncInvoke del = () => OnDisconnected(e);
        SynchronizationObject.Invoke(del, null);
        return;
      }

      if (Disconnected != null) {
        Disconnected(this, e);
      }
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the port which the <see cref="DccServerConnection" /> will communicate
    ///   over.
    /// </summary>
    /// <exception cref="NotSupportedException">
    ///   This exception will be thrown if an attempt is made to change the
    ///   <see cref="DccServerConnection.Port" /> if the <see cref="DccServerConnection.Status" /> is
    ///   not <see cref="ConnectionStatus.Disconnected" />.
    /// </exception>
    public int Port {
      get {
        return _port;
      }
      set {
        if (Status == ConnectionStatus.Disconnected) {
          _port = value;
        } else {
          throw new NotSupportedException(Properties.Resources.PortCannotBeChanged);
        }
      }
    }

    /// <summary>
    ///   Gets or sets the length of time to wait after calling <see cref="Send" /> before the
    ///   thread will stop waiting for a connection.
    /// </summary>
    public TimeSpan TimeOut {
      get;
      set;
    }

    /// <summary>
    ///   Gets the <see cref="ConnectionStatus" /> of the <see cref="DccServerConnection" />.
    /// </summary>
    public ConnectionStatus Status {
      get;
      private set;
    }

    /// <summary>
    ///   Gets or sets the <see cref="ISynchronizeInvoke" /> implementor which will be used to
    ///   synchronize threads and events.
    /// </summary>
    /// <remarks>
    ///   This is usually the main form of the application.
    /// </remarks>
    public ISynchronizeInvoke SynchronizationObject {
      get;
      set;
    }

    /// <summary>
    ///   Gets the transfer information for the connection.
    /// </summary>
    public DccTransfer Transfer {
      get;
      private set;
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Attempts to send the file specified in the current
    ///   <see cref="DccServerConnection.Transfer" /> information.
    /// </summary>
    public void Send() {
      lock (_syncLock) {
        if (Status != ConnectionStatus.Disconnected) {
          throw new InvalidOperationException(Properties.Resources.AlreadyConnectToAnotherClient);
        }

        Status = ConnectionStatus.Connecting;
        OnConnecting(EventArgs.Empty);
      }

      _connectionWorker = new Thread(RunSend) {
        IsBackground = true
      };
      _connectionWorker.Start();

      if (TimeOut != TimeSpan.Zero) {
        _timeoutTimer = new Timer(CheckTimeOut, null, TimeOut, TimeSpan.Zero);
      }
    }

    /// <summary>
    ///   Closes the current network connection.
    /// </summary>
    public void Disconnect() {
      Status = ConnectionStatus.Disconnected;
      OnDisconnected(new ConnectionDataEventArgs("Disconnect Called"));
    }

    /// <summary>
    ///   Forces closing the current network connection and kills the thread running it.
    /// </summary>
    public void DisconnectForce() {
      Disconnect();
      if (_connectionWorker != null && _connectionWorker.IsAlive) {
        _connectionWorker.Abort();
      }
    }

    #endregion

    #region Private Methods

    private void CheckTimeOut(object state) {
      if (Status == ConnectionStatus.Connecting) {
        DisconnectForce();
      }
      _timeoutTimer.Dispose();
      _timeoutTimer = null;
    }

    private void RunSend() {
      string disconnectReason = string.Empty;

      try {
        _chatListener = new TcpListener(System.Net.IPAddress.Any, Port);
        _chatListener.Start();
        Socket socket = _chatListener.AcceptSocket();

        Status = ConnectionStatus.Connected;
        OnConnected(EventArgs.Empty);

        Transfer.TransferSocket = socket;
        Transfer.Send();
      } catch (Exception ex) {
        System.Diagnostics.Trace.WriteLine("Error Opening DccServerConnection On Port " + _port.ToString(CultureInfo.InvariantCulture) + ", " + ex.ToString(), "DccServerConnection");
        throw;
      } finally {
        Status = ConnectionStatus.Disconnected;
        if (_chatListener != null) {
          _chatListener.Stop();
          _chatListener = null;
        }
        OnDisconnected(new ConnectionDataEventArgs(disconnectReason));
      }
    }

    #endregion
  }
}
