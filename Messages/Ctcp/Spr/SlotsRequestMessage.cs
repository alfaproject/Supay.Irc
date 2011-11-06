using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   An SPR Jukebox message that notifies the recipient of the senders available mp3 serving capabilities.
  /// </summary>
  [Serializable]
  public class SlotsRequestMessage : CtcpRequestMessage {
    /// <summary>
    ///   Creates a new instance of the <see cref="SlotsRequestMessage" /> class.
    /// </summary>
    public SlotsRequestMessage()
      : base() {
      InternalCommand = "SLOTS";
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class with the given text and target.
    /// </summary>
    /// <param name="target">The target of the action.</param>
    public SlotsRequestMessage(string target)
      : this() {
      Target = target;
    }

    /// <summary>
    ///   TotalSendSlots
    /// </summary>
    public int TotalSendSlots {
      get {
        return totalSendSlots;
      }
      set {
        totalSendSlots = value;
      }
    }

    private int totalSendSlots;

    /// <summary>
    ///   AvailableSendSlots
    /// </summary>
    public int AvailableSendSlots {
      get {
        return availableSendSlots;
      }
      set {
        availableSendSlots = value;
      }
    }

    private int availableSendSlots;

    /// <summary>
    ///   NextSend
    /// </summary>
    public string NextSend {
      get {
        return nextSend;
      }
      set {
        nextSend = value;
      }
    }

    private string nextSend;

    /// <summary>
    ///   TakenQueueSlots
    /// </summary>
    public int TakenQueueSlots {
      get {
        return takenQueueSlots;
      }
      set {
        takenQueueSlots = value;
      }
    }

    private int takenQueueSlots;

    /// <summary>
    ///   TotalQueueSlots
    /// </summary>
    public int TotalQueueSlots {
      get {
        return totalQueueSlots;
      }
      set {
        totalQueueSlots = value;
      }
    }

    private int totalQueueSlots;

    /// <summary>
    ///   CpsRecord
    /// </summary>
    public int CpsRecord {
      get {
        return cpsRecord;
      }
      set {
        cpsRecord = value;
      }
    }

    private int cpsRecord;

    /// <summary>
    ///   TotalFiles
    /// </summary>
    public int TotalFiles {
      get {
        return totalFiles;
      }
      set {
        totalFiles = value;
      }
    }

    private int totalFiles;

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit) {
      conduit.OnSlotsRequest(new IrcMessageEventArgs<SlotsRequestMessage>(this));
    }

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData {
      get {
        return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6}", TotalSendSlots, AvailableSendSlots, NextSend, TakenQueueSlots, TotalQueueSlots, CpsRecord, TotalFiles);
      }
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.int.TryParse(System.string,System.Globalization.NumberStyles,System.IFormatProvider,System.int@)")]
    public override void Parse(string unparsedMessage) {
      base.Parse(unparsedMessage);
      string slotInfo = CtcpUtil.GetExtendedData(unparsedMessage);
      string[] slotInfoItems = slotInfo.Split(' ');
      if (slotInfoItems.Length >= 7) {
        int.TryParse(slotInfoItems[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out totalSendSlots);
        int.TryParse(slotInfoItems[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out availableSendSlots);
        nextSend = slotInfoItems[2];
        int.TryParse(slotInfoItems[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out takenQueueSlots);
        int.TryParse(slotInfoItems[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out totalQueueSlots);
        int.TryParse(slotInfoItems[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out cpsRecord);
        int.TryParse(slotInfoItems[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out totalFiles);
      }
    }
  }
}
