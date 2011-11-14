using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Reply for the <see cref="UserHostMessage" /> to list replies to the query list.
  /// </summary>
  [Serializable]
  public class UserHostReplyMessage : NumericMessage
  {
    private readonly UserCollection replies = new UserCollection();

    /// <summary>
    ///   Creates a new instance of the <see cref="UserHostReplyMessage" /> class.
    /// </summary>
    public UserHostReplyMessage()
      : base(302)
    {
    }

    /// <summary>
    ///   Gets the list of replies in the message.
    /// </summary>
    public virtual UserCollection Users
    {
      get
      {
        return this.replies;
      }
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(MessageUtil.CreateList(this.Users, " ", user => {
        string result = user.Nickname;
        if (user.IrcOperator)
        {
          result += "*";
        }
        result += "=";
        result += user.Away ? "+" : "-";
        result += user.Username;
        result += "@";
        result += user.Host;
        return result;
      }));
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Users.Clear();
      var userInfo = parameters[parameters.Count - 1].Split(' ');
      foreach (string info in userInfo)
      {
        string nick = info.Substring(0, info.IndexOf("=", StringComparison.Ordinal));
        bool oper = false;
        if (nick.EndsWith("*", StringComparison.Ordinal))
        {
          oper = true;
          nick = nick.Substring(0, nick.Length - 1);
        }
        string away = info.Substring(info.IndexOf("=", StringComparison.Ordinal) + 1, 1);
        string standardHost = info.Substring(info.IndexOf(away, StringComparison.Ordinal));

        User user = new User(standardHost) {
          Nickname = nick,
          IrcOperator = oper,
          Away = away == "+"
        };

        this.Users.Add(user);
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnUserHostReply(new IrcMessageEventArgs<UserHostReplyMessage>(this));
    }
  }
}
