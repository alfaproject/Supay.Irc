using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   As a reply to the <see cref="WhoIsMessage" /> message,
  ///   carries information about idle time and such.
  /// </summary>
  [Serializable]
  public class WhoIsIdleReplyMessage : NumericMessage {
    private int idleTime;
    private string info = string.Empty;
    private string nick = string.Empty;
    private DateTime signOnTime = DateTime.Now;

    /// <summary>
    ///   Creates a new instance of the <see cref="WhoIsIdleReplyMessage" /> class.
    /// </summary>
    public WhoIsIdleReplyMessage()
      : base(317) {
    }

    /// <summary>
    ///   Gets or sets the nick of the user who is being examined.
    /// </summary>
    public virtual string Nick {
      get {
        return nick;
      }
      set {
        nick = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of seconds the user has been idle.
    /// </summary>
    public virtual int IdleLength {
      get {
        return idleTime;
      }
      set {
        idleTime = value;
      }
    }

    /// <summary>
    ///   Gets or sets the time the user signed on to their current server.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "SignOn")]
    public virtual DateTime SignOnTime {
      get {
        return signOnTime;
      }
      set {
        signOnTime = value;
      }
    }

    /// <summary>
    ///   Gets or sets some additional info about the user being examined.
    /// </summary>
    public virtual string Info {
      get {
        return info;
      }
      set {
        info = value;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters() {
      IList<string> parameters = base.GetParameters();
      parameters.Add(Nick);
      parameters.Add(IdleLength.ToString(CultureInfo.InvariantCulture));
      parameters.Add(MessageUtil.ConvertToUnixTime(SignOnTime).ToString(CultureInfo.InvariantCulture));
      parameters.Add(Info);
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters) {
      base.ParseParameters(parameters);

      Nick = string.Empty;
      IdleLength = 0;
      SignOnTime = DateTime.Now;
      Info = string.Empty;

      if (parameters.Count > 2) {
        Nick = parameters[1];
        IdleLength = Convert.ToInt32(parameters[2], CultureInfo.InvariantCulture);

        if (parameters.Count == 5) {
          SignOnTime = MessageUtil.ConvertFromUnixTime(Convert.ToInt32(parameters[3], CultureInfo.InvariantCulture));
          Info = parameters[4];
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnWhoIsIdleReply(new IrcMessageEventArgs<WhoIsIdleReplyMessage>(this));
    }
  }
}
