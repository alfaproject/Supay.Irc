using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Supay.Irc.Messages;
using Supay.Irc.Network;

namespace Supay.Irc.Contacts
{
  internal class ContactsAreOnTracker : ContactsTracker, IDisposable
  {
    private readonly IList<string> trackedNicks = new Collection<string>();
    private readonly IList<string> waitingOnNicks = new Collection<string>();
    private Timer timer;

    public ContactsAreOnTracker(ContactList contacts)
      : base(contacts)
    {
    }

    public override void Initialize()
    {
      this.Contacts.Client.Messages.IsOnReply += this.ClientIsOnReply;
      base.Initialize();
      if (this.timer != null)
      {
        this.timer.Dispose();
      }
      this.timer = new Timer();
      this.timer.Elapsed += this.TimerElapsed;
      this.timer.Start();
    }

    protected override void AddNicks(IEnumerable<string> nicks)
    {
      foreach (string nick in nicks)
      {
        this.AddNick(nick);
      }
    }

    protected override void AddNick(string nick)
    {
      if (!this.trackedNicks.Contains(nick))
      {
        this.trackedNicks.Add(nick);
      }
    }

    protected override void RemoveNick(string nick)
    {
      if (this.trackedNicks.Contains(nick))
      {
        this.trackedNicks.Remove(nick);
      }
    }

    #region Event Handlers

    private void ClientIsOnReply(object sender, IrcMessageEventArgs<IsOnReplyMessage> e)
    {
      foreach (string onlineNick in e.Message.Nicks)
      {
        if (this.waitingOnNicks.Contains(onlineNick))
        {
          this.waitingOnNicks.Remove(onlineNick);
        }
        User knownUser = this.Contacts.Users.Find(onlineNick);
        if (knownUser != null)
        {
          knownUser.Online = true;
        }
        else if (this.trackedNicks.Contains(onlineNick))
        {
          this.trackedNicks.Remove(onlineNick);
        }
      }
      foreach (string nick in this.waitingOnNicks)
      {
        User offlineUser = this.Contacts.Users.Find(nick);
        offlineUser.Online = false;
        this.waitingOnNicks.Remove(nick);
      }
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (this.Contacts.Client.Connection.Status == ConnectionStatus.Connected)
      {
        var ison = new IsOnMessage();
        foreach (string nick in this.trackedNicks)
        {
          ison.Nicks.Add(nick);
          if (!this.waitingOnNicks.Contains(nick))
          {
            this.waitingOnNicks.Add(nick);
          }
        }
        this.Contacts.Client.Send(ison);
      }
    }

    #endregion

    #region IDisposable

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting
    ///   unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.timer != null)
      {
        this.timer.Dispose();
      }
    }

    #endregion
  }
}
