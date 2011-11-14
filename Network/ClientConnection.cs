using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Supay.Irc.Properties;

namespace Supay.Irc.Network
{
  /// <summary>
  ///   Represents a network connection to an IRC server.
  /// </summary>
  /// <remarks>
  ///   Use the <see cref="ClientConnection" /> class to send a <see cref="Supay.Irc.Messages.IrcMessage" />
  ///   to an IRC server, and to be notified when it returns a <see cref="Supay.Irc.Messages.IrcMessage" />.
  /// </remarks>
  [DesignerCategory("Code")]
  public class ClientConnection : Component
  {
    private readonly object syncLock = new object();

    private string address;

    private TcpClient client;
    private Encoding encoding;
    private int port;
    private StreamReader reader;
    private bool ssl;
    private Thread worker;
    private StreamWriter writer;

    #region Nested type: SyncInvoke

    private delegate void SyncInvoke();

    #endregion

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="ClientConnection" /> class.
    /// </summary>
    /// <remarks>
    ///   With this constructor, the <see cref="Address" /> defaults to localhost,
    ///   and the <see cref="Port" /> defaults to 6667.
    /// </remarks>
    public ClientConnection()
      : this("localhost", 6667)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="ClientConnection" /> class with the given address on the given port.
    /// </summary>
    /// <param name="address">The network address to connect to.</param>
    /// <param name="port">The port to connect on.</param>
    public ClientConnection(string address, int port)
    {
      this.Status = ConnectionStatus.Disconnected;
      this.Encoding = Encoding.ASCII;
      this.Ssl = false;
      this.Address = address;
      this.Port = port;
    }

    #endregion

    #region Events

    /// <summary>
    ///   Occurs when the <see cref="ClientConnection" /> recieves data.
    /// </summary>
    internal event EventHandler<ConnectionDataEventArgs> DataReceived;

    /// <summary>
    ///   Occurs when the <see cref="ClientConnection" /> sends data.
    /// </summary>
    internal event EventHandler<ConnectionDataEventArgs> DataSent;

    /// <summary>
    ///   Occurs when starting the connecting sequence to a server.
    /// </summary>
    public event EventHandler Connecting;

    /// <summary>
    ///   Occurs after the connecting sequence is successful.
    /// </summary>
    public event EventHandler Connected;

    /// <summary>
    ///   Occurs when the disconnecting sequence is successful.
    /// </summary>
    public event EventHandler<ConnectionDataEventArgs> Disconnected;

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets the internet address which the current <see cref="ClientConnection" /> uses.
    /// </summary>
    /// <remarks>
    ///   A <see cref="NotSupportedException" /> will be thrown if an attempt is made to change
    ///   the <see cref="ClientConnection.Address" /> if the <see cref="ClientConnection.Status" />
    ///   is not <see cref="ConnectionStatus.Disconnected" />.
    /// </remarks>
    public string Address
    {
      get
      {
        return this.address;
      }
      set
      {
        if (this.Status == ConnectionStatus.Disconnected)
        {
          this.address = value;
        }
        else
        {
          throw new NotSupportedException(Resources.AddressCannotBeChanged);
        }
      }
    }

    /// <summary>
    ///   Gets or sets the port which the <see cref="ClientConnection" /> will communicate over.
    /// </summary>
    /// <remarks>
    ///   <para>For IRC, the <see cref="Port" /> is generally between 6667 and 7000.</para>
    ///   <para>A <see cref="NotSupportedException" /> will be thrown if an attempt is made to change
    ///     the <see cref="ClientConnection.Port" /> if the <see cref="ClientConnection.Status" />
    ///     is not <see cref="ConnectionStatus.Disconnected" />.</para>
    /// </remarks>
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
    ///   Gets the <see cref="ConnectionStatus" /> of the <see cref="ClientConnection" />.
    /// </summary>
    public ConnectionStatus Status
    {
      get;
      private set;
    }

