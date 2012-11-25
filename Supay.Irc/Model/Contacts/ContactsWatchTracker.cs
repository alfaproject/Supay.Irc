using System.Collections.Generic;
using System.Threading.Tasks;
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

        protected override async Task AddNicks(IEnumerable<string> nicks)
        {
            var addMsg = new WatchListEditorMessage();
            foreach (var nick in nicks)
            {
                addMsg.AddedNicks.Add(nick);
            }
            await Contacts.Client.Send(addMsg);
        }

        protected override async Task RemoveNicks(IEnumerable<string> nicks)
        {
            var remMsg = new WatchListEditorMessage();
            foreach (var nick in nicks)
            {
                remMsg.RemovedNicks.Add(nick);
            }
            await Contacts.Client.Send(remMsg);
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
