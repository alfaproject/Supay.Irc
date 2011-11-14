using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   This message is sent directly after connecting, 
  ///   giving the client information about the server software in use.
  /// </summary>
  [Serializable]
  public class YourHostMessage : NumericMessage
  {
    private const string YOUR_HOST_IS = "Your host is ";
    private const string RUNNING_VERSION = ", running version ";
    private string serverName = string.Empty;
    private string version = string.Empty;

    /// <summary>
    ///   Creates a new instance of the <see cref="YourHostMessage" /> class.
    /// </summary>
    public YourHostMessage()
      : base(002)
    {
    }

    /// <summary>
    ///   Gets or sets the name of the software the server is running.
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
    ///   Gets or sets the version of the software the server is running.
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
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(YOUR_HOST_IS + this.ServerName + RUNNING_VERSION + this.Version);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      string reply = parameters[1];
      if (reply.IndexOf(YOUR_HOST_IS, StringComparison.Ordinal) != -1 && reply.IndexOf(RUNNING_VERSION, StringComparison.Ordinal) != -1)
      {
        int startOfServerName = YOUR_HOST_IS.Length;
        int startOfVersion = reply.IndexOf(RUNNING_VERSION, StringComparison.Ordinal) + RUNNING_VERSION.Length;
        int lengthOfServerName = reply.IndexOf(RUNNING_VERSION, StringComparison.Ordinal) - startOfServerName;

        this.ServerName = reply.Substring(startOfServerName, lengthOfServerName);
        this.Version = reply.Substring(startOfVersion);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnYourHost(new IrcMessageEventArgs<YourHostMessage>(this));
    }
  }
}
