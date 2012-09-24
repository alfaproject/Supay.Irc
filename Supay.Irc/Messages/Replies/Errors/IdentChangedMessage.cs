using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ErrorMessage" /> sent when a user tries to connect with an user name containing invalid characters
    /// </summary>
    /// <remarks>
    ///   Not all networks will send this message, some will silently change your user name,
    ///   while others will simply disconnect you.
    /// </remarks>
    [Serializable]
    public class IdentChangedMessage : ErrorMessage
    {
        /// <summary>
        ///   Creates a new instances of the <see cref="IdentChangedMessage" /> class.
        /// </summary>
        public IdentChangedMessage()
            : base(455)
        {
        }

        /// <summary>
        ///   Gets or sets the username that was attempted
        /// </summary>
        public string Ident
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the characters in the attempted username which were invalid
        /// </summary>
        public string InvalidCharacters
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the new username being assigned
        /// </summary>
        public string NewIdent
        {
            get;
            set;
        }

        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(string.Format(CultureInfo.InvariantCulture, "Your user name {0} contained the invalid character(s) {1} and has been changed to {2}. Please use only the characters 0-9 a-z A-z _ - or . in your user name. Your user name is the part before the @ in your email address.", this.Ident, this.InvalidCharacters, this.NewIdent));
            return parameters;
        }

        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);
            string param = parameters[1];
            this.Ident = MessageUtil.StringBetweenStrings(param, "Your username ", " contained the invalid ");
            this.InvalidCharacters = MessageUtil.StringBetweenStrings(param, "invalid character(s) ", " and has ");
            this.NewIdent = MessageUtil.StringBetweenStrings(param, "has been changed to ", ". Please");
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnIdentChanged(new IrcMessageEventArgs<IdentChangedMessage>(this));
        }
    }
}
