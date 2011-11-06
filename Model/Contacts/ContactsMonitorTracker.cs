using System.Collections.Generic;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts {
  internal class ContactsMonitorTracker : ContactsTracker {
    public ContactsMonitorTracker(ContactList contacts)
      : base(contacts) {
    }

    public override void Initialize() {
      Contacts.Client.Messages.MonitoredUserOffline += client_MonitoredUserOffline;
      Contacts.Client.Messages.MonitoredUserOnline += client_MonitoredUserOnline;
      base.Initialize();
    }

    protected override void AddNicks(IEnumerable<string> nicks) {
      MonitorAddUsersMessage add = new MonitorAddUsersMessage();
      foreach (string nick in nicks) {
        add.Nicks.Add(nick);
      }
      Contacts.Client.Send(add);
    }

    protected override void AddNick(string nick) {
      MonitorAddUsersMessage add = new MonitorAddUsersMessage();
      add.Nicks.Add(nick);
      Contacts.Client.Send(add);
    }

    protected override void RemoveNick(string nick) {
      MonitorRemoveUsersMessage remove = new MonitorRemoveUsersMessage();
      remove.Nicks.Add(nick);
      Contacts.Client.Send(remove);
    }

    #region Reply Handlers

    private void client_MonitoredUserOnline(object sender, IrcMessageEventArgs<MonitoredUserOnlineMessage> e) {
      foreach (User onlineUser in e.Message.Users) {
        User knownUser = Contacts.Users.Find(onlineUser.Nickname);
        if (knownUser != null) {
          knownUser.CopyFrom(onlineUser);
          knownUser.Online = true;
        }
      }
    }

    private void client_MonitoredUserOffline(object sender, IrcMessageEventArgs<MonitoredUserOfflineMessage> e) {
      foreach (string offlineNick in e.Message.Nicks) {
        User knownUser = Contacts.Users.Find(offlineNick);
        if (knownUser != null) {
          knownUser.Online = false;
        }
      }
    }

    #endregion
  }
}
