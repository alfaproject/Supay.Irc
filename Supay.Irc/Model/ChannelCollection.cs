using System;
using System.Collections.ObjectModel;

namespace Supay.Irc
{
    /// <summary>
    /// A collection that stores <see cref="Channel" /> objects.
    /// </summary>
    [Serializable]
    public class ChannelCollection : ObservableDictionary<string, Channel>
    {
        public ChannelCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Either finds or creates the channel by the given name.
        /// </summary>
        public Channel EnsureChannel(string name)
        {
            Channel channel;
            if (!this.TryGetValue(name, out channel))
            {
                channel = new Channel(name);
                this.Add(name, channel);
            }
            return channel;
        }
    }
}
