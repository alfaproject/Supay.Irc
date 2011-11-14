using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc
{
  /// <summary>
  ///   Represents a status window for communication between the user and the server
  /// </summary>
  public class ServerQuery : INotifyPropertyChanged
  {
    #region CTor

    /// <summary>
    ///   Creates a new instance of the <see cref="Query" /> class on the given client with the given User.
    /// </summary>
    public ServerQuery(Client client)
    {
      this.client = client;
      this.journal.CollectionChanged += this.JournalCollectionChanged;
    }

    #endregion

    #region Properties

    private readonly Client client;
    private readonly Journal journal = new Journal();

    /// <summary>
    ///   Gets the journal of messages on the query
    /// </summary>
    public virtual Journal Journal
    {
      get
      {
        return this.journal;
      }
    }

    /// <summary>
    ///   Gets the client which the status is on.
    /// </summary>
    public virtual Client Client
    {
      get
      {
        return this.client;
      }
    }

    #endregion

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
