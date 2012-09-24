using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc
{
    /// <summary>
    ///   Represents a status window for communication between the user and the server
    /// </summary>
    public class ServerQuery : INotifyPropertyChanged
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="Query" /> class on the given client with the given User.
        /// </summary>
        public ServerQuery(Client client)
        {
            Client = client;
            Journal = new Journal();
            Journal.CollectionChanged += JournalCollectionChanged;
        }

        /// <summary>
        ///   Gets the journal of messages on the query
        /// </summary>
        public Journal Journal
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets the client which the status is on.
        /// </summary>
        public Client Client
        {
            get;
            private set;
        }


        #region Event Handlers

        private void JournalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs("Journal"));
        }

        #endregion


        #region INotifyPropertyChanged Members

        /// <summary>
        ///   Raised when a property on the instance has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}
