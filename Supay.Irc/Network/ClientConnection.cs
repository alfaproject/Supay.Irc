using System;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Supay.Irc.Properties;

namespace Supay.Irc.Network
{
    /// <summary>
    /// Provides client connections for IRC network services.
    /// </summary>
    /// <remarks>
    /// The <see cref="ClientConnection"/> class provides simple methods for connecting, sending, and receiving
    /// messages over an IRC network.
    /// </remarks>
    public class ClientConnection
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;

        private string host;
        private int port;
        private bool ssl;
        private Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnection"/> class with the specified port, and the
        /// specified host set.
        /// </summary>
        /// <param name="host">The DNS name of the remote host to which you intend to connect.</param>
        /// <param name="port">The port number of the remote host to which you intend to connect.</param>
        public ClientConnection(string host, int port)
        {
            this.Status = ConnectionStatus.Disconnected;
            this.Encoding = Encoding.ASCII;
            this.Ssl = false;

            this.Host = host;
            this.Port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConnection"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor creates a new <see cref="ClientConnection"/> with host set to 'localhost', and port set to
        /// 6667.
        /// </remarks>
        public ClientConnection()
            : this("localhost", 6667)
        {
        }

        /// <summary>
        /// Occurs when starting the connecting sequence to a server.
        /// </summary>
        public event EventHandler Connecting;

        /// <summary>
        /// Occurs after the connecting sequence is successful.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        /// Occurs when the disconnecting sequence is successful.
        /// </summary>
        public event EventHandler<ConnectionDataEventArgs> Disconnected;

        /// <summary>
        /// Occurs when the <see cref="ClientConnection"/> receives data.
        /// </summary>
        public event EventHandler<ConnectionDataEventArgs> DataReceived;

        /// <summary>
        /// Occurs when the <see cref="ClientConnection"/> sends data.
        /// </summary>
        public event EventHandler<ConnectionDataEventArgs> DataSent;

        /// <summary>
        /// Gets or sets the internet host which the current <see cref="ClientConnection"/> uses.
        /// </summary>
        /// <exception cref="NotSupportedException"><see cref="Status"/> isn't <see cref="ConnectionStatus.Disconnected"/>.</exception>
        public string Host
        {
            get
            {
                return this.host;
            }
            set
            {
                if (this.Status != ConnectionStatus.Disconnected)
                {
                    throw new NotSupportedException(Resources.AddressCannotBeChanged);
                }
                
                this.host = value;
            }
        }

        /// <summary>
        /// Gets or sets the port which the <see cref="ClientConnection"/> will communicate over.
        /// </summary>
        /// <remarks>
        /// The <see cref="Port"/> is generally between 6667 and 7000.
        /// </remarks>
        /// <exception cref="NotSupportedException"><see cref="Status"/> isn't <see cref="ConnectionStatus.Disconnected"/>.</exception>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (this.Status != ConnectionStatus.Disconnected)
                {
                    throw new NotSupportedException(Resources.PortCannotBeChanged);
                }

                this.port = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connection will use SSL to connect to the server.
        /// </summary>
        /// <exception cref="NotSupportedException"><see cref="Status"/> isn't <see cref="ConnectionStatus.Disconnected"/>.</exception>
        public bool Ssl
        {
            get
            {
                return this.ssl;
            }
            set
            {
                if (this.Status != ConnectionStatus.Disconnected)
                {
                    throw new NotSupportedException(Resources.SslCannotBeChanged);
                }

                this.ssl = value;
            }
        }

        /// <summary>
        /// Gets or sets the encoding used by the stream reader and writer.
        /// </summary>
        /// <remarks>
        /// Generally, only ASCII and UTF-8 are supported.
        /// </remarks>
        /// <exception cref="NotSupportedException"><see cref="Status"/> isn't <see cref="ConnectionStatus.Disconnected"/>.</exception>
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                if (this.Status != ConnectionStatus.Disconnected)
                {
                    throw new NotSupportedException(Resources.EncodingCannotBeChanged);
                }

                this.encoding = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ConnectionStatus"/> of the <see cref="ClientConnection"/>.
        /// </summary>
        public ConnectionStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a network connection to the current <see cref="Host"/> and <see cref="Port"/>.
        /// </summary>
        /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Status"/> isn't <see cref="ConnectionStatus.Disconnected"/>.</exception>
        public virtual async Task Connect()
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
                await this.client.ConnectAsync(this.Host, this.Port);

                // get stream
                Stream dataStream = this.client.GetStream();
                if (this.Ssl)
                {
                    dataStream = new SslStream(dataStream, false, (sender, cert, chain, errors) => errors == SslPolicyErrors.None);
                    await (dataStream as SslStream).AuthenticateAsClientAsync(this.Host);
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
        /// Closes the current network connection.
        /// </summary>
        public virtual void Disconnect()
        {
            this.Status = ConnectionStatus.Disconnected;

            if (this.client != null)
            {
                this.client.Close();
            }
        }

        /// <summary>
        /// Sends the given string over the network.
        /// </summary>
        /// <param name="message">The <see cref="string"/> to send.</param>
        /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">The connection can't be written to yet.</exception>
        public virtual async Task Write(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (this.writer == null || this.writer.BaseStream == null || !this.writer.BaseStream.CanWrite)
            {
                throw new InvalidOperationException(Resources.ConnectionCanNotBeWrittenToYet);
            }

            message = message.Replace(@"\c", "\x03").Replace(@"\b", "\x02").Replace(@"\u", "\x1F");
            if (!message.EndsWith("\r\n", StringComparison.Ordinal))
            {
                message += "\r\n";
            }

            //// TODO: validate message length accordingly to the server settings
            ////if (data.Length > 512) {
            ////  throw new Messages.InvalidMessageException(Properties.Resources.MessagesAreLimitedInSize, data);
            ////}

            try
            {
                await this.writer.WriteAsync(message);
                await this.writer.FlushAsync();
                this.OnDataSent(new ConnectionDataEventArgs(message));
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Couldn't send '" + message + "':\n" + ex);
                throw;
            }
        }

        /// <summary>
        /// Raises the <see cref="Connecting"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnConnecting(EventArgs e)
        {
            var handler = this.Connecting;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Connected"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnConnected(EventArgs e)
        {
            var handler = this.Connected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">A <see cref="ConnectionDataEventArgs"/> that contains the event data.</param>
        protected virtual void OnDisconnected(ConnectionDataEventArgs e)
        {
            var handler = this.Disconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DataReceived"/> event.
        /// </summary>
        /// <param name="e">A <see cref="ConnectionDataEventArgs"/> that contains the event data.</param>
        protected virtual void OnDataReceived(ConnectionDataEventArgs e)
        {
            var handler = this.DataReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DataSent"/> event.
        /// </summary>
        /// <param name="e">A <see cref="ConnectionDataEventArgs"/> that contains the event data.</param>
        protected virtual void OnDataSent(ConnectionDataEventArgs e)
        {
            var handler = this.DataSent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
