using System;
using System.Collections.Specialized;

namespace Supay.Irc.Contacts {

  internal abstract class ContactsTracker {
  
    public ContactsTracker(ContactList contacts) {
      this.contacts = contacts;
      this.contacts.Users.CollectionChanged += new NotifyCollectionChangedEventHandler(Users_CollectionChanged);
    }

    void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
      if (e.Action == NotifyCollectionChangedAction.Add) {
        foreach (User newUser in e.NewItems) {
          this.AddNick(newUser.Nickname);
        }
      }
      if (e.Action == NotifyCollectionChangedAction.Remove) {
        foreach (User oldUser in e.OldItems) {
          this.RemoveNick(oldUser.Nickname);
        }
      }
    }

    ContactList contacts;

    protected ContactList Contacts {
      get {
        return contacts;
      }
    }

    public virtual void Initialize() {
      StringCollection nicks = new StringCollection();
      foreach (User u in this.Contacts.Users) {
        nicks.Add(u.Nickname);
      }
      this.AddNicks(nicks);
    }

    protected abstract void AddNicks(StringCollection nicks);

    protected abstract void AddNick(String nick);

    protected abstract void RemoveNick(String nick);

  }

}