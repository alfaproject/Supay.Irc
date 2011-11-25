using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc
{
  /// <summary>
  ///   Represents a query window for private chat with one User
  /// </summary>
  public class Query : INotifyPropertyChanged
  {
    #region CTor

    /// <summary>
    ///   Creates a new instance of the <see cref="Query" /> class on the given client with the given User.
    /// </summary>
    public Query(Client client, User user)
    {
      this.client = client;
      this.journal.CollectionChanged += this.JournalCollectionChanged;
      this.User = user;
    }

    #endregion

    #region Properties

    private readonly Client client;
    private readonly Journal journal = new Journal();
    private User user;

    /// <summary>
    ///   Gets the User in the private chat.
    /// </summary>
    public User User
    {
      get
      {
        return this.user;
      }
      private set
      {
        this.user = value;
        this.NotifyPropertyChanged("User");
      }
    }

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
    ///   Gets the client which the query is on.
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
    ///   Raised when a property value has changed.
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

    private void NotifyPropertyChanged(string propertyName)
    {
      this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
  }
}
