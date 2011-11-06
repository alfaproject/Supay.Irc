using System.Collections.ObjectModel;
using System.Linq;

namespace Supay.Irc {
  /// <summary>
  ///   A collection of <see href = "Query" /> objects.
  /// </summary>
  public class QueryCollection : ObservableCollection<Query> {
    /// <summary>
    ///   Finds the <see href = "Query" /> instance within the collection which is with the given user.
    /// </summary>
    /// <returns>The found query, or null.</returns>
    public Query FindQuery(User user) {
      return this.FirstOrDefault(q => q.User == user || q.User.IsMatch(user));
    }

    /// <summary>
    ///   Either finds or creates a <see href = "Query" /> instance for the given <see href = "User" /> on the given <see href = "Client" />.
    /// </summary>
    public Query EnsureQuery(User user, Client client) {
      Query q = FindQuery(user);
      if (q == null) {
        q = new Query(client, user);
        this.Add(q);
      }
      return q;
    }
  }
}
