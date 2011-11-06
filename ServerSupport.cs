using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Supay.Irc.Messages;

namespace Supay.Irc {
  /// <summary>
  ///   Contains information about what irc extensions and such the server supports.
  /// </summary>
  /// <remarks>
  ///   This information is sent from a <see cref="Client" /> when it receives a <see cref="Supay.Irc.Messages.SupportMessage" />.
  ///   This most likely makes it unneccesary to catch this message's received event.
  /// </remarks>
  [Serializable]
  public class ServerSupport {
    #region ExtendedListParameters enum

    /// <summary>
    ///   The extended parameters which the server can support on a List message.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    [Flags]
    public enum ExtendedListParameters {
      /// <summary>
      ///   No extended parameters are supported
      /// </summary>
      None = 0,

      /// <summary>
      ///   Searching by matching only the given mask is supported
      /// </summary>
      Mask = 1,

      /// <summary>
      ///   Searching by not matching the given mask is supported
      /// </summary>
      NotMask = 2,

      /// <summary>
      ///   Searching by number of users in the channel is supported
      /// </summary>
      UserCount = 4,

      /// <summary>
      ///   Searching by the channel creation time is supported
      /// </summary>
      CreationTime = 8,

      /// <summary>
      ///   Searching by the most recent change in a channel's topic is supported
      /// </summary>
      Topic = 16
    }

    #endregion

    #region Default Support

    private static ServerSupport _defaultSupport;

    /// <summary>
    /// </summary>
    public static ServerSupport DefaultSupport {
      get {
        if (_defaultSupport == null) {
          _defaultSupport = new ServerSupport();
          //TODO Create A Good Default ServerSupport
        }
        return _defaultSupport;
      }
      set {
        _defaultSupport = value;
      }
    }

    #endregion

    #region Properties

    private readonly Dictionary<string, int> _channelLimits = new Dictionary<string, int>();
    private readonly Collection<string> _channelTypes = new Collection<string>();
    private readonly Dictionary<string, int> _maxMessageTargets = new Dictionary<string, int>();
    private readonly Collection<string> _modesWithParameters = new Collection<string>();
    private readonly Collection<string> _modesWithParametersWhenSet = new Collection<string>();
    private readonly Collection<string> _modesWithoutParameters = new Collection<string>();
    private readonly Dictionary<string, int> _safeChannelPrefixLengths = new Dictionary<string, int>();
    private bool _banExceptions;
    private bool _callerId;
    private string _caseMapping = "rfc1459";
    private int _channelIdLength = -1;
    private bool _channelMessages;
    private bool _channelNotices;
    private string _channelStatuses = "(ov)@+";
    private string _characterSet = string.Empty;
    private ExtendedListParameters _eList;
    private bool _forcedNickChanges;
    private bool _invitationExceptions;
    private bool _knock;
    private int _maxAwayMessageLength = -1;
    private int _maxBans = -1;
    private int _maxChannelNameLength = 200;
    private int _maxChannels = -1;
    private int _maxKickCommentLength = -1;
    private int _maxModes = 3;
    private int _maxMonitors;
    private int _maxNickLength = 9;
    private int _maxSilences;
    private int _maxTopicLength = -1;
    private int _maxWatches = -1;
    private bool _messagesToOperators;
    private string _networkName = string.Empty;
    private bool _penalties;
    private bool _rfc2812;
    private bool _safeList;
    private string _standard = "i-d";
    private bool _userIp;
    private bool _virtualChannels;
    private bool _whoX;
    private bool eTrace;
    private int maxBanExceptions = -1;
    private int maxInvitationsExceptions = -1;
    private string statusMessages = string.Empty;

    /// <summary>
    ///   Gets or sets if the server supports the Deaf user mode
    /// </summary>
    public bool DeafMode {
      get;
      set;
    }

    /// <summary>
    ///   Gets or sets the standard used by the server.
    /// </summary>
    public string Standard {
      get {
        return (_standard);
      }
      set {
        _standard = value;
      }
    }

