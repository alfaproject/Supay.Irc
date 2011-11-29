using System;
using System.Collections.Generic;
using Supay.Irc.Messages.Modes;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   The <see cref="ErrorMessage" /> received when a <see cref="ChannelModeMessage" /> was sent
  ///   with a <see cref="ChannelMode" /> which the server didn't recognize.
  /// </summary>
  [Serializable]
  public class UnknownChannelModeMessage : ErrorMessage
  {
    private string unknownMode;

    /// <summary>
    ///   Creates a new instances of the <see cref="UnknownChannelModeMessage" /> class.
    /// </summary>
    public UnknownChannelModeMessage()
      : base(472)
    {
    }

    /// <summary>
    ///   Gets or sets the mode which the server didn't recognize
    /// </summary>
    public string UnknownMode
    {
      get
      {
        return this.unknownMode;
      }
      set
      {
        this.unknownMode = value;
      }
    }

    /// <exclude />
    protected override IList<string> Tokens
    {
      get
      {
        var parameters = base.Tokens;
        parameters.Add(this.UnknownMode);
        parameters.Add("is unknown mode char to me");
        return parameters;
      }
    }

    /// <exclude />
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);
      this.UnknownMode = string.Empty;
      if (parameters.Count > 2)
      {
        this.UnknownMode = parameters[1];
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnUnknownChannelMode(new IrcMessageEventArgs<UnknownChannelModeMessage>(this));
    }
  }
}