    /// <summary>
    ///   Gets or sets the <see cref="ISynchronizeInvoke" /> implementor which
    ///   will be used to synchronize threads and events.
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
    ///   Gets or sets the encoding used by stream reader and writer.
    /// </summary>
    /// <remarks>
    ///   Generally, only ASCII and UTF-8 are supported.
    /// </remarks>
    public Encoding Encoding
    {
      get
      {
        return this.encoding;
      }
      set
      {
        if (this.Status == ConnectionStatus.Disconnected)
        {
          this.encoding = value;
        }
        else
        {
          throw new NotSupportedException(Resources.EncodingCannotBeChanged);
        }
      }
    }

    /// <summary>
    ///   Gets or sets if the connection will use SSL to connect to the server.
    /// </summary>
    public bool Ssl
    {
      get
      {
        return this.ssl;
      }
      set
      {
        if (this.Status == ConnectionStatus.Disconnected)
        {
          this.ssl = value;
        }
        else
        {
          throw new NotSupportedException(Resources.SslCannotBeChanged);
        }
      }
    }

    #endregion

    #region Methods

    /// <summary>
    ///   Creates a network connection to the current <see cref="ClientConnection.Address" />
    ///   and <see cref="ClientConnection.Port" />.
    /// </summary>
    /// <remarks>
    ///   Only use this overload if your application is not a Windows.Forms application, you've set
    ///   the <see cref="SynchronizationObject" /> property, or you want to handle threading issues yourself.
    /// </remarks>
    public void Connect()
    {
      lock (this.syncLock)
      {
        if (this.Status != ConnectionStatus.Disconnected)
        {
          throw new InvalidOperationException(Resources.AlreadyConnected);
        }

        this.Status = ConnectionStatus.Connecting;
        this.OnConnecting(EventArgs.Empty);
      }

      this.worker = new Thread(this.ReceiveData) {
        IsBackground = true
      };
      this.worker.Start();
    }

    /// <summary>
    ///   Creates a network connection to the current <see cref="ClientConnection.Address" />
    ///   and <see cref="ClientConnection.Port" />.
    /// </summary>
    /// <remarks>
    ///   <p>When using this class from an application, you need to pass in a control so that
    ///     data-receiving thread can sync with your application.</p>
    ///   <p>If calling this from a form or other control, just pass in the current instance.</p>
    /// </remarks>
    public void Connect(ISynchronizeInvoke syncObject)
    {
      this.SynchronizationObject = syncObject;
      this.Connect();
    }

    /// <summary>
    ///   Closes the current network connection.
    /// </summary>
    public void Disconnect()
    {
      this.Status = ConnectionStatus.Disconnected;
    }

    /// <summary>
    ///   Forces closing the current network connection and kills the thread running it.
    /// </summary>
    public void DisconnectForce()
    {
      this.Disconnect();
      if (this.worker != null && this.worker.IsAlive)
      {
        this.worker.Abort();
      }
    }

    /// <summary>
    ///   Sends the given string over the network.
    /// </summary>
    /// <param name="data">The <see cref="System.string" /> to send.</param>
    public void Write(string data)
    {
      if (string.IsNullOrEmpty(data))
      {
        return;
      }

      if (this.writer == null || this.writer.BaseStream == null || !this.writer.BaseStream.CanWrite)
      {
        throw new InvalidOperationException(Resources.ConnectionCanNotBeWrittenToYet);
      }

      data = data.Replace("\\c", "\x0003").Replace("\\b", "\x0002").Replace("\\u", "\x001F");
      if (!data.EndsWith("\r\n", StringComparison.Ordinal))
      {
        data += "\r\n";
      }

      ////if (data.Length > 512) {
      ////  throw new Supay.Irc.Messages.InvalidMessageException(Properties.Resources.MessagesAreLimitedInSize, data);
      ////}

      try
      {
        this.writer.WriteLine(data);
        this.writer.Flush();
        this.OnDataSent(new ConnectionDataEventArgs(data));
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Couldn't Send '" + data + "'. " + ex);
        throw;
      }
    }

    #endregion

    #region Protected Event Raisers

