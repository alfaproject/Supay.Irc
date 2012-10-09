using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Supay.Irc.Network
{
    /// <summary>
    /// An Ident daemon which is still used by some IRC networks for authentication.
    /// </summary>
    public sealed class Ident
    {
        private static readonly Lazy<Ident> instance = new Lazy<Ident>(() => new Ident());

        private TcpListener listener;

        private Ident()
        {
        }

        /// <summary>
        /// The singleton Ident service.
        /// </summary>
        public static Ident Service
        {
            get
            {
                return instance.Value;
            }
        }

        /// <summary>
        /// Gets or sets the user <see cref="Mask"/> to respond to an ident request with.
        /// </summary>
        public Mask User
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the status of the Ident service.
        /// </summary>
        public ConnectionStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Starts the Ident server.
        /// </summary>
        /// <param name="stopAfterFirstAnswer">If true, Ident will stop immediately after answering. If false, will continue until <see cref="Stop"/> is called.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Start(bool stopAfterFirstAnswer)
        {
            if (this.Status != ConnectionStatus.Disconnected)
            {
                Trace.WriteLine("Ident listener already started.", "Ident");
                return;
            }

            this.Status = ConnectionStatus.Connecting;

            try
            {
                this.listener = new TcpListener(IPAddress.Any, 113);
                this.listener.Start();

                while (this.Status != ConnectionStatus.Disconnected)
                {
                    using (var client = await this.listener.AcceptTcpClientAsync())
                    {
                        var stream = client.GetStream();
                        this.Status = ConnectionStatus.Connected;

                        // Read query
                        using (var reader = new StreamReader(stream))
                        {
                            var ports = await reader.ReadLineAsync();
                            if (ports != null)
                            {
                                using (var writer = new StreamWriter(stream))
                                {
                                    // Send back reply
                                    var userId = "Supay";
                                    if (this.User != null)
                                    {
                                        if (!string.IsNullOrEmpty(this.User.Username))
                                        {
                                            userId = this.User.Username;
                                        }
                                        else if (!string.IsNullOrEmpty(this.User.Nickname))
                                        {
                                            userId = this.User.Nickname;
                                        }
                                    }

                                    var response = ports + " : USERID : UNIX : " + userId;
                                    await writer.WriteLineAsync(response);
                                    await writer.FlushAsync();
                                }
                            }
                        }
                    }

                    if (stopAfterFirstAnswer)
                    {
                        this.Status = ConnectionStatus.Disconnected;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unexpected error on Ident listener: " + ex, "Ident");
                this.Status = ConnectionStatus.Disconnected;
                throw;
            }
            finally
            {
                this.listener.Stop();
            }
        }

        /// <summary>
        /// Starts the Ident server.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Start()
        {
            await this.Start(false);
        }

        /// <summary>
        /// Stops the Ident server.
        /// </summary>
        public void Stop()
        {
            this.Status = ConnectionStatus.Disconnected;
            if (this.listener != null)
            {
                this.listener.Stop();
            }
        }
    }
}
