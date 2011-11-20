using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Supay.Irc.Network
{
  /// <summary>
  ///   An Ident daemon which is still used by some
  ///   IRC networks for authentication.
  /// </summary>
  [DesignerCategory("Code")]
  public sealed class Ident : Component
  {
    private const string REPLY = " : USERID : UNIX : ";
    private const int PORT = 113;
    private static readonly Ident instance = new Ident();

    private readonly object syncLock = new object();
    private TcpListener listener;
    private Thread socketThread;
    private bool stopAfter;

    private Ident()
    {
      this.User = new User();
      this.Status = ConnectionStatus.Disconnected;
      this.stopAfter = true;
    }

    /// <summary>
    ///   The singleton Ident service.
    /// </summary>
    public static Ident Service
    {
      get
      {
        return instance;
      }
    }

    /// <summary>
    ///   Gets or sets the <see cref="Supay.Irc.User" /> to respond to an ident request with.
    /// </summary>
    public User User
    {
      get;
      set;
    }

    /// <summary>
    ///   Gets the status of the Ident service.
    /// </summary>
    public ConnectionStatus Status
    {
      get;
      private set;
    }

    /// <summary>
    /// Starts the Ident server.
    /// </summary>
    /// <param name="stopAfterFirstAnswer">If true, Ident will stop immediately after answering. If false, will continue until <see cref="Ident.Stop" /> is called.</param>
    public void Start(bool stopAfterFirstAnswer)
    {
      lock (this.syncLock)
      {
        if (this.Status != ConnectionStatus.Disconnected)
        {
          Trace.WriteLine("Ident Already Started");
          return;
        }

        this.stopAfter = stopAfterFirstAnswer;
        this.socketThread = new Thread(this.Run) {
          Name = "Identd",
          IsBackground = true
        };
        this.socketThread.Start();
      }
    }

    /// <summary>
    /// Starts the Ident server.
    /// </summary>
    public void Start()
    {
      this.Start(false);
    }

    /// <summary>
    ///   Stops the Ident server.
    /// </summary>
    public void Stop()
    {
      lock (this.syncLock)
      {
        this.Status = ConnectionStatus.Disconnected;
        if (this.listener != null)
        {
          this.listener.Stop();
        }
      }
    }

    private void Run()
    {
      this.Status = ConnectionStatus.Connecting;

      try
      {
        this.listener = new TcpListener(IPAddress.Any, PORT);
        this.listener.Start();
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Error Opening Ident Listener On Port " + PORT.ToString(CultureInfo.InvariantCulture) + ", " + ex, "Ident");
        this.Status = ConnectionStatus.Disconnected;
        throw;
      }

      try
      {
        while (this.Status != ConnectionStatus.Disconnected)
        {
          try
          {
            TcpClient client = this.listener.AcceptTcpClient();
            this.Status = ConnectionStatus.Connected;

            // Read query
            var reader = new StreamReader(client.GetStream());
            string identRequest = reader.ReadLine();
            if (identRequest != null)
            {
              // Send back reply
              var writer = new StreamWriter(client.GetStream());
              string identName = this.User.Username;
              if (identName.Length == 0)
              {
                identName = this.User.Nickname.Length != 0 ? this.User.Nickname : "Supay";
              }
              string identReply = identRequest.Trim() + REPLY + identName;
              writer.WriteLine(identReply);
              writer.Flush();
            }

            // Close connection with client
            client.Close();

            if (this.stopAfter)
            {
              this.Status = ConnectionStatus.Disconnected;
            }
          }
          catch (IOException ex)
          {
            Trace.WriteLine("Error Processing Ident Request: " + ex.Message, "Ident");
          }
        }
      }
      catch (SocketException ex)
      {
        switch ((SocketError) ex.ErrorCode)
        {
          case SocketError.InterruptedFunctionCall:
            Trace.WriteLine("Ident Stopped By Thread Abort", "Ident");
            break;
          default:
            Trace.WriteLine("Ident Abnormally Stopped: " + ex, "Ident");
            break;
        }
      }

      this.listener.Stop();
    }
  }
}
