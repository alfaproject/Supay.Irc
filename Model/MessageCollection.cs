using System;
using System.Collections.ObjectModel;
using Supay.Irc.Messages;

namespace Supay.Irc {

  /// <summary>
  ///     <para>
  ///       A collection that stores <see cref='Supay.Irc.Messages.IrcMessage'/> objects.
  ///    </para>
  /// </summary>
  [Serializable]
  public class MessageCollection : ObservableCollection<IrcMessage> {

  }

}