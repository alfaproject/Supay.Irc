using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   A Monitor system error message informing the user which nicks couldn't be added 
    ///   to their monitor list because it is full.
    /// </summary>
    [Serializable]
    public class MonitorListFullMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="MonitorListFullMessage" />.
        /// </summary>
        public MonitorListFullMessage()
            : base(734)
        {
            Nicks = new List<string>();
        }

        /// <summary>
        ///   Gets or sets the limit of monitor lists on the server.
        /// </summary>
        public int Limit
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the collection of nicks which couldn't be added to the monitor list.
        /// </summary>
        public ICollection<string> Nicks
        {
            get;
            private set;
        }

        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.Limit.ToString(CultureInfo.InvariantCulture));
            parameters.Add(string.Join(",", this.Nicks));
            parameters.Add("Monitor list is full.");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.Limit = -1;
            this.Nicks.Clear();

            if (parameters.Count > 2)
            {
                this.Limit = Convert.ToInt32(parameters[1], CultureInfo.InvariantCulture);

                string userListParam = parameters[2];
                var userList = userListParam.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string nick in userList)
                {
                    this.Nicks.Add(nick);
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnMonitorListFull(new IrcMessageEventArgs<MonitorListFullMessage>(this));
        }
    }
}
