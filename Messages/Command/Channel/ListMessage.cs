using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   The <see cref="ListMessage" /> is used to list channels and their topics.
  /// </summary>
  /// <remarks>
  ///   A server sent a <see cref="ListMessage" /> will reply with a 
  ///   <see cref="ListStartReplyMessage" />, <see cref="ListReplyMessage" />, and a <see cref="ListEndReplyMessage" />.
  /// </remarks>
  [Serializable]
  public class ListMessage : CommandMessage {
    private readonly List<string> channels = new List<string>();
    private string matchMask = string.Empty;
    private int maxUsers = -1;
    private int minUsers = -1;
    private string notMatchMask = string.Empty;
    private int olderThan = -1;
    private string server = string.Empty;
    private int topicOlderThan = -1;
    private int topicYoungerThan = -1;
    private int youngerThan = -1;

    /// <summary>
    ///   Creates a new instance of the <see cref="ListMessage" /> class.
    /// </summary>
    public ListMessage() {
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ListMessage" /> class with the given channel.
    /// </summary>
    /// <param name="channel"></param>
    public ListMessage(string channel) {
      channels.Add(channel);
    }

    /// <summary>
    ///   Gets the channels to get info about.
    /// </summary>
    /// <remarks>
    ///   If this is empty, information about all channels is requested.
    /// </remarks>
    public virtual List<string> Channels {
      get {
        return channels;
      }
    }

    /// <summary>
    ///   Gets or sets the server that should return the info.
    /// </summary>
    public virtual string Server {
      get {
        return server;
      }
      set {
        server = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of users that channels can have to be returned.
    /// </summary>
    public int MaxUsers {
      get {
        return maxUsers;
      }
      set {
        maxUsers = value;
      }
    }

    /// <summary>
    ///   Gets or sets the minimum number of users that channels can have to be returned.
    /// </summary>
    public int MinUsers {
      get {
        return minUsers;
      }
      set {
        minUsers = value;
      }
    }

    /// <summary>
    ///   Gets or sets, in minutes, the longest amount of time 
    ///   which may have passed since a channel was created in order to be returned.
    /// </summary>
    public int YoungerThan {
      get {
        return youngerThan;
      }
      set {
        youngerThan = value;
      }
    }

    /// <summary>
    ///   Gets or sets, in minutes, the shortest amount of time 
    ///   which may have passed since a channel was created in order to be returned.
    /// </summary>
    public int OlderThan {
      get {
        return olderThan;
      }
      set {
        olderThan = value;
      }
    }

    /// <summary>
    ///   Gets or sets the a mask which a channel must match to be returned.
    /// </summary>
    public string MatchMask {
      get {
        return matchMask;
      }
      set {
        matchMask = value;
      }
    }

    /// <summary>
    ///   Gets or sets a mask which a channel cannot match to be returned.
    /// </summary>
    public string NotMatchMask {
      get {
        return notMatchMask;
      }
      set {
        notMatchMask = value;
      }
    }

    /// <summary>
    ///   Gets or sets, in minutes, the shortest amount of time 
    ///   which may have passed since a channel's topic was changed, to be returned.
    /// </summary>
    /// <remarks>
    ///   Setting this property to "5" will cause only channels to be returned where their
    ///   topic hasn't been changed in the last 5 minutes.
    /// </remarks>
    public int TopicOlderThan {
      get {
        return topicOlderThan;
      }
      set {
        topicOlderThan = value;
      }
    }

    /// <summary>
    ///   Gets or sets, in minutes, the longest amount of time 
    ///   which may have passed since a channel's topic was changed, to be returned.
    /// </summary>
    /// <remarks>
    ///   Setting this property to "5" will cause only channels to be returned where their
    ///   topic has been changed in the last 5 minutes.
    /// </remarks>
    public int TopicYoungerThan {
      get {
        return topicYoungerThan;
      }
      set {
        topicYoungerThan = value;
      }
    }

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command {
      get {
        return "LIST";
      }
    }

    /// <exclude />
    public override void Validate(ServerSupport serverSupport) {
      base.Validate(serverSupport);
      if (serverSupport != null) {
        if (MaxUsers >= 0 || MinUsers >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.UserCount);
        }
        if (YoungerThan >= 0 || OlderThan >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.CreationTime);
        }
        if (!string.IsNullOrEmpty(MatchMask)) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Mask);
        }
        if (!string.IsNullOrEmpty(NotMatchMask)) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.NotMask);
        }
        if (TopicOlderThan >= 0 || TopicYoungerThan >= 0) {
          VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Topic);
        }
      }
    }

    private static void VerifySupport(ServerSupport serverSupport, ServerSupport.ExtendedListParameters parameter) {
      if ((serverSupport.ExtendedList & parameter) != parameter) {
        throw new InvalidMessageException(string.Format(CultureInfo.InvariantCulture, Resources.ServerDoesNotSupportExtendedListParameter, parameter));
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override Collection<string> GetParameters() {
      var options = new Collection<string>();
      if (MaxUsers >= 0) {
        options.Add("<" + MaxUsers.ToString(CultureInfo.InvariantCulture));
      }
      if (MinUsers >= 0) {
        options.Add(">" + MinUsers.ToString(CultureInfo.InvariantCulture));
      }
      if (YoungerThan >= 0) {
        options.Add("C<" + YoungerThan.ToString(CultureInfo.InvariantCulture));
      }
      if (OlderThan >= 0) {
        options.Add("C>" + OlderThan.ToString(CultureInfo.InvariantCulture));
      }
      if (!string.IsNullOrEmpty(MatchMask)) {
        options.Add("*" + MatchMask + "*");
      }
      if (!string.IsNullOrEmpty(NotMatchMask)) {
        options.Add("!*" + NotMatchMask + "*");
      }
      if (TopicOlderThan >= 0) {
        options.Add("T>" + TopicOlderThan.ToString(CultureInfo.InvariantCulture));
      }
      if (TopicYoungerThan >= 0) {
        options.Add("T<" + TopicYoungerThan.ToString(CultureInfo.InvariantCulture));
      }

      Collection<string> parameters = base.GetParameters();
      if (options.Count != 0) {
        parameters.Add(MessageUtil.CreateList(options, ","));
      } else if (Channels.Count != 0) {
        parameters.Add(MessageUtil.CreateList(Channels, ","));
        if (!string.IsNullOrEmpty(Server)) {
          parameters.Add(Server);
        }
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(Collection<string> parameters) {
      base.ParseParameters(parameters);

      Channels.Clear();
      Server = string.Empty;
      MatchMask = string.Empty;
      NotMatchMask = string.Empty;
      MaxUsers = -1;
      MinUsers = -1;
      OlderThan = -1;
      YoungerThan = -1;
      TopicOlderThan = -1;
      TopicYoungerThan = -1;

      if (parameters.Count >= 1) {
        if (IsExtendedParameter(parameters[0])) {
          foreach (string extOption in parameters[0].Split(',')) {
            if (extOption.StartsWith("!*", StringComparison.Ordinal)) {
              NotMatchMask = MessageUtil.StringBetweenStrings(extOption, "!*", "*");
            } else if (extOption.StartsWith("*", StringComparison.Ordinal)) {
              MatchMask = MessageUtil.StringBetweenStrings(extOption, "*", "*");
            } else if (extOption.StartsWith("C>", StringComparison.Ordinal)) {
              OlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("C<", StringComparison.Ordinal)) {
              YoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("T>", StringComparison.Ordinal)) {
              TopicOlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("T<", StringComparison.Ordinal)) {
              TopicYoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith(">", StringComparison.Ordinal)) {
              MinUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
            } else if (extOption.StartsWith("<", StringComparison.Ordinal)) {
              MaxUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
            }
          }
        } else if (MessageUtil.HasValidChannelPrefix(parameters[0])) {
          Channels.AddRange(parameters[0].Split(','));
          if (parameters.Count >= 2) {
            Server = parameters[1];
          }
        }
      }
    }

    private static bool IsExtendedParameter(string p) {
      if (string.IsNullOrEmpty(p)) {
        return false;
      }
      var exList = new[] { "!*", "*", "<", ">", "T", "C" };
      return exList.Any(extStart => p.StartsWith(extStart, StringComparison.Ordinal));
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnList(new IrcMessageEventArgs<ListMessage>(this));
    }
  }
}
