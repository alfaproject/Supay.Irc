using System;

namespace Supay.Irc.Network
{
    /// <summary>
    /// Provides data for connection events that carry a data payload.
    /// </summary>
    public class ConnectionDataEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionDataEventArgs"/> class with the given data.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> <see cref="string"/> received by the <see cref="ClientConnection"/>.</param>
        public ConnectionDataEventArgs(string data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the <see cref="Data"/> received by the the <see cref="ClientConnection"/>.
        /// </summary>
        public string Data
        {
            get;
            private set;
        }
    }
}
