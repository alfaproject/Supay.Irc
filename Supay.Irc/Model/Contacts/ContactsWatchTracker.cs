using System.Collections.Generic;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts
{
    internal sealed class ContactsWatchTracker : ContactsTracker
    {
        public ContactsWatchTracker(ContactList contacts)
            : base(contacts)
        {
        }

        public override void Initialize()
        {
            this.Contacts.Client.Messages.WatchedUserOffline += this.ClientWatchedUserOffline;
            this.Contacts.Client.Messages.WatchedUserOnline += this.ClientWatchedUserOnline;
            base.Initialize();
        }

        protected override void AddNicks(IEnumerable<string> nicks)
        {
            var addMsg = new WatchListEditorMessage();
            foreach (var nick in nicks)
            {
                addMsg.AddedNicks.Add(nick);
            }
            Contacts.Client.Send(addMsg);
        }

        protected override void RemoveNicks(IEnumerable<string> nicks)
        {
            var remMsg = new WatchListEditorMessage();
            foreach (var nick in nicks)
            {
                remMsg.RemovedNicks.Add(nick);
            }
            Contacts.Client.Send(remMsg);
        }


        #region Reply Handlers

        private void ClientWatchedUserOnline(object sender, IrcMessageEventArgs<WatchedUserOnlineMessage> e)
        {
            User knownUser;
            if (this.Contacts.Users.TryGetValue(e.Message.WatchedUser.Nickname, out knownUser))
            {
                knownUser.Online = true;
            }
        }

        private void ClientWatchedUserOffline(object sender, IrcMessageEventArgs<WatchedUserOfflineMessage> e)
        {
            User knownUser;
            if (this.Contacts.Users.TryGetValue(e.Message.WatchedUser.Nickname, out knownUser))
            {
                knownUser.Online = false;
            }
        }

        #endregion
    }
}
