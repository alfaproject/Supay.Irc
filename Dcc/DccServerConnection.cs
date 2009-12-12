using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;
using Supay.Irc.Network;

namespace Supay.Irc.Dcc {

  /// <summary>
  /// The DccServerConnection is used after sending a <see cref="Supay.Irc.Messages.DccSendRequestMessage"/> to send the file to the target.
  /// </summary>
  /// <remarks>
  /// The nature of dcc sending is such that this class will create a listening server on the given port.
  /// If the target of the <see cref="Supay.Irc.Messages.DccSendRequestMessage"/> decides to connect to the server, this class will send the file.
  /// Set a <see cref="DccServerConnection.TimeOut"/> to have the server stop after the given amount of time, 
  /// in case the target ignores the initial message.
  /// </remarks>
  [System.ComponentModel.DesignerCategory("Code")]
  public class DccServerConnection : Component {

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DccServerConnection"/> class.
    /// </summary>
    public DccServerConnection() {
      this.transfer = new DccTransfer();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DccServerConnection"/> class on the given port.
    /// </summary>
    /// <param name="port">The port to listen on.</param>
    public DccServerConnection(int port)
      : this() {
      Port = port;
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when starting the connecting sequence to a server
    /// </summary>
    public event EventHandler Connecting;

    /// <summary>
    /// Raises the <see cref="DccServerConnection.Connecting"/> event of the <see cref="DccServerConnection"/> object.
    /// </summary>
    protected virtual void OnConnecting(EventArgs e) {
      if (Connecting != null) {
        Connecting(this, e);
      }
    }

    /// <summary>
    /// Occurs after the connecting sequence is successful.
    /// </summary>
    public event EventHandler Connected;

    /// <summary>
    /// Raises the <see cref="DccServerConnection.Connected"/> event of the <see cref="DccServerConnection"/> object.
    /// </summary>
    protected virtual void OnConnected(EventArgs e) {
      if (this.synchronizationObject != null && this.synchronizationObject.InvokeRequired) {
        SyncInvoke del = delegate {
          this.OnConnected(e);
        };
        this.synchronizationObject.Invoke(del, null);
        return;
      }

      if (Connected != null) {
        Connected(this, e);
      }
    }

    /// <summary>
    /// Occurs when the disconnecting sequence is successful.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> Disconnected;

    /// <summary>
    /// Raises the <see cref="DccServerConnection.Disconnected"/> event of the <see cref="DccServerConnection"/> object.
    /// </summary>
    protected virtual void OnDisconnected(ConnectionDataEventArgs e) {
      if (this.synchronizationObject != null && this.synchronizationObject.InvokeRequired) {
        SyncInvoke del = delegate {
          this.OnDisconnected(e);
        };
        this.synchronizationObject.Invoke(del, null);
        return;
      }

      if (Disconnected != null) {
        Disconnected(this, e);
      }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the port which the <see cref="DccServerConnection"/> will communicate over.
    /// </summary>
    /// <remarks>
    /// <para>A <see cref="NotSupportedException"/> will be thrown if an attempt is made to change the <see cref="DccServerConnection.Port"/> if the <see cref="DccServerConnection.Status"/> is not <see cref="ConnectionStatus.Disconnected"/>.</para>
    /// </remarks>
    public int Port {
      get {
        return port;
      }
      set {
        if (this.Status == ConnectionStatus.Disconnected) {
          port = value;
        } else {
          throw new NotSupportedException(Properties.Resources.PortCannotBeChanged);
        }
      }
    }

    /// <summary>
    /// Gets or sets the length of time to wait after calling <see cref="Send"/> before the thread will stop waiting for a connection.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeOut")]
    public TimeSpan TimeOut {
      get {
        return timeOut;
      }
      set {
        timeOut = value;
      }
    }

    /// <summary>
    /// Gets the <see cref="ConnectionStatus"/> of the <see cref="DccServerConnection"/>.
    /// </summary>
    public ConnectionStatus Status {
      get {
        return status;
      }
      private set {
        this.status = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="ISynchronizeInvoke"/> implementor which will be used to synchronize threads and events.
    /// </summary>
    /// <remarks>
    /// This is usually the main form of the application.
    /// </remarks>
    public System.ComponentModel.ISynchronizeInvoke SynchronizationObject {
      get {
        return synchronizationObject;
      }
      set {
        synchronizationObject = value;
      }
    }

    /// <summary>
    /// Gets the transfer information for the connection.
    /// </summary>
    public DccTransfer Transfer {
      get {
        return transfer;
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Attempts to send the file specified in the current <see cref="DccServerConnection.Transfer"/> information.
    /// </summary>
    public virtual void Send() {
      lock (lockObject) {
        if (this.Status != ConnectionStatus.Disconnected) {
          throw new InvalidOperationException(Properties.Resources.AlreadyConnectToAnotherClient);
        }

        this.Status = ConnectionStatus.Connecting;
        this.OnConnecting(EventArgs.Empty);
      }

      connectionWorker = new Thread(new ThreadStart(RunSend));
      connectionWorker.IsBackground = true;
      connectionWorker.Start();

      if (this.TimeOut != TimeSpan.Zero) {
        timeoutTimer = new Timer(new TimerCallback(checkTimeOut), null, this.TimeOut, TimeSpan.Zero);
      }
    }

    /// <summary>
    /// Closes the current network connection.
    /// </summary>
    public virtual void Disconnect() {
      this.Status = ConnectionStatus.Disconnected;
      ConnectionDataEventArgs disconnectArgs = new ConnectionDataEventArgs("Disconnect Called");
      this.OnDisconnected(disconnectArgs);
    }

    /// <summary>
    /// Forces closing the current network connection and kills the thread running it.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    public virtual void DisconnectForce() {
      this.Disconnect();
      if (connectionWorker != null) {
        try {
          connectionWorker.Abort();
        } catch {
        }
      }
    }

    /// <summary>
    /// Releases the resources used by the <see cref="DccServerConnection"/>
    /// </summary>
    protected override void Dispose(bool disposing) {
      try {
        if (disposing) {
          if (this.chatListener != null) {
            ((IDisposable)this.chatListener).Dispose();
          }
          if (this.timeoutTimer != null) {
            ((IDisposable)this.timeoutTimer).Dispose();
          }
        }
      } finally {
        base.Dispose(disposing);
      }
    }

    #endregion

    #region Helpers

    private void checkTimeOut(object state) {
      if (this.Status == ConnectionStatus.Connecting) {
        this.DisconnectForce();
      }
      this.timeoutTimer.Dispose();
      this.timeoutTimer = null;
    }

    private void RunSend() {
      ConnectionDataEventArgs disconnectArgs;
      String disconnectReason = "";

      try {
        chatListener = new TcpListener(System.Net.IPAddress.Any, this.Port);
        chatListener.Start();
        Socket socket = chatListener.AcceptSocket();

        this.Status = ConnectionStatus.Connected;
        this.OnConnected(EventArgs.Empty);

        this.Transfer.TransferSocket = socket;
        this.Transfer.Send();

      } catch (Exception ex) {
        System.Diagnostics.Trace.WriteLine("Error Opening DccServerConnection On Port " + port.ToString(CultureInfo.InvariantCulture) + ", " + ex.ToString(), "DccServerConnection");
        throw;
      } finally {
        this.Status = ConnectionStatus.Disconnected;
        if (chatListener != null) {
          chatListener.Stop();
          chatListener = null;
        }
        disconnectArgs = new ConnectionDataEventArgs(disconnectReason);
        this.OnDisconnected(disconnectArgs);
      }

    }

    #endregion

    #region Private

    private Timer timeoutTimer;
    private object lockObject = new object();

    private DccTransfer transfer;
    private TimeSpan timeOut = TimeSpan.Zero;
    private int port;
    private ConnectionStatus status = ConnectionStatus.Disconnected;

    private TcpListener chatListener;
    private Thread connectionWorker;
    private System.ComponentModel.ISynchronizeInvoke synchronizationObject;
    private delegate void SyncInvoke();

    #endregion

  }

}