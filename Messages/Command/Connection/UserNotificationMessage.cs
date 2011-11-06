using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The UserNotificationMessage is used at the beginning of connection to specify the username, hostname and real name of a new user.
  /// </summary>
  [Serializable]
  public class UserNotificationMessage : CommandMessage {
    private bool initialInvisibility = true;
    private bool initialWallops;
    private string realName = string.Empty;
    private string userName = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "USER";
      }
    }

    /// <summary>
    ///   Gets or sets the UserName of client.
    /// </summary>
    public virtual string UserName {
      get {
        return userName;
      }
      set {
        userName = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the client is initialized with a user mode of invisible.
    /// </summary>
    public virtual bool InitialInvisibility {
      get {
        return initialInvisibility;
      }
      set {
        initialInvisibility = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the client is initialized with a user mode of receiving wallops.
    /// </summary>
    public virtual bool InitialWallops {
      get {
        return initialWallops;
      }
      set {
        initialWallops = value;
      }
    }

    /// <summary>
    ///   Gets or sets the real name of the client.
    /// </summary>
    public virtual string RealName {
      get {
        return realName;
      }
      set {
        realName = value;
      }
    }

    /// <exclude />
    public override bool CanParse(string unparsedMessage) {
      if (!base.CanParse(unparsedMessage)) {
        return false;
      }
      Collection<string> p = MessageUtil.GetParameters(unparsedMessage);
      int tempInt;
      if (p.Count != 4 || !int.TryParse(p[1], out tempInt) || p[2] != "*") {
        return false;
      }
      return true;
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      Collection<string> parameters = base.GetParameters();
      parameters.Add(UserName);
      int modeBitMask = 0;
      if (InitialInvisibility) {
        modeBitMask += 8;
      }
      if (InitialWallops) {
        modeBitMask += 4;
      }
      parameters.Add(modeBitMask.ToString(CultureInfo.InvariantCulture));
      parameters.Add("*");
      parameters.Add(RealName);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);
      if (parameters.Count >= 4) {
        UserName = parameters[0];
        RealName = parameters[3];
        int modeBitMask = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);
        InitialInvisibility = ((modeBitMask & 8) == 8);
        InitialWallops = ((modeBitMask & 4) == 4);
      } else {
        UserName = string.Empty;
        RealName = string.Empty;
        InitialInvisibility = true;
        InitialWallops = false;
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnUserNotification(new IrcMessageEventArgs<UserNotificationMessage>(this));
    }
  }
}
