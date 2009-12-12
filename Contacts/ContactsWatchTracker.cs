using System;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts {

  internal class ContactsWatchTracker : ContactsTracker {
  
    public ContactsWatchTracker(ContactList contacts)
      : base(contacts) {
    }

    public override void Initialize() {
      this.Contacts.Client.Messages.WatchedUserOffline += new EventHandler<Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.WatchedUserOfflineMessage>>(client_WatchedUserOffline);
      this.Contacts.Client.Messages.WatchedUserOnline += new EventHandler<Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.WatchedUserOnlineMessage>>(client_WatchedUserOnline);
      base.Initialize();
    }

    protected override void AddNicks(System.Collections.Specialized.StringCollection nicks) {
      WatchListEditorMessage addMsg = new WatchListEditorMessage();
      foreach (String nick in nicks) {
        addMsg.AddedNicks.Add(nick);
      }
      this.Contacts.Client.Send(addMsg);
    }

    protected override void AddNick(String nick) {
      WatchListEditorMessage addMsg = new WatchListEditorMessage();
      addMsg.AddedNicks.Add(nick);
      this.Contacts.Client.Send(addMsg);
    }

    protected override void RemoveNick(String nick) {
      WatchListEditorMessage remMsg = new WatchListEditorMessage();
      remMsg.RemovedNicks.Add(nick);
      this.Contacts.Client.Send(remMsg);
    }

    #region Reply Handlers

    void client_WatchedUserOnline(object sender, Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.WatchedUserOnlineMessage> e) {
      User knownUser = this.Contacts.Users.Find(e.Message.WatchedUser.Nick);
      if (knownUser != null && knownUser.OnlineStatus == UserOnlineStatus.Offline) {
        knownUser.OnlineStatus = UserOnlineStatus.Online;
      }
    }

    void client_WatchedUserOffline(object sender, Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.WatchedUserOfflineMessage> e) {
      User knownUser = this.Contacts.Users.Find(e.Message.WatchedUser.Nick);
      if (knownUser != null) {
        knownUser.OnlineStatus = UserOnlineStatus.Offline;
      }
    }

    #endregion

  }

}