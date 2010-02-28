using System;
using System.Collections.ObjectModel;

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
      foreach (Channel channel in this) {
        if (channel.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase)) {
          return channel;
        }
      }
      return null;
    }

    /// <summary>
    ///   Either finds or creates the channel by the given name. </summary>
    public Channel EnsureChannel(string name, Client client) {
      Channel channel = this.Find(name);
      if (channel == null) {
        channel = new Channel(name);
        this.Add(channel);
      }
      return channel;
    }

  } //class ChannelCollection
} //namespace Supay.Irc