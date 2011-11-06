using System;
using System.Collections.Generic;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts {
  internal class ContactsWatchTracker : ContactsTracker {
    public ContactsWatchTracker(ContactList contacts)
      : base(contacts) {
    }

    public override void Initialize() {
      Contacts.Client.Messages.WatchedUserOffline += new EventHandler<IrcMessageEventArgs<WatchedUserOfflineMessage>>(client_WatchedUserOffline);
      Contacts.Client.Messages.WatchedUserOnline += new EventHandler<IrcMessageEventArgs<WatchedUserOnlineMessage>>(client_WatchedUserOnline);
      base.Initialize();
    }

    protected override void AddNicks(IEnumerable<string> nicks) {
      WatchListEditorMessage addMsg = new WatchListEditorMessage();
      foreach (string nick in nicks) {
        addMsg.AddedNicks.Add(nick);
      }
      Contacts.Client.Send(addMsg);
    }

    protected override void AddNick(string nick) {
      WatchListEditorMessage addMsg = new WatchListEditorMessage();
      addMsg.AddedNicks.Add(nick);
      Contacts.Client.Send(addMsg);
    }

    protected override void RemoveNick(string nick) {
      WatchListEditorMessage remMsg = new WatchListEditorMessage();
      remMsg.RemovedNicks.Add(nick);
      Contacts.Client.Send(remMsg);
    }

    #region Reply Handlers

    private void client_WatchedUserOnline(object sender, IrcMessageEventArgs<WatchedUserOnlineMessage> e) {
      User knownUser = Contacts.Users.Find(e.Message.WatchedUser.Nickname);
      if (knownUser != null) {
        knownUser.Online = true;
      }
    }

    private void client_WatchedUserOffline(object sender, IrcMessageEventArgs<WatchedUserOfflineMessage> e) {
      User knownUser = Contacts.Users.Find(e.Message.WatchedUser.Nickname);
      if (knownUser != null) {
        knownUser.Online = false;
      }
    }

    #endregion
  }
}
