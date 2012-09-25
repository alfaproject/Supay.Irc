using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
        private string address;

        private TcpClient client;
        private Encoding encoding;
        private int port;
        private StreamReader reader;
        private bool ssl;
        private StreamWriter writer;


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
        public async Task Connect()
        {
            if (this.Status != ConnectionStatus.Disconnected)
            {
                throw new InvalidOperationException(Resources.AlreadyConnected);
            }

            this.Status = ConnectionStatus.Connecting;
            this.OnConnecting(EventArgs.Empty);

            try
            {
                // connect
                this.client = new TcpClient();
                await this.client.ConnectAsync(this.Address, this.Port);

                // get stream
                Stream dataStream = this.client.GetStream();
                if (this.Ssl)
                {
                    dataStream = new SslStream(dataStream, false, ValidateServerCertificate);
                    await ((SslStream) dataStream).AuthenticateAsClientAsync(this.Address);
                }

                this.reader = new StreamReader(dataStream, this.Encoding);
                this.writer = new StreamWriter(dataStream, this.Encoding);

                this.Status = ConnectionStatus.Connected;
                this.OnConnected(EventArgs.Empty);

                // listen for messages
                string message;
                while ((message = await this.reader.ReadLineAsync()) != null)
                {
                    this.OnDataReceived(new ConnectionDataEventArgs(message.Trim()));
                }
            }
            catch (AuthenticationException ex)
            {
                this.Status = ConnectionStatus.Disconnected;
                this.OnDisconnected(new ConnectionDataEventArgs(ex.Message));
            }
            catch (ObjectDisposedException)
            {
                this.Status = ConnectionStatus.Disconnected;
                this.OnDisconnected(new ConnectionDataEventArgs("Disconnected"));
            }
            catch (Exception ex)
            {
                var verboseException = ex.ToString();
                Trace.WriteLine(verboseException);

                this.Status = ConnectionStatus.Disconnected;
                this.OnDisconnected(new ConnectionDataEventArgs(verboseException));
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        ///   Closes the current network connection.
        /// </summary>
        public void Disconnect()
        {
            this.Status = ConnectionStatus.Disconnected;
            this.Dispose();
        }

        /// <summary>
        ///   Sends the given string over the network.
        /// </summary>
        /// <param name="data">The <see cref="System.string" /> to send.</param>
        public async Task Write(string data)
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
                await this.writer.WriteAsync(data);
                await this.writer.FlushAsync();
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
            if (this.DataReceived != null)
            {
                this.DataReceived(this, e);
            }
        }

        /// <summary>
        ///   Raises the <see cref="ClientConnection.DataSent" /> event of the <see cref="ClientConnection" /> object.
        /// </summary>
        /// <param name="e">A <see cref="ConnectionDataEventArgs" /> that contains the data.</param>
        protected void OnDataSent(ConnectionDataEventArgs e)
        {
            if (this.DataSent != null)
            {
                this.DataSent(this, e);
            }
        }

        /// <summary>
        ///   Raises the <see cref="ClientConnection.Disconnected" /> event of the <see cref="ClientConnection" /> object.
        /// </summary>
        protected void OnDisconnected(ConnectionDataEventArgs e)
        {
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

        #endregion


        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ClientConnection"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">Set to true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.client != null)
                {
                    this.client.Close();
                }
            }

            this.client = null;

            base.Dispose(disposing);
        }
    }
}
