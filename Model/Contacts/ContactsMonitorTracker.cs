using System;
using System.Collections.Generic;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts {
  internal class ContactsMonitorTracker : ContactsTracker {
    public ContactsMonitorTracker(ContactList contacts)
      : base(contacts) {
    }

    public override void Initialize() {
      this.Contacts.Client.Messages.MonitoredUserOffline += new EventHandler<Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.MonitoredUserOfflineMessage>>(client_MonitoredUserOffline);
      this.Contacts.Client.Messages.MonitoredUserOnline += new EventHandler<Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.MonitoredUserOnlineMessage>>(client_MonitoredUserOnline);
      base.Initialize();
    }

    protected override void AddNicks(IEnumerable<string> nicks) {
      MonitorAddUsersMessage add = new MonitorAddUsersMessage();
      foreach (string nick in nicks) {
        add.Nicks.Add(nick);
      }
      this.Contacts.Client.Send(add);
    }

    protected override void AddNick(string nick) {
      MonitorAddUsersMessage add = new MonitorAddUsersMessage();
      add.Nicks.Add(nick);
      this.Contacts.Client.Send(add);
    }

    protected override void RemoveNick(string nick) {
      MonitorRemoveUsersMessage remove = new MonitorRemoveUsersMessage();
      remove.Nicks.Add(nick);
      this.Contacts.Client.Send(remove);
    }

    #region Reply Handlers

    private void client_MonitoredUserOnline(object sender, Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.MonitoredUserOnlineMessage> e) {
      foreach (User onlineUser in e.Message.Users) {
        User knownUser = this.Contacts.Users.Find(onlineUser.Nickname);
        if (knownUser != null) {
          knownUser.CopyFrom(onlineUser);
          knownUser.Online = true;
        }
      }
    }

    private void client_MonitoredUserOffline(object sender, Supay.Irc.Messages.IrcMessageEventArgs<Supay.Irc.Messages.MonitoredUserOfflineMessage> e) {
      foreach (string offlineNick in e.Message.Nicks) {
        User knownUser = this.Contacts.Users.Find(offlineNick);
        if (knownUser != null) {
          knownUser.Online = false;
        }
      }
    }

    #endregion
  }
}
