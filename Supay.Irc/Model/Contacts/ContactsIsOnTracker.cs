using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Supay.Irc.Messages;
using Supay.Irc.Network;

namespace Supay.Irc.Contacts
{
    internal sealed class ContactsIsOnTracker : ContactsTracker, IDisposable
    {
        private readonly ICollection<string> _trackedNicks = new List<string>();
        private readonly ICollection<string> _waitingOnNicks = new List<string>();
        private Timer _timer;

        public ContactsIsOnTracker(ContactList contacts)
            : base(contacts)
        {
        }

        public override void Initialize()
        {
            this.Contacts.Client.Messages.IsOnReply += this.ClientIsOnReply;
            base.Initialize();
            if (this._timer != null)
            {
                this._timer.Dispose();
            }
            this._timer = new Timer();
            this._timer.Elapsed += this.TimerElapsed;
            this._timer.Start();
        }

        protected override async Task AddNicks(IEnumerable<string> nicks)
        {
            foreach (var nick in nicks.Where(nick => !_trackedNicks.Contains(nick)))
            {
                _trackedNicks.Add(nick);
            }
        }

        protected override async Task RemoveNicks(IEnumerable<string> nicks)
        {
            foreach (var nick in nicks.Where(nick => _trackedNicks.Contains(nick)))
            {
                _trackedNicks.Remove(nick);
            }
        }


        #region Event Handlers

        private void ClientIsOnReply(object sender, IrcMessageEventArgs<IsOnReplyMessage> e)
        {
            foreach (string onlineNick in e.Message.Nicks)
            {
                if (this._waitingOnNicks.Contains(onlineNick))
                {
                    this._waitingOnNicks.Remove(onlineNick);
                }
                User knownUser;
                if (this.Contacts.Users.TryGetValue(onlineNick, out knownUser))
                {
                    knownUser.Online = true;
                }
                else if (this._trackedNicks.Contains(onlineNick))
                {
                    this._trackedNicks.Remove(onlineNick);
                }
            }

            foreach (var offlineUser in this._waitingOnNicks.Select(nick => this.Contacts.Users[nick]))
            {
                offlineUser.Online = false;
            }
            this._waitingOnNicks.Clear();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.Contacts.Client.Connection.Status == ConnectionStatus.Connected)
            {
                var ison = new IsOnMessage();
                foreach (string nick in this._trackedNicks)
                {
                    ison.Nicks.Add(nick);
                    if (!this._waitingOnNicks.Contains(nick))
                    {
                        this._waitingOnNicks.Add(nick);
                    }
                }
                await this.Contacts.Client.Send(ison);
            }
        }

        #endregion


        #region IDisposable

        /// <summary>
        /// Releases all resources used by the <see cref="ContactsIsOnTracker"/>.
        /// </summary>
        public void Dispose()
        {
            if (this._timer != null)
            {
                this._timer.Close();
            }
        }

        #endregion
    }
}
