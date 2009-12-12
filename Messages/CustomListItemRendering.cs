using System;

namespace Supay.Irc.Messages {

  /// <summary>
  /// A delegate which provides custom format rendering for the items in a list.
  /// </summary>
  public delegate String CustomListItemRendering<T>(T item);

}