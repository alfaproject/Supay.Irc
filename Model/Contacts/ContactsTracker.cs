using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Supay.Irc.Contacts {
  internal abstract class ContactsTracker {
    private readonly ContactList contacts;

    protected ContactsTracker(ContactList contacts) {
      this.contacts = contacts;
      this.contacts.Users.CollectionChanged += Users_CollectionChanged;
    }

    protected ContactList Contacts {
      get {
        return contacts;
      }
    }

    private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      if (e.Action == NotifyCollectionChangedAction.Add) {
        foreach (User newUser in e.NewItems) {
          AddNick(newUser.Nickname);
        }
      }
      if (e.Action == NotifyCollectionChangedAction.Remove) {
        foreach (User oldUser in e.OldItems) {
          RemoveNick(oldUser.Nickname);
        }
      }
    }

    public virtual void Initialize() {
      var nicks = new Collection<string>();
      foreach (User u in Contacts.Users) {
        nicks.Add(u.Nickname);
      }
      AddNicks(nicks);
    }

    protected abstract void AddNicks(IEnumerable<string> nicks);

    protected abstract void AddNick(string nick);

    protected abstract void RemoveNick(string nick);
  }
}
