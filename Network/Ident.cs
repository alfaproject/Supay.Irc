using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Supay.Irc.Network {
  /// <summary>
  ///   An Ident daemon which is still used by some
  ///   IRC networks for authentication.
  /// </summary>
  [DesignerCategory("Code")]
  public sealed class Ident : Component {
    private static readonly Ident _instance = new Ident();

    private const string REPLY = " : USERID : UNIX : ";
    private const int PORT = 113;

    private readonly object _syncLock = new object();
    private TcpListener _listener;
    private Thread _socketThread;
    private bool _stopAfter;

    private Ident() {
      User = new User();
      Status = ConnectionStatus.Disconnected;
      _stopAfter = true;
    }

    /// <summary>
    ///   The singleton Ident service.
    /// </summary>
    public static Ident Service {
      get {
        return _instance;
      }
    }

    /// <summary>
    ///   Gets or sets the <see cref="Supay.Irc.User" /> to respond to an ident request with.
    /// </summary>
    public User User {
      get;
      set;
    }

    /// <summary>
    ///   Gets the status of the Ident service.
    /// </summary>
    public ConnectionStatus Status {
      get;
      private set;
    }

    /// <summary>
    ///   Starts the Ident server.
    /// </summary>
    /// <param name="stopAfterFirstAnswer">If true, Ident will stop immediately after answering. If false, will continue until <see cref="Ident.Stop" /> is called.</param>
    public void Start(bool stopAfterFirstAnswer = false) {
      lock (_syncLock) {
        if (Status != ConnectionStatus.Disconnected) {
          Trace.WriteLine("Ident Already Started");
          return;
        }

        _stopAfter = stopAfterFirstAnswer;
        _socketThread = new Thread(Run) {
          Name = "Identd",
          IsBackground = true
        };
        _socketThread.Start();
      }
    }

    /// <summary>
    ///   Stops the Ident server.
    /// </summary>
    public void Stop() {
      lock (_syncLock) {
        Status = ConnectionStatus.Disconnected;
        if (_listener != null) {
          _listener.Stop();
        }
      }
    }

    private void Run() {
      Status = ConnectionStatus.Connecting;

      try {
        _listener = new TcpListener(IPAddress.Any, PORT);
        _listener.Start();
      } catch (Exception ex) {
        Trace.WriteLine("Error Opening Ident Listener On Port " + PORT.ToString(CultureInfo.InvariantCulture) + ", " + ex.ToString(), "Ident");
        Status = ConnectionStatus.Disconnected;
        throw;
      }

      try {
        while (Status != ConnectionStatus.Disconnected) {
          try {
            TcpClient client = _listener.AcceptTcpClient();
            Status = ConnectionStatus.Connected;

            //Read query
            var reader = new StreamReader(client.GetStream());
            string identRequest = reader.ReadLine();

            //Send back reply
            var writer = new StreamWriter(client.GetStream());
            string identName = User.Username;
            if (identName.Length == 0) {
              if (User.Nickname.Length != 0) {
                identName = User.Nickname;
              } else {
                identName = "supay";
              }
            }
            string identReply = identRequest.Trim() + REPLY + identName.ToLower(CultureInfo.InvariantCulture);
            writer.WriteLine(identReply);
            writer.Flush();

            //Close connection with client
            client.Close();

            if (_stopAfter) {
              Status = ConnectionStatus.Disconnected;
            }
          } catch (IOException ex) {
            Trace.WriteLine("Error Processing Ident Request: " + ex.Message, "Ident");
          }
        }
      } catch (SocketException ex) {
        switch ((SocketError) ex.ErrorCode) {
          case SocketError.InterruptedFunctionCall:
            Trace.WriteLine("Ident Stopped By Thread Abort", "Ident");
            break;
          default:
            Trace.WriteLine("Ident Abnormally Stopped: " + ex.ToString(), "Ident");
            break;
        }
        //throw( ex );
      }

      _listener.Stop();
    }
  }
}
