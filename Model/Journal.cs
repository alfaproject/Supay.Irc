﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc {
  /// <summary>
  ///   The journal of messages and related information related to an irc channel or query.
  /// </summary>
  [Serializable]
  public class Journal : ObservableCollection<JournalEntry> {
    /// <summary>
    ///   Creates a new instance of the Journal class.
    /// </summary>
    public Journal()
      : base() {
    }

    /// <summary>
    ///   Creates a new instance of the Journal class starting with the given entry list.
    /// </summary>
    public Journal(IEnumerable<JournalEntry> list)
      : base(list) {
    }

    /// <summary>
    ///   Inserts the given entry into the collection at the given index.
    /// </summary>
    protected override void InsertItem(int index, JournalEntry item) {
      CheckReentrancy();
      Items.Insert(index, item);
      if (Items.Count > MaxEntries) {
        Items.RemoveAt(index != 0 ? 0 : Items.Count - 1);
      }
      base.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
      base.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
      base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
    }

    /// <summary>
    ///   The maximum number of entries kept in the journal at once.
    /// </summary>
    public int MaxEntries {
      get {
        return _maxEntries;
      }
      set {
        _maxEntries = value;
        base.OnPropertyChanged(new PropertyChangedEventArgs("MaxEntries"));
      }
    }

    private int _maxEntries = 1000;
  }
}
