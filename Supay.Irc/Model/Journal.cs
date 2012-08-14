using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc
{
    /// <summary>
    ///   The journal of messages and related information related to an irc channel or query.
    /// </summary>
    [Serializable]
    public class Journal : ObservableCollection<JournalEntry>
    {
        private int maxEntries = 1000;

        /// <summary>
        ///   Creates a new instance of the Journal class.
        /// </summary>
        public Journal()
        {
        }

        /// <summary>
        ///   Creates a new instance of the Journal class starting with the given entry list.
        /// </summary>
        public Journal(IEnumerable<JournalEntry> list)
            : base(list)
        {
        }

        /// <summary>
        ///   The maximum number of entries kept in the journal at once.
        /// </summary>
        public int MaxEntries
        {
            get
            {
                return this.maxEntries;
            }
            set
            {
                this.maxEntries = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("MaxEntries"));
            }
        }

        /// <summary>
        ///   Inserts the given entry into the collection at the given index.
        /// </summary>
        protected override void InsertItem(int index, JournalEntry item)
        {
            this.CheckReentrancy();
            this.Items.Insert(index, item);
            if (this.Items.Count > this.MaxEntries)
            {
                this.Items.RemoveAt(index != 0 ? 0 : this.Items.Count - 1);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }
    }
}
