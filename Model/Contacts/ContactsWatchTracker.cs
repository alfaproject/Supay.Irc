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
      foreach (string nick in nicks)
      {
        addMsg.AddedNicks.Add(nick);
      }
      this.Contacts.Client.Send(addMsg);
    }

    protected override void AddNick(string nick)
    {
      var addMsg = new WatchListEditorMessage();
      addMsg.AddedNicks.Add(nick);
      this.Contacts.Client.Send(addMsg);
    }

    protected override void RemoveNick(string nick)
    {
      var remMsg = new WatchListEditorMessage();
      remMsg.RemovedNicks.Add(nick);
      this.Contacts.Client.Send(remMsg);
    }

    #region Reply Handlers

    private void ClientWatchedUserOnline(object sender, IrcMessageEventArgs<WatchedUserOnlineMessage> e)
    {
      User knownUser = this.Contacts.Users.Find(e.Message.WatchedUser.Nickname);
      if (knownUser != null)
      {
        knownUser.Online = true;
      }
    }

    private void ClientWatchedUserOffline(object sender, IrcMessageEventArgs<WatchedUserOfflineMessage> e)
    {
      User knownUser = this.Contacts.Users.Find(e.Message.WatchedUser.Nickname);
      if (knownUser != null)
      {
        knownUser.Online = false;
      }
    }

    #endregion
  }
}
