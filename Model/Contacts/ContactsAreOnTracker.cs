using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Supay.Irc.Messages;

namespace Supay.Irc.Contacts {
  internal class ContactsAreOnTracker : ContactsTracker, IDisposable {
    private readonly Collection<string> _trackedNicks = new Collection<string>();
    private readonly Collection<string> _waitingOnNicks = new Collection<string>();
    private Timer _timer;

    public ContactsAreOnTracker(ContactList contacts)
      : base(contacts) {
    }

    public override void Initialize() {
      Contacts.Client.Messages.IsOnReply += client_IsOnReply;
      base.Initialize();
      if (_timer != null) {
        _timer.Dispose();
      }
      _timer = new Timer();
      _timer.Elapsed += timer_Elapsed;
      _timer.Start();
    }

    protected override void AddNicks(IEnumerable<string> nicks) {
      foreach (string nick in nicks) {
        AddNick(nick);
      }
    }

    protected override void AddNick(string nick) {
      if (!_trackedNicks.Contains(nick)) {
        _trackedNicks.Add(nick);
      }
    }

    protected override void RemoveNick(string nick) {
      if (_trackedNicks.Contains(nick)) {
        _trackedNicks.Remove(nick);
      }
    }

    #region Event Handlers

    private void client_IsOnReply(object sender, IrcMessageEventArgs<IsOnReplyMessage> e) {
      foreach (string onlineNick in e.Message.Nicks) {
        if (_waitingOnNicks.Contains(onlineNick)) {
          _waitingOnNicks.Remove(onlineNick);
        }
        User knownUser = Contacts.Users.Find(onlineNick);
        if (knownUser != null) {
          knownUser.Online = true;
        } else if (_trackedNicks.Contains(onlineNick)) {
          _trackedNicks.Remove(onlineNick);
        }
      }
      foreach (string nick in _waitingOnNicks) {
        User offlineUser = Contacts.Users.Find(nick);
        offlineUser.Online = false;
        _waitingOnNicks.Remove(nick);
      }
    }

    private void timer_Elapsed(object sender, ElapsedEventArgs e) {
      if (Contacts.Client.Connection.Status == Network.ConnectionStatus.Connected) {
        IsOnMessage ison = new IsOnMessage();
        foreach (string nick in _trackedNicks) {
          ison.Nicks.Add(nick);
          if (!_waitingOnNicks.Contains(nick)) {
            _waitingOnNicks.Add(nick);
          }
        }
        Contacts.Client.Send(ison);
      }
    }

    #endregion

    #region IDisposable

    protected virtual void Dispose(bool disposing) {
      if (disposing && _timer != null) {
        _timer.Dispose();
      }
    }

    /// <summary>
    ///   Performs application-defined tasks associated with freeing, releasing, or resetting
    ///   unmanaged resources. </summary>
    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
