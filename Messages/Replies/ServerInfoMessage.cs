using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Contains basic information about a server.
  /// </summary>
  [Serializable]
  public class ServerInfoMessage : NumericMessage {
    private string channelModes = string.Empty;
    private string channelModesWithParams = string.Empty;
    private string serverModes = string.Empty;
    private string serverModesWithParams = string.Empty;
    private string serverName = string.Empty;
    private string userModes = string.Empty;
    private string userModesWithParams = string.Empty;
    private string version = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="ServerInfoMessage" /> class.
    /// </summary>
    public ServerInfoMessage()
      : base(004) {
    }

    /// <summary>
    ///   Gets or sets the name of the server being referenced.
    /// </summary>
    public virtual string ServerName {
      get {
        return serverName;
      }
      set {
        serverName = value;
      }
    }

    /// <summary>
    ///   Gets or sets the version of the server.
    /// </summary>
    public virtual string Version {
      get {
        return version;
      }
      set {
        version = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user modes supported by this server.
    /// </summary>
    public virtual string UserModes {
      get {
        return userModes;
      }
      set {
        userModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel modes supported by this server.
    /// </summary>
    public virtual string ChannelModes {
      get {
        return channelModes;
      }
      set {
        channelModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel modes that require a parameter.
    /// </summary>
    public virtual string ChannelModesWithParams {
      get {
        return channelModesWithParams;
      }
      set {
        channelModesWithParams = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user modes that require a parameter.
    /// </summary>
    public virtual string UserModesWithParams {
      get {
        return userModesWithParams;
      }
      set {
        userModesWithParams = value;
      }
    }

    /// <summary>
    ///   Gets or sets the server modes supported by this server.
    /// </summary>
    public virtual string ServerModes {
      get {
        return serverModes;
      }
      set {
        serverModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the server modes which require parameters.
    /// </summary>
    public virtual string ServerModesWithParams {
      get {
        return serverModesWithParams;
      }
      set {
        serverModesWithParams = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(ServerName);
      parameters.Add(Version);
      parameters.Add(UserModes);
      parameters.Add(ChannelModes);
      if (!string.IsNullOrEmpty(ChannelModesWithParams)) {
        parameters.Add(ChannelModesWithParams);
        if (!string.IsNullOrEmpty(UserModesWithParams)) {
          parameters.Add(UserModesWithParams);
          if (!string.IsNullOrEmpty(ServerModes)) {
            parameters.Add(ServerModes);
            if (!string.IsNullOrEmpty(ServerModesWithParams)) {
              parameters.Add(ServerModesWithParams);
            }
          }
        }
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      ServerName = parameters[1];
      Version = parameters[2];
      UserModes = parameters[3];
      ChannelModes = parameters[4];

      int pCount = parameters.Count;

      if (pCount > 5) {
        ChannelModesWithParams = parameters[5];
        if (pCount > 6) {
          UserModesWithParams = parameters[6];

          if (pCount > 7) {
            ServerModes = parameters[7];

            if (pCount > 8) {
              ServerModesWithParams = parameters[8];
            }
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnServerInfo(new IrcMessageEventArgs<ServerInfoMessage>(this));
    }
  }
}
