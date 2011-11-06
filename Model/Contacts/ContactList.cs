using System;

namespace Supay.Irc.Contacts {
  /// <summary>
  ///   A contact list which tracks the online and offline status of the users within the Users
  ///   collection property. </summary>
  /// <remarks>
  ///   The ContactList will use Watch, Monitor, or IsOn, depending on server support. User status
  ///   changes will be updated via the User.OnlineStatus property. </remarks>
  public class ContactList : IDisposable {
    private ContactsTracker _tracker;

    /// <summary>
    ///   Gets the collection of users being tracked as a contact list. </summary>
    public UserCollection Users {
      get;
      private set;
    }

    /// <summary>
    ///   The client on which the list is tracked. </summary>
    public Client Client {
      get;
      private set;
    }

    /// <summary>
    ///   Initializes the <see cref="ContactList"/> on the given client. </summary>
    /// <remarks>
    ///   This method should not be called until the Client receives the
    ///   <see cref="ServerSupport"/> is populated. An easy way to make sure is to wait until the
    ///   Ready event of the Client. </remarks>
    public void Initialize(Client client) {
      if (client == null) {
        throw new ArgumentNullException("client");
      }
      ServerSupport support = client.ServerSupports;
      Client = client;
      Users = new UserCollection();

      if (support.MaxWatches > 0) {
        _tracker = new ContactsWatchTracker(this);
      } else if (support.MaxMonitors > 0) {
        _tracker = new ContactsMonitorTracker(this);
      } else {
        _tracker = new ContactsAreOnTracker(this);
      }

      _tracker.Initialize();
    }

    #region IDisposable

    protected virtual void Dispose(bool disposing) {
      if (disposing && _tracker != null && _tracker is ContactsAreOnTracker) {
        ((IDisposable) _tracker).Dispose();
      }
    }

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting
    ///   unmanaged resources. </summary>
    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
