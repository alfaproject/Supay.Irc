using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Contains basic information about a server.
    /// </summary>
    [Serializable]
    public class ServerInfoMessage : NumericMessage
    {
        /// <summary>
        ///   Creates a new instance of the <see cref="ServerInfoMessage" /> class.
        /// </summary>
        public ServerInfoMessage()
            : base(004)
        {
            ServerName = string.Empty;
            Version = string.Empty;
            UserModes = string.Empty;
            UserModesWithParams = string.Empty;
            ServerModesWithParams = string.Empty;
            ServerModes = string.Empty;
            ChannelModesWithParams = string.Empty;
            ChannelModes = string.Empty;
        }

        /// <summary>
        ///   Gets or sets the name of the server being referenced.
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the version of the server.
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the user modes supported by this server.
        /// </summary>
        public string UserModes
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the channel modes supported by this server.
        /// </summary>
        public string ChannelModes
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the channel modes that require a parameter.
        /// </summary>
        public string ChannelModesWithParams
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the user modes that require a parameter.
        /// </summary>
        public string UserModesWithParams
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the server modes supported by this server.
        /// </summary>
        public string ServerModes
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the server modes which require parameters.
        /// </summary>
        public string ServerModesWithParams
        {
            get;
            set;
        }

        /// <summary>
        /// Overrides <see cref="IrcMessage.GetTokens"/>.
        /// </summary>
        protected override ICollection<string> GetTokens()
        {
            var parameters = base.GetTokens();
            parameters.Add(this.ServerName);
            parameters.Add(this.Version);
            parameters.Add(this.UserModes);
            parameters.Add(this.ChannelModes);
            if (!string.IsNullOrEmpty(this.ChannelModesWithParams))
            {
                parameters.Add(this.ChannelModesWithParams);
                if (!string.IsNullOrEmpty(this.UserModesWithParams))
                {
                    parameters.Add(this.UserModesWithParams);
                    if (!string.IsNullOrEmpty(this.ServerModes))
                    {
                        parameters.Add(this.ServerModes);
                        if (!string.IsNullOrEmpty(this.ServerModesWithParams))
                        {
                            parameters.Add(this.ServerModesWithParams);
                        }
                    }
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

            this.ServerName = parameters[1];
            this.Version = parameters[2];
            this.UserModes = parameters[3];
            this.ChannelModes = parameters[4];

            int pCount = parameters.Count;

            if (pCount > 5)
            {
                this.ChannelModesWithParams = parameters[5];
                if (pCount > 6)
                {
                    this.UserModesWithParams = parameters[6];

                    if (pCount > 7)
                    {
                        this.ServerModes = parameters[7];

                        if (pCount > 8)
                        {
                            this.ServerModesWithParams = parameters[8];
                        }
                    }
                }
            }
        }

        /// <summary>
        ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
        /// </summary>
        public override void Notify(MessageConduit conduit)
        {
            conduit.OnServerInfo(new IrcMessageEventArgs<ServerInfoMessage>(this));
        }
    }
}
