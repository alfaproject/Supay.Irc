using System;
using System.Collections.ObjectModel;
using System.Linq;

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
    /// Finds the <see href = "Channel" /> in the collection with the given name.
    /// </summary>
    /// <returns>The so-named channel, or null.</returns>
    public Channel Find(string name)
    {
      Channel channel;
      return this.TryGetValue(name, out channel) ? channel : null;
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
