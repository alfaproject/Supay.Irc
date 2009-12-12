using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  /// The <see cref="ListMessage"/> is used to list channels and their topics.
  /// </summary>
  /// <remarks>
  /// A server sent a <see cref="ListMessage"/> will reply with a 
  /// <see cref="ListStartReplyMessage"/>, <see cref="ListReplyMessage"/>, and a <see cref="ListEndReplyMessage"/>.
  /// </remarks>
  [Serializable]
  public class ListMessage : CommandMessage {

    /// <summary>
    /// Creates a new instance of the <see cref="ListMessage"/> class.
    /// </summary>
    public ListMessage() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ListMessage"/> class with the given channel.
    /// </summary>
    /// <param name="channel"></param>
    public ListMessage(String channel) {
      this.channels.Add(channel);
    }

    /// <summary>
    /// Gets the channels to get info about.
    /// </summary>
    /// <remarks>
    /// If this is empty, information about all channels is requested.
    /// </remarks>
    public virtual StringCollection Channels {
      get {
        return this.channels;
      }
    }
    private StringCollection channels = new StringCollection();

    /// <summary>
    /// Gets or sets the server that should return the info.
    /// </summary>
    public virtual String Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }
    private String server = "";

    /// <summary>
    /// Gets or sets the maximum number of users that channels can have to be returned.
    /// </summary>
    public int MaxUsers {
      get {
        return maxUsers;
      }
      set {
        maxUsers = value;
      }
    }
    private int maxUsers = -1;

    /// <summary>
    /// Gets or sets the minimum number of users that channels can have to be returned.
    /// </summary>
    public int MinUsers {
      get {
        return minUsers;
      }
      set {
        minUsers = value;
      }
    }
    private int minUsers = -1;

    /// <summary>
    /// Gets or sets, in minutes, the longest amount of time 
    /// which may have passed since a channel was created in order to be returned.
    /// </summary>
    public int YoungerThan {
      get {
        return youngerThan;
      }
      set {
        youngerThan = value;
      }
    }
    private int youngerThan = -1;

    /// <summary>
    /// Gets or sets, in minutes, the shortest amount of time 
    /// which may have passed since a channel was created in order to be returned.
    /// </summary>
    public int OlderThan {
      get {
        return olderThan;
      }
      set {
        olderThan = value;
      }
    }
    private int olderThan = -1;

    /// <summary>
    /// Gets or sets the a mask which a channel must match to be returned.
    /// </summary>
    public String MatchMask {
      get {
        return matchMask;
      }
      set {
        matchMask = value;
      }
    }
    private String matchMask = "";

    /// <summary>
    /// Gets or sets a mask which a channel cannot match to be returned.
    /// </summary>
    public String NotMatchMask {
      get {
        return notMatchMask;
      }
      set {
        notMatchMask = value;
      }
    }
    private String notMatchMask = "";

    /// <summary>
    /// Gets or sets, in minutes, the shortest amount of time 
    /// which may have passed since a channel's topic was changed, to be returned.
    /// </summary>
    /// <remarks>
    /// Setting this property to "5" will cause only channels to be returned where their
    /// topic hasn't been changed in the last 5 minutes.
    /// </remarks>
    public int TopicOlderThan {
      get {
        return topicOlderThan;
      }
      set {
        topicOlderThan = value;
      }
    }
    private int topicOlderThan = -1;

    /// <summary>
    /// Gets or sets, in minutes, the longest amount of time 
    /// which may have passed since a channel's topic was changed, to be returned.
    /// </summary>
    /// <remarks>
    /// Setting this property to "5" will cause only channels to be returned where their
    /// topic has been changed in the last 5 minutes.
    /// </remarks>
    public int TopicYoungerThan {
      get {
        return topicYoungerThan;
      }
      set {
        topicYoungerThan = value;
      }
    }
    private int topicYoungerThan = -1;

    /// <summary>
    /// Gets the Irc command associated with this message.
    /// </summary>
    protected override String Command {
      get {
        return "LIST";
      }
    }

    /// <exclude />
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport != null) {
        if (this.MaxUsers >= 0 || this.MinUsers >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.UserCount);
        }
        if (this.YoungerThan >= 0 || this.OlderThan >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.CreationTime);
        }
        if (!String.IsNullOrEmpty(this.MatchMask)) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Mask);
        }
        if (!String.IsNullOrEmpty(this.NotMatchMask)) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.NotMask);
        }
        if (TopicOlderThan >= 0 || TopicYoungerThan >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Topic);
        }
      }
    }

    private static void VerifySupport(ServerSupport serverSupport, ServerSupport.ExtendedListParameters parameter) {
      if ((serverSupport.ExtendedList & parameter) != parameter) {
        throw new InvalidMessageException(String.Format(CultureInfo.InvariantCulture, Properties.Resources.ServerDoesNotSupportExtendedListParameter, parameter.ToString()));
      }
    }

    /// <summary>
    /// Overrides <see cref="IrcMessage.AddParametersToFormat"/>.
    /// </summary>
    protected override void AddParametersToFormat(IrcMessageWriter writer) {
      base.AddParametersToFormat(writer);

      StringCollection options = new StringCollection();
      if (this.MaxUsers >= 0) {
        options.Add("<" + this.MaxUsers.ToString(CultureInfo.InvariantCulture));
      }
      if (this.MinUsers >= 0) {
        options.Add(">" + this.MinUsers.ToString(CultureInfo.InvariantCulture));
      }
      if (this.YoungerThan >= 0) {
        options.Add("C<" + this.YoungerThan.ToString(CultureInfo.InvariantCulture));
      }
      if (this.OlderThan >= 0) {
        options.Add("C>" + this.OlderThan.ToString(CultureInfo.InvariantCulture));
      }
      if (!string.IsNullOrEmpty(MatchMask)) {
        options.Add("*" + this.MatchMask + "*");
      }
      if (!String.IsNullOrEmpty(NotMatchMask)) {
        options.Add("!*" + this.NotMatchMask + "*");
      }
      if (this.TopicOlderThan >= 0) {
        options.Add("T>" + this.TopicOlderThan.ToString(CultureInfo.InvariantCulture));
      }
      if (this.TopicYoungerThan >= 0) {
        options.Add("T<" + this.TopicYoungerThan.ToString(CultureInfo.InvariantCulture));
      }


      if (options.Count > 0) {
        writer.AddList(options, ",", false);
      } else if (this.Channels.Count != 0) {
        writer.AddList(this.Channels, ",", false);
        if (this.Server.Length != 0) {
          writer.AddParameter(this.Server);
        }
      }
    }

    /// <summary>
    /// Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(StringCollection parameters) {
      base.ParseParameters(parameters);

      this.Channels.Clear();
      this.Server = "";
      this.MatchMask = "";
      this.NotMatchMask = "";
      this.MaxUsers = -1;
      this.MinUsers = -1;
      this.OlderThan = -1;
      this.YoungerThan = -1;
      this.TopicOlderThan = -1;
      this.TopicYoungerThan = -1;

      if (parameters.Count >= 1) {

        if (IsExtendedParameter(parameters[0])) {
          foreach (String extOption in parameters[0].Split(',')) {
            if (extOption.StartsWith("!*", StringComparison.Ordinal)) {
              this.NotMatchMask = MessageUtil.StringBetweenStrings(extOption, "!*", "*");
            } else if (extOption.StartsWith("*", StringComparison.Ordinal)) {
              this.MatchMask = MessageUtil.StringBetweenStrings(extOption, "*", "*");
            } else if (extOption.StartsWith("C>", StringComparison.Ordinal)) {
              this.OlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("C<", StringComparison.Ordinal)) {
              this.YoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("T>", StringComparison.Ordinal)) {
              this.TopicOlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("T<", StringComparison.Ordinal)) {
              this.TopicYoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith(">", StringComparison.Ordinal)) {
              this.MinUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("<", StringComparison.Ordinal)) {
              this.MaxUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
            }
          }
        } else if (MessageUtil.HasValidChannelPrefix(parameters[0])) {
          this.Channels.AddRange(parameters[0].Split(','));
          if (parameters.Count >= 2) {
            this.Server = parameters[1];
          }
        }

      }
    }

    private static bool IsExtendedParameter(String p) {
      if (String.IsNullOrEmpty(p)) {
        return false;
      }
      String[] exList = new String[] { "!*", "*", "<", ">", "T", "C" };
      foreach (String extStart in exList) {
        if (p.StartsWith(extStart, StringComparison.Ordinal)) {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Notifies the given <see cref="MessageConduit"/> by raising the appropriate event for the current <see cref="IrcMessage"/> subclass.
    /// </summary>
    public override void Notify(Supay.Irc.Messages.MessageConduit conduit) {
      conduit.OnList(new IrcMessageEventArgs<ListMessage>(this));
    }

  }

}