using System;
using System.Collections.ObjectModel;

namespace Supay.Irc {

  /// <summary>
  ///   A collection that stores <see cref="User"/> objects. </summary>
  [Serializable]
  public class UserCollection : ObservableCollection<User> {

    /// <summary>
    ///   Removes the first User in the collection which is matched by the Predicate. </summary>
    /// <returns>
    ///   True if a User was removed, false if no User was removed. </returns>
    public bool RemoveFirst(Predicate<User> match) {
      for (int i = 0; i < this.Count; i++) {
        if (match(this[i])) {
          this.RemoveAt(i);
          return true;
        }
      }
      return false;
    }

    /// <summary>
    ///   Removes the first User in the collection which has the given nick. </summary>
    public bool RemoveFirst(string nick) {
      return this.RemoveFirst(u => u.Nickname.Equals(nick, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///   Finds the first User in the collection which matches the given Predicate. </summary>
    public User Find(Predicate<User> match) {
      for (int i = 0; i < this.Count; i++) {
        if (match(this[i])) {
          return this[i];
        }
      }
      return null;
    }

    /// <summary>
    ///   Finds the first User in the collection which matches the given nick. </summary>
    public User Find(string nick) {
      return this.Find(u => u.Nickname.Equals(nick, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    ///   Ensures that the collection has a User with the given nick. </summary>
    /// <remarks>
    ///   If no User has the given nick, then a new User is created with the nick, and is added to the collection. </remarks>
    /// <param name="nick">
    ///   The nick to ensure. </param>
    /// <returns>
    ///   The User in the collection with the given nick. </returns>
    public User EnsureUser(string nick) {
      User user = this.Find(nick);
      if (user == null) {
        user = new User(nick);
        this.Add(user);
      }
      return user;
    }

    /// <summary>
    ///   Ensures that the collection has a User which matches the nick of the given User. </summary>
    /// <remarks>
    ///   If no User matches the given User, then the given User is added to the collection.
    ///   If a User is found, then the existing User is merged with the given User. </remarks>
    /// <returns>
    ///   The User in the collection which matches the given User. </returns>
    public User EnsureUser(User newUser) {
      User user = this.Find(newUser.Nickname);
      if (user == null) {
        user = newUser;
        this.Add(user);
      } else {
        user.CopyFrom(newUser);
      }
      return user;
    }

  } //class UserCollection
} //namespace Supay.Irc