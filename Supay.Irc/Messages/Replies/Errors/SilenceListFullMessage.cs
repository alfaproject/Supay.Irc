using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> received when an user's silence list is full, and a
    ///   <see cref="SilenceMessage" /> is sent adding an user to the list.
    /// </summary>
    [Serializable]
    public class SilenceListFullMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="SilenceListFullMessage" /> class.
        /// </summary>
        public SilenceListFullMessage()
            : base(511)
        {
        }

        /// <summary>
        ///   Gets or sets the mask of the user being silenced.
        /// </summary>
        public Mask SilenceMask
        {
            get;
            set;
        }

        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.SilenceMask.ToString());
            parameters.Add("Your silence list is full");
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            this.SilenceMask = parameters.Count > 1 ? new Mask(parameters[1]) : new Mask();
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnSilenceListFull(new IrcMessageEventArgs<SilenceListFullMessage>(this));
        }
    }
}