    /// <summary>
    ///   Gets or sets a list of channel modes a person can get and the respective prefix a channel or nickname will get in case the person has it.
    /// </summary>
    /// <remarks>
    ///   The order of the modes goes from most powerful to least powerful. 
    ///   Those prefixes are shown in the output of the WHOIS, WHO and NAMES command.
    /// </remarks>
    public string ChannelStatuses {
      get {
        return (_channelStatuses);
      }
      set {
        _channelStatuses = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel status prefixes supported for matched-status-only messages
    /// </summary>
    public string StatusMessages {
      get {
        return (statusMessages);
      }
      set {
        statusMessages = value;
      }
    }

    /// <summary>
    ///   Gets the supported channel prefixes.
    /// </summary>
    public Collection<string> ChannelTypes {
      get {
        return (_channelTypes);
      }
    }

    /// <summary>
    ///   Gets the modes that require parameters
    /// </summary>
    public Collection<string> ModesWithParameters {
      get {
        return (_modesWithParameters);
      }
    }

    /// <summary>
    ///   Gets the modes that require parameters only when set.
    /// </summary>
    public Collection<string> ModesWithParametersWhenSet {
      get {
        return (_modesWithParametersWhenSet);
      }
    }

    /// <summary>
    ///   Gets the modes that do not require parameters.
    /// </summary>
    public Collection<string> ModesWithoutParameters {
      get {
        return (_modesWithoutParameters);
      }
    }

    /// <summary>
    ///   Maximum number of channel modes with parameter allowed per <see cref="Supay.Irc.Messages.ChannelModeMessage" /> command.
    /// </summary>
    public int MaxModes {
      get {
        return (_maxModes);
      }
      set {
        _maxModes = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of channels a client can join.
    /// </summary>
    /// <remarks>
    ///   This property is considered obsolete, as most servers use the ChannelLimits property instead.
    /// </remarks>
    public int MaxChannels {
      get {
        return (_maxChannels);
      }
      set {
        _maxChannels = value;
      }
    }

    /// <summary>
    ///   Gets the collection of channel limits, grouped by channel type (ex, #, +).
    /// </summary>
    /// <remarks>
    ///   This property has replaced MaxChannels becuase of the added flexibility.
    /// </remarks>
    public Dictionary<string, int> ChannelLimits {
      get {
        return _channelLimits;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum nickname length.
    /// </summary>
    public int MaxNickLength {
      get {
        return (_maxNickLength);
      }
      set {
        _maxNickLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum channel topic length.
    /// </summary>
    public int MaxTopicLength {
      get {
        return (_maxTopicLength);
      }
      set {
        _maxTopicLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum length of the reason in a <see cref="Supay.Irc.Messages.KickMessage" />.
    /// </summary>
    public int MaxKickCommentLength {
      get {
        return (_maxKickCommentLength);
      }
      set {
        _maxKickCommentLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum length of a channel name.
    /// </summary>
    public int MaxChannelNameLength {
      get {
        return (_maxChannelNameLength);
      }
      set {
        _maxChannelNameLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of bans that a channel can have.
    /// </summary>
    public int MaxBans {
      get {
        return (_maxBans);
      }
      set {
        _maxBans = value;
      }
    }

    /// <summary>
    ///   Gets or sets the Maximum number of invitation exceptions a channel can have.
    /// </summary>
    public int MaxInvitationExceptions {
      get {
        return (maxInvitationsExceptions);
      }
      set {
        maxInvitationsExceptions = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of ban exceptions that a channel can have.
    /// </summary>
    public int MaxBanExceptions {
      get {
        return (maxBanExceptions);
      }
      set {
        maxBanExceptions = value;
      }
    }

    /// <summary>
    ///   Gets or sets the name of the network which the server is on.
    /// </summary>
    public string NetworkName {
      get {
        return (_networkName);
      }
      set {
        _networkName = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports channel ban exceptions.
    /// </summary>
    public bool BanExceptions {
      get {
        return (_banExceptions);
      }
      set {
        _banExceptions = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports channel invitation exceptions.
    /// </summary>
    public bool InvitationExceptions {
      get {
        return (_invitationExceptions);
      }
      set {
        _invitationExceptions = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of silence ( serverside ignore ) listings a client can store.
    /// </summary>
    public int MaxSilences {
      get {
        return (_maxSilences);
      }
      set {
        _maxSilences = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports messages to channel operators.
    /// </summary>
    /// <remarks>
    ///   To send a message to channel operators, use a <see cref="Supay.Irc.Messages.NoticeMessage" />
    ///   with a target in the format "@#channel".
    /// </remarks>
    public bool MessagesToOperators {
      get {
        return (_messagesToOperators);
      }
      set {
        _messagesToOperators = value;
      }
    }

    /// <summary>
    ///   Gets or sets the case mapping supported by the server.
    /// </summary>
    /// <remarks>
    ///   "ascii", "rfc1459", and "strict-rfc1459" are the only known values.
    /// </remarks>
    public string CaseMapping {
      get {
        return (_caseMapping);
      }
      set {
        _caseMapping = value;
      }
    }

    /// <summary>
    ///   Gets or sets the text encoding used by the server.
    /// </summary>
    public string CharacterSet {
      get {
        return (_characterSet);
      }
      set {
        _characterSet = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports the standards declared in rfc 2812.
    /// </summary>
    public bool Rfc2812 {
      get {
        return (_rfc2812);
      }
      set {
        _rfc2812 = value;
      }
    }

    /// <summary>
    ///   Gets or sets the length of channel ids.
    /// </summary>
    public int ChannelIdLength {
      get {
        return (_channelIdLength);
      }
      set {
        _channelIdLength = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server has a message penalty.
    /// </summary>
    public bool Penalties {
      get {
        return (_penalties);
      }
      set {
        _penalties = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server will change your nick automatticly when it needs to.
    /// </summary>
    public bool ForcedNickChanges {
      get {
        return (_forcedNickChanges);
      }
      set {
        _forcedNickChanges = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports the USERIP command.
    /// </summary>
    public bool UserIP {
      get {
        return (_userIp);
      }
      set {
        _userIp = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports the CPRIVMSG command.
    /// </summary>
    public bool ChannelMessages {
      get {
        return (_channelMessages);
      }
      set {
        _channelMessages = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports the CNOTICE command.
    /// </summary>
    public bool ChannelNotices {
      get {
        return (_channelNotices);
      }
      set {
        _channelNotices = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of targets allowed on targetted messages, grouped by message command
    /// </summary>
    public Dictionary<string, int> MaxMessageTargets {
      get {
        return _maxMessageTargets;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports the <see cref="Supay.Irc.Messages.KnockMessage" />.
    /// </summary>
    public bool Knock {
      get {
        return (_knock);
      }
      set {
        _knock = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports virtual channels.
    /// </summary>
    public bool VirtualChannels {
      get {
        return (_virtualChannels);
      }
      set {
        _virtualChannels = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the <see cref="Supay.Irc.Messages.ListReplyMessage" /> is sent in multiple itterations.
    /// </summary>
    public bool SafeList {
      get {
        return (_safeList);
      }
      set {
        _safeList = value;
      }
    }

    /// <summary>
    ///   Gets or sets the extended parameters the server supports for a <see cref="T:Supay.Irc.Messages.ListMessage" />.
    /// </summary>
    public ExtendedListParameters ExtendedList {
      get {
        return _eList;
      }
      set {
        _eList = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of watches a user is allowed to set.
    /// </summary>
    public int MaxWatches {
      get {
        return (_maxWatches);
      }
      set {
        _maxWatches = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the <see cref="Supay.Irc.Messages.WhoMessage" /> uses the WHOX protocol
    /// </summary>
    public bool WhoX {
      get {
        return (_whoX);
      }
      set {
        _whoX = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server suports callerid-style ignore.
    /// </summary>
    public bool CallerId {
      get {
        return (_callerId);
      }
      set {
        _callerId = value;
      }
    }

    /// <summary>
    ///   Gets or sets if the server supports ETrace.
    /// </summary>
    public bool ETrace {
      get {
        return (eTrace);
      }
      set {
        eTrace = value;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum number of user monitors a user is allowed to set.
    /// </summary>
    /// <remarks>
    ///   A value of 0 indicates that the server doesn't support the monitor system.
    ///   A value of -1 indicates that the server has no limit to the users on the monitor system list.
    ///   A value greater than 0 indicates the maximum number of users which can be added to the monitor system list.
    /// </remarks>
    public int MaxMonitors {
      get {
        return _maxMonitors;
      }
      set {
        _maxMonitors = value;
      }
    }

    /// <summary>
    ///   Gets the collection of safe channel prefix lengths, grouped by the channel type they apply to.
    /// </summary>
    public Dictionary<string, int> SafeChannelPrefixLengths {
      get {
        return _safeChannelPrefixLengths;
      }
    }

    /// <summary>
    ///   Gets or sets the maximum length of away messages.
    /// </summary>
    public int MaxAwayMessageLength {
      get {
        return _maxAwayMessageLength;
      }
      set {
        _maxAwayMessageLength = value;
      }
    }

    #endregion

    private readonly NameValueCollection unknownItems = new NameValueCollection();

    /// <summary>
    ///   Gets the list of unknown items supported by the server.
    /// </summary>
    public virtual NameValueCollection UnknownItems {
      get {
        return unknownItems;
      }
    }

    /// <summary>
    ///   Loads support information from the given <see cref="Supay.Irc.Messages.SupportMessage" />.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
    public void LoadInfo(SupportMessage msg) {
      NameValueCollection items = msg.SupportedItems;
      foreach (string key in items.Keys) {
        string value = items[key] ?? "";
        switch (key) {
          case "DEAF":
            DeafMode = true;
            break;
          case "AWAYLEN":
            SetIfNumeric(GetType().GetProperty("MaxAwayMessageLength"), value);
            break;
          case "IDCHAN":
            foreach (InfoPair pair in CreateInfoPairs(value)) {
              int prefixLength;
              if (int.TryParse(pair.Value, out prefixLength)) {
                SafeChannelPrefixLengths.Add(pair.Key, prefixLength);
              }
            }
            break;
          case "STD":
            Standard = value;
            break;
          case "PREFIX":
            ChannelStatuses = value;
            break;
          case "STATUSMSG":
          case "WALLVOICES":
            StatusMessages = value;
            break;
          case "CHANTYPES":
            AddChars(ChannelTypes, value);
            break;
          case "CHANMODES":
            string[] modeGroups = value.Split(',');
            if (modeGroups.Length >= 4) {
              AddChars(ModesWithParameters, modeGroups[0]);
              AddChars(ModesWithParameters, modeGroups[1]);
              AddChars(ModesWithParametersWhenSet, modeGroups[2]);
              AddChars(ModesWithoutParameters, modeGroups[3]);
            } else {
              Trace.WriteLine("Unknown CHANMODES " + value);
            }
            break;
          case "MODES":
            SetIfNumeric(GetType().GetProperty("MaxModes"), value);
            break;
          case "MAXCHANNELS":
            SetIfNumeric(GetType().GetProperty("MaxChannels"), value);
            break;
          case "CHANLIMIT":
            foreach (InfoPair chanLimitInfo in CreateInfoPairs(value)) {
              int limit;
              if (int.TryParse(chanLimitInfo.Value, out limit)) {
                foreach (Char c in chanLimitInfo.Key) {
                  ChannelLimits.Add(c.ToString(), limit);
                }
              }
            }
            break;
          case "NICKLEN":
          case "MAXNICKLEN":
            SetIfNumeric(GetType().GetProperty("MaxNickLength"), value);
            break;
          case "TOPICLEN":
            SetIfNumeric(GetType().GetProperty("MaxTopicLength"), value);
            break;
          case "KICKLEN":
            SetIfNumeric(GetType().GetProperty("MaxKickCommentLength"), value);
            break;
          case "CHANNELLEN":
          case "MAXCHANNELLEN":
            SetIfNumeric(GetType().GetProperty("MaxChannelNameLength"), value);
            break;
          case "MAXBANS":
            SetIfNumeric(GetType().GetProperty("MaxBans"), value);
            break;
          case "NETWORK":
            NetworkName = value;
            break;
          case "EXCEPTS":
            BanExceptions = true;
            break;
          case "INVEX":
            InvitationExceptions = true;
            break;
          case "SILENCE":
            SetIfNumeric(GetType().GetProperty("MaxSilences"), value);
            break;
          case "WALLCHOPS":
            MessagesToOperators = true;
            break;
          case "CASEMAPPING":
            CaseMapping = value;
            break;
          case "CHARSET":
            CharacterSet = value;
            break;
          case "RFC2812":
            Rfc2812 = true;
            break;
          case "CHIDLEN":
            SetIfNumeric(GetType().GetProperty("ChannelIdLength"), value);
            break;
          case "PENALTY":
            Penalties = true;
            break;
          case "FNC":
            ForcedNickChanges = true;
            break;
          case "USERIP":
            UserIP = true;
            break;
          case "CPRIVMSG":
            ChannelMessages = true;
            break;
          case "CNOTICE":
            ChannelNotices = true;
            break;
          case "MAXTARGETS":
            int maxTargets;
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxTargets)) {
              MaxMessageTargets.Add("", maxTargets);
            }
            break;
          case "TARGMAX":
            foreach (InfoPair targmaxInfo in CreateInfoPairs(value)) {
              int targmax;
              if (int.TryParse(targmaxInfo.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out targmax)) {
                MaxMessageTargets.Add(targmaxInfo.Key, targmax);
              } else {
                MaxMessageTargets.Add(targmaxInfo.Key, -1);
              }
            }
            break;
          case "KNOCK":
            Knock = true;
            break;
          case "VCHANS":
            VirtualChannels = true;
            break;
          case "SAFELIST":
            SafeList = true;
            break;
          case "ELIST":
            var elistMap = new Dictionary<char, ExtendedListParameters> {
              { 'M', ExtendedListParameters.Mask },
              { 'N', ExtendedListParameters.NotMask },
              { 'U', ExtendedListParameters.UserCount },
              { 'C', ExtendedListParameters.CreationTime },
              { 'T', ExtendedListParameters.Topic }
            };

            ExtendedList = ExtendedListParameters.None;
            foreach (Char c in value.ToUpperInvariant()) {
              ExtendedList = (ExtendedList | elistMap[c]);
            }

            break;
          case "WATCH":
            SetIfNumeric(GetType().GetProperty("MaxWatches"), value);
            break;
          case "MONITOR":
            MaxMonitors = -1;
            SetIfNumeric(GetType().GetProperty("MaxMonitors"), value);
            break;
          case "WHOX":
            WhoX = true;
            break;
          case "CALLERID":
          case "ACCEPT":
            CallerId = true;
            break;
          case "ETRACE":
            ETrace = true;
            break;
          case "MAXLIST":
            foreach (InfoPair maxListInfoPair in CreateInfoPairs(value)) {
              int maxLength;
              if (int.TryParse(maxListInfoPair.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out maxLength)) {
                if (maxListInfoPair.Key.IndexOf("b", StringComparison.Ordinal) != -1) {
                  MaxBans = maxLength;
                }
                if (maxListInfoPair.Key.IndexOf("e", StringComparison.Ordinal) != -1) {
                  MaxBanExceptions = maxLength;
                }
                if (maxListInfoPair.Key.IndexOf("I", StringComparison.Ordinal) != -1) {
                  MaxInvitationExceptions = maxLength;
                }
              }
            }
            break;
          default:
            UnknownItems[key] = value;
            Trace.WriteLine("Unknown ServerSupport key/value " + key + " " + value);
            break;
        }
      }
    }

    private static void AddChars(ICollection<string> target, IEnumerable<char> source) {
      foreach (Char c in source) {
        target.Add(c.ToString());
      }
    }

    private void SetIfNumeric(PropertyInfo property, string value) {
      int intValue;
      if (value != null && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out intValue)) {
        property.SetValue(this, intValue, null);
      } else {
        Trace.WriteLine("Expected numeric for ServerSupport target " + property.Name + " but it was '" + (value ?? "") + "'");
      }
    }

    private IEnumerable<InfoPair> CreateInfoPairs(string value) {
      var list = new Collection<InfoPair>();
      foreach (string chanLimitPair in value.Split(',')) {
        if (chanLimitPair.Contains(":")) {
          string[] chanLimitInfo = chanLimitPair.Split(':');
          if (chanLimitInfo.Length == 2 && chanLimitInfo[0].Length > 0) {
            var pair = new InfoPair(chanLimitInfo[0], chanLimitInfo[1]);
            list.Add(pair);
          }
        }
      }
      return list;
    }

    #region Nested type: InfoPair

    private struct InfoPair {
      public readonly string Key;
      public readonly string Value;

      public InfoPair(string key, string value) {
        Key = key;
        Value = value;
      }
    }

    #endregion
  }
}
