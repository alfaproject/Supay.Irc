using System;
using System.Collections.ObjectModel;

namespace Supay.Irc
{
  /// <summary>
  ///   A collection that stores <see cref="Client" /> objects.
  /// </summary>
  /// <seealso cref="Client" />
  [Serializable]
  public class ClientCollection : ObservableCollection<Client>
  {
  }
}
