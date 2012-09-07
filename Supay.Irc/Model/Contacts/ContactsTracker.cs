using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Supay.Irc.Contacts
{
    internal abstract class ContactsTracker
    {
        private readonly ContactList contacts;

        protected ContactsTracker(ContactList contacts)
        {
            this.contacts = contacts;
            this.contacts.Users.CollectionChanged += this.UsersCollectionChanged;
        }

        protected ContactList Contacts
        {
            get
            {
                return this.contacts;
            }
        }

        private void UsersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddNicks(from User newUser in e.NewItems
                                  select newUser.Nickname);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveNicks(from User oldUser in e.NewItems
                                  select oldUser.Nickname);
                    break;
            }
        }

        public virtual void Initialize()
        {
            this.AddNicks(from u in Contacts.Users.Values
                          select u.Nickname);
        }

        protected abstract void AddNicks(IEnumerable<string> nicks);

        protected void AddNick(string nick)
        {
            AddNicks(new[] { nick });
        }

        protected abstract void RemoveNicks(IEnumerable<string> nicks);
        
        protected void RemoveNick(string nick)
        {
            RemoveNicks(new[] { nick });
        }
    }
}
