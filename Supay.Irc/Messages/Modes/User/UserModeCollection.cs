using System;
using System.Collections.ObjectModel;

namespace Supay.Irc.Messages.Modes
{
  /// <summary>
  ///   <para>
  ///     A collection that stores <see cref = 'Supay.Irc.Messages.Modes.UserMode' /> objects.
  ///   </para>
  /// </summary>
  /// <seealso cref = 'Supay.Irc.Messages.Modes.UserModeCollection' />
  [Serializable]
  public class UserModeCollection : ObservableCollection<UserMode>
  {
  }
}
