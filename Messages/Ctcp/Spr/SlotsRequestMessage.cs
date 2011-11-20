using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   An SPR Jukebox message that notifies the recipient of the senders available mp3 serving capabilities.
  /// </summary>
  [Serializable]
  public class SlotsRequestMessage : CtcpRequestMessage
  {
    private int availableSendSlots;
    private int cpsRecord;
    private string nextSend;
    private int takenQueueSlots;
    private int totalFiles;
    private int totalQueueSlots;
    private int totalSendSlots;

    /// <summary>
    ///   Creates a new instance of the <see cref="SlotsRequestMessage" /> class.
    /// </summary>
    public SlotsRequestMessage()
    {
      this.InternalCommand = "SLOTS";
    }

    /// <summary>
    ///   Creates a new instance of the <see cref="ActionRequestMessage" /> class with the given text and target.
    /// </summary>
    /// <param name="target">The target of the action.</param>
    public SlotsRequestMessage(string target)
      : this()
    {
      this.Target = target;
    }

    /// <summary>
    ///   TotalSendSlots
    /// </summary>
    public int TotalSendSlots
    {
      get
      {
        return this.totalSendSlots;
      }
      set
      {
        this.totalSendSlots = value;
      }
    }

    /// <summary>
    ///   AvailableSendSlots
    /// </summary>
    public int AvailableSendSlots
    {
      get
      {
        return this.availableSendSlots;
      }
      set
      {
        this.availableSendSlots = value;
      }
    }

    /// <summary>
    ///   NextSend
    /// </summary>
    public string NextSend
    {
      get
      {
        return this.nextSend;
      }
      set
      {
        this.nextSend = value;
      }
    }

    /// <summary>
    ///   TakenQueueSlots
    /// </summary>
    public int TakenQueueSlots
    {
      get
      {
        return this.takenQueueSlots;
      }
      set
      {
        this.takenQueueSlots = value;
      }
    }

    /// <summary>
    ///   TotalQueueSlots
    /// </summary>
    public int TotalQueueSlots
    {
      get
      {
        return this.totalQueueSlots;
      }
      set
      {
        this.totalQueueSlots = value;
      }
    }

    /// <summary>
    ///   CpsRecord
    /// </summary>
    public int CpsRecord
    {
      get
      {
        return this.cpsRecord;
      }
      set
      {
        this.cpsRecord = value;
      }
    }

    /// <summary>
    ///   TotalFiles
    /// </summary>
    public int TotalFiles
    {
      get
      {
        return this.totalFiles;
      }
      set
      {
        this.totalFiles = value;
      }
    }

    /// <summary>
    ///   Gets the data payload of the Ctcp request.
    /// </summary>
    protected override string ExtendedData
    {
      get
      {
        return string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6}", this.TotalSendSlots, this.AvailableSendSlots, this.NextSend, this.TakenQueueSlots, this.TotalQueueSlots, this.CpsRecord, this.TotalFiles);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnSlotsRequest(new IrcMessageEventArgs<SlotsRequestMessage>(this));
    }

    /// <summary>
    ///   Parses the given string to populate this <see cref="IrcMessage" />.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Int32.TryParse(System.String,System.Globalization.NumberStyles,System.IFormatProvider,System.Int32@)", Justification = "No need to double check TryParse return value.")]
    public override void Parse(string unparsedMessage)
    {
      base.Parse(unparsedMessage);
      string slotInfo = CtcpUtil.GetExtendedData(unparsedMessage);
      var slotInfoItems = slotInfo.Split(' ');
      if (slotInfoItems.Length >= 7)
      {
        int.TryParse(slotInfoItems[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.totalSendSlots);
        int.TryParse(slotInfoItems[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.availableSendSlots);
        this.nextSend = slotInfoItems[2];
        int.TryParse(slotInfoItems[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.takenQueueSlots);
        int.TryParse(slotInfoItems[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.totalQueueSlots);
        int.TryParse(slotInfoItems[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.cpsRecord);
        int.TryParse(slotInfoItems[6], NumberStyles.Integer, CultureInfo.InvariantCulture, out this.totalFiles);
      }
    }
  }
}
