using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Supay.Irc {

  /// <summary>
  /// A single entry in the journal of messages and related information related to an irc channel or query.
  /// </summary>
  [DataContractAttribute]
  public class JournalEntry : INotifyPropertyChanged {

    /// <summary>
    /// Creates a new instance of the <see href="JournalEntry"/> class.
    /// </summary>
    public JournalEntry() {
    }

    /// <summary>
    /// Creates a new instance of the <see href="JournalEntry"/> class, populated with the given item.
    /// </summary>
    public JournalEntry(object item) {
      this.Item = item;
    }

    #region Properties


    /// <summary>
    /// The time at which the entry was added to the journal.
    /// </summary>
    [DataMember]
    public DateTime Time {
      get {
        return _time;
      }
      set {
        _time = value;
        NotifyPropertyChanged("Time");
      }
    }
    private DateTime _time;


    /// <summary>
    /// The entry data, usually an IrcMessage, but can be any object.
    /// </summary>
    [DataMember]
    public object Item {
      get {
        return _item;
      }
      set {
        _item = value;
        NotifyPropertyChanged("Item");
      }
    }
    private object _item;



    #endregion

    #region INotifyPropertyChanged

    /// <summary>
    /// Raised when a property on the instance has changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName) {
      if (this.PropertyChanged != null) {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion

  }

}