    /// <summary>
    ///   Raises the <see cref="ClientConnection.Connecting" /> event of the <see cref="ClientConnection" /> object.
    /// </summary>
    protected void OnConnecting(EventArgs e)
    {
      if (this.Connecting != null)
      {
        this.Connecting(this, e);
      }
    }

    /// <summary>
    ///   Raises the <see cref="ClientConnection.Connected" /> event of the <see cref="ClientConnection" /> object.
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
    ///   Raises the <see cref="ClientConnection.DataReceived" /> event of the <see cref="ClientConnection" /> object.
    /// </summary>
    /// <param name="e">A <see cref="ConnectionDataEventArgs" /> that contains the data.</param>
    protected void OnDataReceived(ConnectionDataEventArgs e)
    {
      if (this.SynchronizationObject != null && this.SynchronizationObject.InvokeRequired)
      {
        SyncInvoke del = () => this.OnDataReceived(e);
        this.SynchronizationObject.Invoke(del, null);
        return;
      }

      if (this.DataReceived != null)
      {
        this.DataReceived(this, e);
      }
    }

    /// <summary>
    ///   Raises the <see cref="ClientConnection.DataSent" /> event of the <see cref="ClientConnection" /> object.
    /// </summary>
    /// <param name="data">A <see cref="ConnectionDataEventArgs" /> that contains the data.</param>
    protected void OnDataSent(ConnectionDataEventArgs data)
    {
      if (this.DataSent != null)
      {
        this.DataSent(this, data);
      }
    }

    /// <summary>
    ///   Raises the <see cref="ClientConnection.Disconnected" /> event of the <see cref="ClientConnection" /> object.
    /// </summary>
    protected void OnDisconnected(ConnectionDataEventArgs e)
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

    #region Private

    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      // Do not allow this client to communicate with unauthenticated servers.
      return sslPolicyErrors == SslPolicyErrors.None;
    }

    /// <summary>
    ///   This method listens for data over the network until the Connection.State is Disconnected.
    /// </summary>
    /// <remarks>
    ///   ReceiveData runs in its own thread.
    /// </remarks>
    private void ReceiveData()
    {
      try
      {
        this.client = new TcpClient(this.Address, this.Port);
        Stream dataStream;
        if (this.Ssl)
        {
          dataStream = new SslStream(this.client.GetStream(), false, ValidateServerCertificate, null);
          ((SslStream) dataStream).AuthenticateAsClient(this.Address);
        }
        else
        {
          dataStream = this.client.GetStream();
        }

        this.reader = new StreamReader(dataStream, this.Encoding);
        this.writer = new StreamWriter(dataStream, this.Encoding) {
          AutoFlush = true
        };
      }
      catch (AuthenticationException e)
      {
        if (this.client != null)
        {
          this.client.Close();
        }
        this.Status = ConnectionStatus.Disconnected;
        this.OnDisconnected(new ConnectionDataEventArgs(e.Message));
        return;
      }
      catch (Exception ex)
      {
        this.Status = ConnectionStatus.Disconnected;
        this.OnDisconnected(new ConnectionDataEventArgs(ex.Message));
        return;
      }

      this.Status = ConnectionStatus.Connected;
      this.OnConnected(EventArgs.Empty);

      string disconnectReason = string.Empty;

      try
      {
        string incomingMessageLine;

        while (this.Status == ConnectionStatus.Connected && ((incomingMessageLine = this.reader.ReadLine()) != null))
        {
          try
          {
            incomingMessageLine = incomingMessageLine.Trim();
            this.OnDataReceived(new ConnectionDataEventArgs(incomingMessageLine));
          }
          catch (ThreadAbortException ex)
          {
            Trace.WriteLine(ex.Message);
            Thread.ResetAbort();
            disconnectReason = "Thread Aborted";
            break;
          }
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
        disconnectReason = ex.Message + Environment.NewLine + ex.StackTrace;
      }
      this.Status = ConnectionStatus.Disconnected;

      this.client.Close();
      this.client = null;

      var disconnectArgs = new ConnectionDataEventArgs(disconnectReason);
      this.OnDisconnected(disconnectArgs);
    }

    #endregion
  }
}
