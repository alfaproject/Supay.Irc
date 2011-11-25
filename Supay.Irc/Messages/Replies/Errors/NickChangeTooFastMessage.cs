using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> sent when a user tries to change his nick too many times too quickly.
  /// </summary>
  [Serializable]
  public class NickChangeTooFastMessage : ErrorMessage
  {
    private string nick;
    private int seconds;

    /// <summary>
    ///   Creates a new instances of the <see cref="NickChangeTooFastMessage" /> class.
    /// </summary>
    public NickChangeTooFastMessage()
      : base(438)
    {
    }

    /// <summary>
    ///   Gets or sets the Nick which was attempted
    /// </summary>
    public string Nick
    {
      get
      {
        return this.nick;
      }
      set
      {
        this.nick = value;
      }
    }

    /// <summary>
    ///   Gets or sets the number of seconds which must be waited before attempting again.
    /// </summary>
    public int Seconds
    {
      get
      {
        return this.seconds;
      }
      set
      {
        this.seconds = value;
      }
    }

    /// <exclude />
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Nick);
      parameters.Add(string.Format(CultureInfo.InvariantCulture, "Nick change too fast. Please wait {0} seconds.", this.Seconds));
      return parameters;
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.Nick = string.Empty;
      this.Seconds = -1;
      if (parameters.Count > 1)
      {
        this.Nick = parameters[1];
        if (parameters.Count > 2)
        {
          this.Seconds = Convert.ToInt32(MessageUtil.StringBetweenStrings(parameters[2], "Please wait ", " seconds"), CultureInfo.InvariantCulture);
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnNickChangeTooFast(new IrcMessageEventArgs<NickChangeTooFastMessage>(this));
    }
  }
}
