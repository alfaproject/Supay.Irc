using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Supay.Irc.Messages;
using Supay.Irc.Network;
using Supay.Irc.Properties;

namespace Supay.Irc.Dcc
{
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
  public class DccServerConnection : Component
  {
    private readonly object syncLock = new object();
    private TcpListener chatListener;
    private Thread connectionWorker;
    private int port;
    private Timer timeoutTimer;

    #region Nested type: SyncInvoke

    private delegate void SyncInvoke();

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="DccServerConnection" /> class.
    /// </summary>
    public DccServerConnection()
    {
      this.Status = ConnectionStatus.Disconnected;
      this.TimeOut = TimeSpan.Zero;
      this.Transfer = new DccTransfer();
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="DccServerConnection" /> class on the given
    ///   port.
    /// </summary>
    /// <param name="port">The port to listen on.</param>
    public DccServerConnection(int port)
      : this()
    {
      this.Port = port;
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
    protected void OnConnecting(EventArgs e)
    {
      if (this.Connecting != null)
      {
        this.Connecting(this, e);
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
    protected void OnConnected(EventArgs e)
    {
      if (this.SynchronizationObject != null && this.SynchronizationObject.InvokeRequired)
      {
        SyncInvoke del = () => this.OnConnected(e);
        this.SynchronizationObject.Invoke(del, null);
        return;
      }

      if (this.Connected != null)
      {
        this.Connected(this, e);
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
    protected virtual void OnDisconnected(ConnectionDataEventArgs e)
    {
      if (this.SynchronizationObject != null && this.SynchronizationObject.InvokeRequired)
      {
        SyncInvoke del = () => this.OnDisconnected(e);
        this.SynchronizationObject.Invoke(del, null);
        return;
      }

      if (this.Disconnected != null)
      {
        this.Disconnected(this, e);
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
    public int Port
    {
      get
      {
        return this.port;
      }
      set
      {
        if (this.Status == ConnectionStatus.Disconnected)
        {
          this.port = value;
        }
        else
        {
          throw new NotSupportedException(Resources.PortCannotBeChanged);
        }
      }
    }

    /// <summary>
    ///   Gets or sets the length of time to wait after calling <see cref="Send" /> before the
    ///   thread will stop waiting for a connection.
    /// </summary>
    public TimeSpan TimeOut
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets the <see cref="ConnectionStatus" /> of the <see cref="DccServerConnection" />.
    /// </summary>
    public ConnectionStatus Status
    {
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
    public ISynchronizeInvoke SynchronizationObject
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets the transfer information for the connection.
    /// </summary>
    public DccTransfer Transfer
    {
      get;
      private set;
    }

    #endregion

    #region Public Methods

    /// <summary>
    ///   Attempts to send the file specified in the current
    ///   <see cref="DccServerConnection.Transfer" /> information.
    /// </summary>
    public void Send()
    {
      lock (this.syncLock)
      {
        if (this.Status != ConnectionStatus.Disconnected)
        {
          throw new InvalidOperationException(Resources.AlreadyConnectToAnotherClient);
        }

        this.Status = ConnectionStatus.Connecting;
        this.OnConnecting(EventArgs.Empty);
      }

      this.connectionWorker = new Thread(this.RunSend) {
        IsBackground = true
      };
      this.connectionWorker.Start();

      if (this.TimeOut != TimeSpan.Zero)
      {
        this.timeoutTimer = new Timer(this.CheckTimeOut, null, this.TimeOut, TimeSpan.Zero);
      }
    }

    /// <summary>
    ///   Closes the current network connection.
    /// </summary>
    public void Disconnect()
    {
      this.Status = ConnectionStatus.Disconnected;
      this.OnDisconnected(new ConnectionDataEventArgs("Disconnect Called"));
    }

    /// <summary>
    ///   Forces closing the current network connection and kills the thread running it.
    /// </summary>
    public void DisconnectForce()
    {
      this.Disconnect();
      if (this.connectionWorker != null && this.connectionWorker.IsAlive)
      {
        this.connectionWorker.Abort();
      }
    }

    #endregion

    #region Private Methods

    private void CheckTimeOut(object state)
    {
      if (this.Status == ConnectionStatus.Connecting)
      {
        this.DisconnectForce();
      }
      this.timeoutTimer.Dispose();
      this.timeoutTimer = null;
    }

    private void RunSend()
    {
      string disconnectReason = string.Empty;

      try
      {
        this.chatListener = new TcpListener(IPAddress.Any, this.Port);
        this.chatListener.Start();
        Socket socket = this.chatListener.AcceptSocket();

        this.Status = ConnectionStatus.Connected;
        this.OnConnected(EventArgs.Empty);

        this.Transfer.TransferSocket = socket;
        this.Transfer.Send();
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Error Opening DccServerConnection On Port " + this.port.ToString(CultureInfo.InvariantCulture) + ", " + ex, "DccServerConnection");
        throw;
      }
      finally
      {
        this.Status = ConnectionStatus.Disconnected;
        if (this.chatListener != null)
        {
          this.chatListener.Stop();
          this.chatListener = null;
        }
        this.OnDisconnected(new ConnectionDataEventArgs(disconnectReason));
      }
    }

    #endregion
  }
}
