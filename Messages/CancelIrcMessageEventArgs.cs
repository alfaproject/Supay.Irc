using System;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The information for a handler of an <see cref="IrcMessage" /> event which can be cancelled.
  /// </summary>
  [Serializable]
  public class CancelIrcMessageEventArgs<T> : IrcMessageEventArgs<T> where T : IrcMessage
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="IrcMessageEventArgs&lt;T&gt;" /> class with
    ///   the given <see cref="IrcMessage" />.
    /// </summary>
    public CancelIrcMessageEventArgs(T msg)
      : base(msg)
    {
    }

    /// <summary>
    ///   Gets or sets a value indicating whether the event should be cancelled.
    /// </summary>
    public bool Cancel
    {
      get;
      set;
    }
  }
}
