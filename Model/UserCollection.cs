using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Supay.Irc
{
  /// <summary>
  ///   A collection that stores <see cref="User" /> objects.
  /// </summary>
  [Serializable]
  public class UserCollection : ObservableDictionary<string, User>
  {
    public UserCollection()
      : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public void Add(User user)
    {
      this.Add(user.Nickname, user);
    }

    public bool Contains(User user)
    {
      return this.ContainsKey(user.Nickname);
    }

    /// <summary>
    ///   Finds the first User in the collection which matches the given Predicate.
    /// </summary>
    public User Find(Func<User, bool> match)
    {
      return this.Values.FirstOrDefault(match);
    }

    /// <summary>
    ///   Finds the first User in the collection which matches the given nick.
    /// </summary>
    public User Find(string nick)
    {
      return this.Find(u => u.Nickname.Equals(nick, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///   Ensures that the collection has a User with the given nick.
    /// </summary>
    /// <remarks>
    ///   If no User has the given nick, then a new User is created with the nick, and is added to the collection.
    /// </remarks>
    /// <param name="nick">The nick to ensure.</param>
    /// <returns>The User in the collection with the given nick.</returns>
    public User EnsureUser(string nick)
    {
      User user = this.Find(nick);
      if (user == null)
      {
        user = new User(nick);
        this.Add(user.Nickname, user);
      }
      return user;
    }

    /// <summary>
    ///   Ensures that the collection has a User which matches the nick of the given User.
    /// </summary>
    /// <remarks>
    ///   If no User matches the given User, then the given User is added to the collection.
    ///   If a User is found, then the existing User is merged with the given User.
    /// </remarks>
    /// <returns>The User in the collection which matches the given User.</returns>
    public User EnsureUser(User newUser)
    {
      User user = this.Find(newUser.Nickname);
      if (user == null)
      {
        user = newUser;
        this.Add(user.Nickname, user);
      }
      else
      {
        user.CopyFrom(newUser);
      }
      return user;
    }
  }
}
