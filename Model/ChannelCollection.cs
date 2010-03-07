using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supay.Irc {

  /// <summary>
  ///   A collection that stores <see cref="Channel"/> objects. </summary>
  [Serializable]
  public class ChannelCollection : ObservableCollection<Channel> {

    /// <summary>
    ///   Finds the <see href="Channel"/> in the collection with the given name. </summary>
    /// <returns>
    ///   The so-named channel, or null. </returns>
    public Channel Find(string channelName) {
      return this.FirstOrDefault(channel => channel.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///   Either finds or creates the channel by the given name. </summary>
    public Channel EnsureChannel(string name) {
      Channel channel = Find(name);
      if (channel == null) {
        channel = new Channel(name);
        Add(channel);
      }
      return channel;
    }

  } //class ChannelCollection
} //namespace Supay.Irc