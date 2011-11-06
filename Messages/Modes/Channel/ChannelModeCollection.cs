using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages.Modes {
  /// <summary>
  ///   A collection that stores <see cref="ChannelMode" /> objects.
  /// </summary>
  [Serializable]
  public class ChannelModeCollection : ObservableCollection<ChannelMode> {
    /// <summary>
    ///   Clears the current collection and adds the given modes.
    /// </summary>
    public void ResetWith(IEnumerable<ChannelMode> newModes) {
      Clear();
      foreach (ChannelMode mode in newModes) {
        Add(mode);
      }
    }
  }
}
