using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   The <see cref="ListMessage" /> is used to list channels and their topics.
    /// </summary>
    /// <remarks>
    ///   A server sent a <see cref="ListMessage" /> will reply with a 
    ///   <see cref="ListStartReplyMessage" />, <see cref="ListReplyMessage" />, and a <see cref="ListEndReplyMessage" />.
    /// </remarks>
    [Serializable]
    public class ListMessage : CommandMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ListMessage" /> class with the given channels.
        /// </summary>
        public ListMessage(params string[] channels)
        {
            Channels = new List<string>(channels);
            Server = string.Empty;
            MaxUsers = -1;
            MinUsers = -1;
            YoungerThan = -1;
            OlderThan = -1;
            MatchMask = string.Empty;
            NotMatchMask = string.Empty;
            TopicOlderThan = -1;
            TopicYoungerThan = -1;
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="ListMessage" /> class.
        /// </summary>
        public ListMessage()
            : this(new string[] { })
        {
        }

        /// <summary>
        ///   Gets the channels to get info about.
        /// </summary>
        /// <remarks>
        ///   If this is empty, information about all channels is requested.
        /// </remarks>
        public List<string> Channels
        {
            get;
            private set;
        }

        /// <summary>
        ///   Gets or sets the server that should return the info.
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the maximum number of users that channels can have to be returned.
        /// </summary>
        public int MaxUsers
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the minimum number of users that channels can have to be returned.
        /// </summary>
        public int MinUsers
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets, in minutes, the longest amount of time 
        ///   which may have passed since a channel was created in order to be returned.
        /// </summary>
        public int YoungerThan
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets, in minutes, the shortest amount of time 
        ///   which may have passed since a channel was created in order to be returned.
        /// </summary>
        public int OlderThan
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the a mask which a channel must match to be returned.
        /// </summary>
        public string MatchMask
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a mask which a channel cannot match to be returned.
        /// </summary>
        public string NotMatchMask
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets, in minutes, the shortest amount of time 
        ///   which may have passed since a channel's topic was changed, to be returned.
        /// </summary>
        /// <remarks>
        ///   Setting this property to "5" will cause only channels to be returned where their
        ///   topic hasn't been changed in the last 5 minutes.
        /// </remarks>
        public int TopicOlderThan
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets, in minutes, the longest amount of time 
        ///   which may have passed since a channel's topic was changed, to be returned.
        /// </summary>
        /// <remarks>
        ///   Setting this property to "5" will cause only channels to be returned where their
        ///   topic has been changed in the last 5 minutes.
        /// </remarks>
        public int TopicYoungerThan
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the IRC command associated with this message.
        /// </summary>
        protected override string Command
        {
            get
            {
                return "LIST";
            }
        }

        public override void Validate(ServerSupport serverSupport)
        {
            base.Validate(serverSupport);
            if (serverSupport != null)
            {
                if (this.MaxUsers >= 0 || this.MinUsers >= 0)
                {
                    VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.UserCount);
                }
                if (this.YoungerThan >= 0 || this.OlderThan >= 0)
                {
                    VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.CreationTime);
                }
                if (!string.IsNullOrEmpty(this.MatchMask))
                {
                    VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Mask);
                }
                if (!string.IsNullOrEmpty(this.NotMatchMask))
                {
                    VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.NotMask);
                }
                if (this.TopicOlderThan >= 0 || this.TopicYoungerThan >= 0)
                {
                    VerifySupport(serverSupport, ServerSupport.ExtendedListParameters.Topic);
                }
            }
        }

        private static void VerifySupport(ServerSupport serverSupport, ServerSupport.ExtendedListParameters parameter)
        {
            if (!serverSupport.ExtendedList.HasFlag(parameter))
            {
                throw new InvalidMessageException(string.Format(CultureInfo.InvariantCulture, Resources.ServerDoesNotSupportExtendedListParameter, parameter));
            }
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var options = new List<string>();
            if (this.MaxUsers >= 0)
            {
                options.Add("<" + this.MaxUsers.ToString(CultureInfo.InvariantCulture));
            }
            if (this.MinUsers >= 0)
            {
                options.Add(">" + this.MinUsers.ToString(CultureInfo.InvariantCulture));
            }
            if (this.YoungerThan >= 0)
            {
                options.Add("C<" + this.YoungerThan.ToString(CultureInfo.InvariantCulture));
            }
            if (this.OlderThan >= 0)
            {
                options.Add("C>" + this.OlderThan.ToString(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrEmpty(this.MatchMask))
            {
                options.Add("*" + this.MatchMask + "*");
            }
            if (!string.IsNullOrEmpty(this.NotMatchMask))
            {
                options.Add("!*" + this.NotMatchMask + "*");
            }
            if (this.TopicOlderThan >= 0)
            {
                options.Add("T>" + this.TopicOlderThan.ToString(CultureInfo.InvariantCulture));
            }
            if (this.TopicYoungerThan >= 0)
            {
                options.Add("T<" + this.TopicYoungerThan.ToString(CultureInfo.InvariantCulture));
            }

            var parameters = base.GetTokens();
            if (options.Count != 0)
            {
                parameters.Add(string.Join(",", options));
            }
            else if (this.Channels.Count != 0)
            {
                parameters.Add(string.Join(",", this.Channels));
                if (!string.IsNullOrEmpty(this.Server))
                {
                    parameters.Add(this.Server);
                }
            }
            return parameters;
        }

        /// <summary>
        ///   Parses the parameters portion of the message.
        /// </summary>
        protected override void ParseParameters(IList<string> parameters)
        {
            base.ParseParameters(parameters);

            this.Channels.Clear();
            this.Server = string.Empty;
            this.MatchMask = string.Empty;
            this.NotMatchMask = string.Empty;
            this.MaxUsers = -1;
            this.MinUsers = -1;
            this.OlderThan = -1;
            this.YoungerThan = -1;
            this.TopicOlderThan = -1;
            this.TopicYoungerThan = -1;

            if (parameters.Count >= 1)
            {
                if (IsExtendedParameter(parameters[0]))
                {
                    foreach (string extOption in parameters[0].Split(','))
                    {
                        if (extOption.StartsWith("!*", StringComparison.Ordinal))
                        {
                            this.NotMatchMask = MessageUtil.StringBetweenStrings(extOption, "!*", "*");
                        }
                        else if (extOption.StartsWith("*", StringComparison.Ordinal))
                        {
                            this.MatchMask = MessageUtil.StringBetweenStrings(extOption, "*", "*");
                        }
                        else if (extOption.StartsWith("C>", StringComparison.Ordinal))
                        {
                            this.OlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
                        }
                        else if (extOption.StartsWith("C<", StringComparison.Ordinal))
                        {
                            this.YoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
                        }
                        else if (extOption.StartsWith("T>", StringComparison.Ordinal))
                        {
                            this.TopicOlderThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
                        }
                        else if (extOption.StartsWith("T<", StringComparison.Ordinal))
                        {
                            this.TopicYoungerThan = Convert.ToInt32(extOption.Substring(2), CultureInfo.InvariantCulture);
                        }
                        else if (extOption.StartsWith(">", StringComparison.Ordinal))
                        {
                            this.MinUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
                        }
                        else if (extOption.StartsWith("<", StringComparison.Ordinal))
                        {
                            this.MaxUsers = Convert.ToInt32(extOption.Substring(1), CultureInfo.InvariantCulture);
                        }
                    }
                }
                else if (MessageUtil.HasValidChannelPrefix(parameters[0]))
                {
                    this.Channels.AddRange(parameters[0].Split(','));
                    if (parameters.Count >= 2)
                    {
                        this.Server = parameters[1];
                    }
                }
            }
        }

        private static bool IsExtendedParameter(string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return false;
            }
            var exList = new[] { "!*", "*", "<", ">", "T", "C" };
            return exList.Any(extStart => p.StartsWith(extStart, StringComparison.Ordinal));
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnList(new IrcMessageEventArgs<ListMessage>(this));
        }
    }
}
