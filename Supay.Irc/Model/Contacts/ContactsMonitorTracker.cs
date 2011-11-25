using System.Collections.Generic;
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
      foreach (var onlineUser in e.Message.Users.Values)
      {
        User knownUser;
        if (this.Contacts.Users.TryGetValue(onlineUser.Nickname, out knownUser))
        {
          knownUser.CopyFrom(onlineUser);
          knownUser.Online = true;
        }
      }
    }

    private void ClientMonitoredUserOffline(object sender, IrcMessageEventArgs<MonitoredUserOfflineMessage> e)
    {
      foreach (var offlineNick in e.Message.Nicks)
      {
        User knownUser;
        if (this.Contacts.Users.TryGetValue(offlineNick, out knownUser))
        {
          knownUser.Online = false;
        }
      }
    }

    #endregion
  }
}
