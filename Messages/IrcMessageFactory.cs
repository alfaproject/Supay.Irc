using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages
{
  /// <summary>
  ///   Provides clients with a correct <see cref="IrcMessage" /> for a given raw message string.
  /// </summary>
  public sealed class IrcMessageFactory
  {
    private const int MIN_MESSAGE_LENGTH = 1;
    private const int MAX_MESSAGE_LENGTH = 512;
    private static readonly IrcMessageFactory factory = new IrcMessageFactory();

    private readonly LinkedList<IrcMessage> commands;
    private readonly LinkedList<IrcMessage> ctcps;
    private readonly LinkedList<IrcMessage> customs;
    private readonly LinkedList<IrcMessage> numerics;

    /// <summary>
    ///   Initializes a new instance of the <see cref="IrcMessageFactory" /> class.
    /// </summary>
    /// <remarks>
    ///   This is private because this class is a Singleton.
    /// </remarks>
    private IrcMessageFactory()
    {
      this.numerics = new LinkedList<IrcMessage>();
      this.commands = new LinkedList<IrcMessage>();
      this.ctcps = new LinkedList<IrcMessage>();
      this.customs = new LinkedList<IrcMessage>();

      Type ircMessageType = typeof(IrcMessage);
      foreach (Type type in ircMessageType.Assembly.GetTypes().Where(t => t.IsSubclassOf(ircMessageType)))
      {
        if (type.IsSubclassOf(typeof(CommandMessage)))
        {
          if (!type.IsAbstract)
          {
            this.commands.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        }
        else if (type.IsSubclassOf(typeof(NumericMessage)))
        {
          if (!type.IsAbstract && type != typeof(GenericNumericMessage) && type != typeof(GenericErrorMessage))
          {
            this.numerics.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        }
        else if (type.IsSubclassOf(typeof(CtcpMessage)))
        {
          if (!type.IsAbstract && type != typeof(GenericCtcpRequestMessage) && type != typeof(GenericCtcpReplyMessage))
          {
            this.ctcps.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        }
      }
    }

    /// <summary>
    ///   Adds a custom message to consider for parsing raw messages received from the server.
    /// </summary>
    public static void AddCustomMessage(IrcMessage msg)
    {
      factory.customs.AddLast(msg);
    }

    /// <summary>
    ///   Parses the given string into an <see cref="IrcMessage" />.
    /// </summary>
    /// <param name="unparsedMessage">The string to parse.</param>
    /// <returns>An <see cref="IrcMessage" /> which represents the given string.</returns>
    public static IrcMessage Parse(string unparsedMessage)
    {
      if (unparsedMessage == null)
      {
        unparsedMessage = string.Empty;
      }
      if (unparsedMessage.Length < MIN_MESSAGE_LENGTH || MAX_MESSAGE_LENGTH < unparsedMessage.Length)
      {
        string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.MessageEmptyOrTooLong, unparsedMessage.Length);
        throw new InvalidMessageException(errorMessage, unparsedMessage);
      }

      IrcMessage msg;
      try
      {
        msg = factory.DetermineMessage(unparsedMessage);
        msg.Parse(unparsedMessage);
      }
      catch (InvalidMessageException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new InvalidMessageException(Resources.CouldNotParseMessage, unparsedMessage, ex);
      }
      return msg;
    }

    /// <summary>
    ///   Determines and instantiates the correct subclass of <see cref="IrcMessage" /> for the
    ///   given string.
    /// </summary>
    private IrcMessage DetermineMessage(string unparsedMessage)
    {
      IrcMessage msg;

      if (this.customs.Count != 0)
      {
        msg = GetMessage(unparsedMessage, this.customs);
        if (msg != null)
        {
          return msg;
        }
      }

      string command = MessageUtil.GetCommand(unparsedMessage);
      if (char.IsDigit(command[0]))
      {
        msg = GetMessage(unparsedMessage, this.numerics);
        if (msg == null)
        {
          int numeric = Convert.ToInt32(command, CultureInfo.InvariantCulture);
          if (NumericMessage.IsError(numeric))
          {
            msg = new GenericErrorMessage();
          }
          else
          {
            msg = new GenericNumericMessage();
          }
        }
      }
      else
      {
        if (CtcpUtil.IsCtcpMessage(unparsedMessage))
        {
          msg = GetMessage(unparsedMessage, this.ctcps);
          if (msg == null)
          {
            if (CtcpUtil.IsRequestMessage(unparsedMessage))
            {
              msg = new GenericCtcpRequestMessage();
            }
            else
            {
              msg = new GenericCtcpReplyMessage();
            }
          }
        }
        else
        {
          msg = GetMessage(unparsedMessage, this.commands) ?? new GenericMessage();
        }
      }

      return msg;
    }

    private static IrcMessage GetMessage(string unparsedMessage, LinkedList<IrcMessage> potentialHandlers)
    {
      IrcMessage msg = potentialHandlers.FirstOrDefault(m => m.CanParse(unparsedMessage));
      if (msg != null)
      {
        // prioritize this IrcMessage by moving it to the beginning of the list
        if (msg != potentialHandlers.First.Value)
        {
          potentialHandlers.Remove(msg);
          potentialHandlers.AddFirst(msg);
        }

        // return a new instance of this message
        return (IrcMessage) Activator.CreateInstance(msg.GetType());
      }

      return null;
    }
  }
}
