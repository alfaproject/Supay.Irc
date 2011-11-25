using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Contains basic information about a server.
  /// </summary>
  [Serializable]
  public class ServerInfoMessage : NumericMessage
  {
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
      : base(004)
    {
    }

    /// <summary>
    ///   Gets or sets the name of the server being referenced.
    /// </summary>
    public virtual string ServerName
    {
      get
      {
        return this.serverName;
      }
      set
      {
        this.serverName = value;
      }
    }

    /// <summary>
    ///   Gets or sets the version of the server.
    /// </summary>
    public virtual string Version
    {
      get
      {
        return this.version;
      }
      set
      {
        this.version = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user modes supported by this server.
    /// </summary>
    public virtual string UserModes
    {
      get
      {
        return this.userModes;
      }
      set
      {
        this.userModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel modes supported by this server.
    /// </summary>
    public virtual string ChannelModes
    {
      get
      {
        return this.channelModes;
      }
      set
      {
        this.channelModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel modes that require a parameter.
    /// </summary>
    public virtual string ChannelModesWithParams
    {
      get
      {
        return this.channelModesWithParams;
      }
      set
      {
        this.channelModesWithParams = value;
      }
    }

    /// <summary>
    ///   Gets or sets the user modes that require a parameter.
    /// </summary>
    public virtual string UserModesWithParams
    {
      get
      {
        return this.userModesWithParams;
      }
      set
      {
        this.userModesWithParams = value;
      }
    }

    /// <summary>
    ///   Gets or sets the server modes supported by this server.
    /// </summary>
    public virtual string ServerModes
    {
      get
      {
        return this.serverModes;
      }
      set
      {
        this.serverModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the server modes which require parameters.
    /// </summary>
    public virtual string ServerModesWithParams
    {
      get
      {
        return this.serverModesWithParams;
      }
      set
      {
        this.serverModesWithParams = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.ServerName);
      parameters.Add(this.Version);
      parameters.Add(this.UserModes);
      parameters.Add(this.ChannelModes);
      if (!string.IsNullOrEmpty(this.ChannelModesWithParams))
      {
        parameters.Add(this.ChannelModesWithParams);
        if (!string.IsNullOrEmpty(this.UserModesWithParams))
        {
          parameters.Add(this.UserModesWithParams);
          if (!string.IsNullOrEmpty(this.ServerModes))
          {
            parameters.Add(this.ServerModes);
            if (!string.IsNullOrEmpty(this.ServerModesWithParams))
            {
              parameters.Add(this.ServerModesWithParams);
            }
          }
        }
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.ServerName = parameters[1];
      this.Version = parameters[2];
      this.UserModes = parameters[3];
      this.ChannelModes = parameters[4];

      int pCount = parameters.Count;

      if (pCount > 5)
      {
        this.ChannelModesWithParams = parameters[5];
        if (pCount > 6)
        {
          this.UserModesWithParams = parameters[6];

          if (pCount > 7)
          {
            this.ServerModes = parameters[7];

            if (pCount > 8)
            {
              this.ServerModesWithParams = parameters[8];
            }
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnServerInfo(new IrcMessageEventArgs<ServerInfoMessage>(this));
    }
  }
}
