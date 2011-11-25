using System;
using System.Collections.Generic;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   With the ChannelPropertyMessage, clients can read and write property values for IRCX enabled channels.
  /// </summary>
  /// <remarks>
  ///   <p>
  ///     To read all channel properties for a channel, simply set the <see cref="Channel" /> property.
  ///     To read a channel property, set  <see cref="Channel" /> and <see cref="Prop" />.
  ///     To write a channel property, set the <see cref="Channel" />, <see cref="Prop" />, and <see cref="NewValue" /> properties. When a server sets the property, the client will receive the same property message back.
  ///   </p>
  ///   <p>This command is only effective for an IRCX enabled server.</p>
  /// </remarks>
  [Serializable]
  public class ChannelPropertyMessage : CommandMessage
  {
    private string channel = string.Empty;
    private string newValue = string.Empty;
    private string property = string.Empty;

    /// <summary>
    ///   Gets the IRC command associated with this message.
    /// </summary>
    protected override string Command
    {
      get
      {
        return "PROP";
      }
    }

    /// <summary>
    ///   Gets or sets the channel being targeted.
    /// </summary>
    /// <remarks>
    ///   Some implementations allow for this to be the name of a server, but this is an extension.
    /// </remarks>
    public virtual string Channel
    {
      get
      {
        return this.channel;
      }
      set
      {
        this.channel = value;
      }
    }

    /// <summary>
    ///   Gets or sets the channel property being targeted.
    /// </summary>
    /// <remarks>
    ///   When this message is sent with an empty <see cref="Prop" />, the values of all current channel properties are sent from the server.
    /// </remarks>
    public virtual string Prop
    {
      get
      {
        return this.property;
      }
      set
      {
        this.property = value;
      }
    }

    /// <summary>
    ///   Gets or sets the value being applied to the target channel property.
    /// </summary>
    /// <remarks>
    ///   You can set the value of a channel property by specify its name in the <see cref="Prop" /> property, and the value in the <see cref="NewValue" /> property.
    /// </remarks>
    public virtual string NewValue
    {
      get
      {
        return this.newValue;
      }
      set
      {
        this.newValue = value;
      }
    }

    /// <summary>
    ///   Validates this message against the given server support
    /// </summary>
    public override void Validate(ServerSupport serverSupport)
    {
      base.Validate(serverSupport);
      this.Channel = MessageUtil.EnsureValidChannelName(this.Channel, serverSupport);
    }

    /// <summary>
    ///   Overrides <see cref="IrcMessage.GetParameters" />.
    /// </summary>
    protected override IList<string> GetParameters()
    {
      var parameters = base.GetParameters();
      parameters.Add(this.Channel);
      if (string.IsNullOrEmpty(this.Prop))
      {
        parameters.Add("*");
      }
      else
      {
        parameters.Add(this.Prop);
        if (!string.IsNullOrEmpty(this.NewValue))
        {
          parameters.Add(this.NewValue);
        }
      }
      return parameters;
    }

    /// <summary>
    ///   Parses the parameters portion of the message.
    /// </summary>
    protected override void ParseParameters(IList<string> parameters)
    {
      base.ParseParameters(parameters);

      this.Channel = string.Empty;
      this.Prop = string.Empty;
      this.NewValue = string.Empty;

      if (parameters.Count > 0)
      {
        this.Channel = parameters[0];
        if (parameters.Count > 1)
        {
          this.Prop = parameters[1];
          if (this.Prop == "*")
          {
            this.Prop = string.Empty;
          }
          if (parameters.Count > 2)
          {
            this.NewValue = parameters[2];
          }
        }
      }
    }

    /// <summary>
    ///   Notifies the given <see cref="MessageConduit" /> by raising the appropriate event for the current <see cref="IrcMessage" /> subclass.
    /// </summary>
    public override void Notify(MessageConduit conduit)
    {
      conduit.OnChannelProperty(new IrcMessageEventArgs<ChannelPropertyMessage>(this));
    }
  }
}
