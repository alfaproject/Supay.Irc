using System;

namespace Supay.Irc.Contacts
{
  /// <summary>
  ///   A contact list which tracks the online and offline status of the users within the Users
  ///   collection property.
  /// </summary>
  /// <remarks>
  ///   The ContactList will use Watch, Monitor, or IsOn, depending on server support. User status
  ///   changes will be updated via the User.OnlineStatus property.
  /// </remarks>
  public class ContactList : IDisposable
  {
    private ContactsTracker tracker;

    /// <summary>
    ///   Gets the collection of users being tracked as a contact list.
    /// </summary>
    public UserCollection Users
    {
      get;
      private set;
    }

    /// <summary>
    ///   The client on which the list is tracked.
    /// </summary>
    public Client Client
    {
      get;
      private set;
    }

    /// <summary>
    ///   Initializes the <see cref="ContactList" /> on the given client.
    /// </summary>
    /// <remarks>
    ///   This method should not be called until the Client receives the
    ///   <see cref="ServerSupport" /> is populated. An easy way to make sure is to wait until the
    ///   Ready event of the Client.
    /// </remarks>
    public void Initialize(Client client)
    {
      if (client == null)
      {
        throw new ArgumentNullException("client");
      }
      ServerSupport support = client.ServerSupports;
      this.Client = client;
      this.Users = new UserCollection();

      if (support.MaxWatches > 0)
      {
        this.tracker = new ContactsWatchTracker(this);
      }
      else if (support.MaxMonitors > 0)
      {
        this.tracker = new ContactsMonitorTracker(this);
      }
      else
      {
        this.tracker = new ContactsAreOnTracker(this);
      }

      this.tracker.Initialize();
    }

    #region IDisposable

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting
    ///   unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.tracker != null && this.tracker is ContactsAreOnTracker)
      {
        ((IDisposable) this.tracker).Dispose();
      }
    }

    #endregion
  }
}
