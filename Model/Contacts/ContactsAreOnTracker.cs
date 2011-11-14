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
    private readonly IList<string> _trackedNicks = new Collection<string>();
    private readonly IList<string> _waitingOnNicks = new Collection<string>();
    private Timer _timer;

    public ContactsAreOnTracker(ContactList contacts)
      : base(contacts)
    {
    }

    public override void Initialize()
    {
      this.Contacts.Client.Messages.IsOnReply += this.client_IsOnReply;
      base.Initialize();
      if (this._timer != null)
      {
        this._timer.Dispose();
      }
      this._timer = new Timer();
      this._timer.Elapsed += this.timer_Elapsed;
      this._timer.Start();
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
      if (!this._trackedNicks.Contains(nick))
      {
        this._trackedNicks.Add(nick);
      }
    }

    protected override void RemoveNick(string nick)
    {
      if (this._trackedNicks.Contains(nick))
      {
        this._trackedNicks.Remove(nick);
      }
    }

    #region Event Handlers

    private void client_IsOnReply(object sender, IrcMessageEventArgs<IsOnReplyMessage> e)
    {
      foreach (string onlineNick in e.Message.Nicks)
      {
        if (this._waitingOnNicks.Contains(onlineNick))
        {
          this._waitingOnNicks.Remove(onlineNick);
        }
        User knownUser = this.Contacts.Users.Find(onlineNick);
        if (knownUser != null)
        {
          knownUser.Online = true;
        }
        else if (this._trackedNicks.Contains(onlineNick))
        {
          this._trackedNicks.Remove(onlineNick);
        }
      }
      foreach (string nick in this._waitingOnNicks)
      {
        User offlineUser = this.Contacts.Users.Find(nick);
        offlineUser.Online = false;
        this._waitingOnNicks.Remove(nick);
      }
    }

    private void timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.Contacts.Client.Connection.Status == ConnectionStatus.Connected)
      {
        IsOnMessage ison = new IsOnMessage();
        foreach (string nick in this._trackedNicks)
        {
          ison.Nicks.Add(nick);
          if (!this._waitingOnNicks.Contains(nick))
          {
            this._waitingOnNicks.Add(nick);
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
      if (disposing && this._timer != null)
      {
        this._timer.Dispose();
      }
    }

    #endregion
  }
}
