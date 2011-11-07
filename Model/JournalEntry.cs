using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Supay.Irc.Messages;

namespace Supay.Irc {
  /// <summary>
  ///   A single entry in the journal of messages and related information related to an IRC channel or query.
  /// </summary>
  public class JournalEntry : INotifyPropertyChanged {
    /// <summary>
    ///   Creates a new instance of the <see href = "JournalEntry" /> class.
    /// </summary>
    public JournalEntry() {
    }

    /// <summary>
    ///   Creates a new instance of the <see href = "JournalEntry" /> class, populated with the given item.
    /// </summary>
    public JournalEntry(object item) {
      Item = item;
    }

    #region Properties

    private object _item;
    private DateTime _time;

    /// <summary>
    ///   The time at which the entry was added to the journal.
    /// </summary>
    public DateTime Time {
      get {
        return _time;
      }
      set {
        _time = value;
        NotifyPropertyChanged("Time");
      }
    }

    /// <summary>
    ///   The entry data, usually an <see cref="IrcMessage" />, but can be any object.
    /// </summary>
    public object Item {
      get {
        return _item;
      }
      set {
        _item = value;
        NotifyPropertyChanged("Item");
      }
    }

    #endregion

    #region INotifyPropertyChanged

    /// <summary>
    ///   Raised when a property on the instance has changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
