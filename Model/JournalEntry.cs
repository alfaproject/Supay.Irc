using System;
using System.ComponentModel;
using Supay.Irc.Messages;

namespace Supay.Irc
{
  /// <summary>
  ///   A single entry in the journal of messages and related information related to an IRC channel or query.
  /// </summary>
  public class JournalEntry : INotifyPropertyChanged
  {
    /// <summary>
    ///   Creates a new instance of the <see href = "JournalEntry" /> class.
    /// </summary>
    public JournalEntry()
    {
    }

    /// <summary>
    ///   Creates a new instance of the <see href = "JournalEntry" /> class, populated with the given item.
    /// </summary>
    public JournalEntry(object item)
    {
      this.Item = item;
    }

    #region Properties

    private object _item;
    private DateTime _time;

    /// <summary>
    ///   The time at which the entry was added to the journal.
    /// </summary>
    public DateTime Time
    {
      get
      {
        return this._time;
      }
      set
      {
        this._time = value;
        this.NotifyPropertyChanged("Time");
      }
    }

    /// <summary>
    ///   The entry data, usually an <see cref="IrcMessage" />, but can be any object.
    /// </summary>
    public object Item
    {
      get
      {
        return this._item;
      }
      set
      {
        this._item = value;
        this.NotifyPropertyChanged("Item");
      }
    }

    #endregion

    #region INotifyPropertyChanged

    /// <summary>
    ///   Raised when a property on the instance has changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion
  }
}
