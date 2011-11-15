using System.Collections.Generic;
using System.Linq;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts
{
  internal sealed class ContactsMonitorTracker : ContactsTracker
  {
    public ContactsMonitorTracker(ContactList contacts)
      : base(contacts)
    {
    }

    public override void Initialize()
    {
      this.Contacts.Client.Messages.MonitoredUserOffline += this.ClientMonitoredUserOffline;
      this.Contacts.Client.Messages.MonitoredUserOnline += this.ClientMonitoredUserOnline;
      base.Initialize();
    }

    protected override void AddNicks(IEnumerable<string> nicks)
    {
      var add = new MonitorAddUsersMessage();
      foreach (string nick in nicks)
      {
        add.Nicks.Add(nick);
      }
      this.Contacts.Client.Send(add);
    }

    protected override void AddNick(string nick)
    {
      var add = new MonitorAddUsersMessage();
      add.Nicks.Add(nick);
      this.Contacts.Client.Send(add);
    }

    protected override void RemoveNick(string nick)
    {
      var remove = new MonitorRemoveUsersMessage();
      remove.Nicks.Add(nick);
      this.Contacts.Client.Send(remove);
    }

    #region Reply Handlers

    private void ClientMonitoredUserOnline(object sender, IrcMessageEventArgs<MonitoredUserOnlineMessage> e)
    {
      foreach (User onlineUser in e.Message.Users)
      {
        User knownUser = this.Contacts.Users.Find(onlineUser.Nickname);
        if (knownUser != null)
        {
          knownUser.CopyFrom(onlineUser);
          knownUser.Online = true;
        }
      }
    }

    private void ClientMonitoredUserOffline(object sender, IrcMessageEventArgs<MonitoredUserOfflineMessage> e)
    {
      foreach (User knownUser in e.Message.Nicks.Select(offlineNick => this.Contacts.Users.Find(offlineNick)).Where(knownUser => knownUser != null))
      {
        knownUser.Online = false;
      }
    }

    #endregion
  }
}
