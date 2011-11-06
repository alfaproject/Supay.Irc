using System.Collections.Specialized;
using System.ComponentModel;

namespace Supay.Irc {
  /// <summary>
  ///   Represents a query window for private chat with one User
  /// </summary>
  public class Query : INotifyPropertyChanged {
    #region CTor

    /// <summary>
    ///   Creates a new instance of the <see cref="Query" /> class on the given client with the given User.
    /// </summary>
    public Query(Client client, User user) {
      this.client = client;
      journal.CollectionChanged += journal_CollectionChanged;
      User = user;
    }

    #endregion

    #region Properties

    private readonly Client client;
    private readonly Journal journal = new Journal();
    private User user;

    /// <summary>
    ///   Gets the User in the private chat.
    /// </summary>
    public User User {
      get {
        return user;
      }
      private set {
        user = value;
        NotifyPropertyChanged("User");
      }
    }

    /// <summary>
    ///   Gets the journal of messages on the query
    /// </summary>
    public virtual Journal Journal {
      get {
        return journal;
      }
    }

    /// <summary>
    ///   Gets the client which the query is on.
    /// </summary>
    public virtual Client Client {
      get {
        return client;
      }
    }

    #endregion

    #region Event Handlers

    private void journal_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      OnPropertyChanged(new PropertyChangedEventArgs("Journal"));
    }

    #endregion

    #region INotifyPropertyChanged Members

    /// <summary>
    ///   Raised when a property value has changed.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    private void OnPropertyChanged(PropertyChangedEventArgs e) {
      if (PropertyChanged != null) {
        PropertyChanged(this, e);
      }
    }

    private void NotifyPropertyChanged(string propertyName) {
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
  }
}
