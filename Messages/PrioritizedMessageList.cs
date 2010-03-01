using System.Collections.Generic;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   A list which contains IrcMessages. </summary>
  /// <remarks>
  ///   Call <see cref="m:Prioritize"/> on frequently accessed nodes
  ///   in order to make finding common messages faster. </remarks>
  public class PrioritizedMessageList : LinkedList<IrcMessage> {

    /// <summary>
    ///   Creates a new instance of the <see cref="PrioritizedMessageList"/> class. </summary>
    public PrioritizedMessageList()
      : base() {
    }

    /// <summary>
    ///   Moves the given <see cref="LinkedListNode&lt;IrcMessage&gt;"/> to the front of an enumeration of the set. </summary>
    /// <param name="node">
    ///   The <see cref="LinkedListNode&lt;IrcMessage&gt;"/> to prioritize. </param>
    public void Prioritize(LinkedListNode<IrcMessage> node) {
      if (node != null && node != this.First) {
        this.Remove(node);
        this.AddFirst(node);
      }
    }

  } //class PrioritizedMessageList
} //namespace Supay.Irc.Messages