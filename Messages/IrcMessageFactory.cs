using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages {
  /// <summary>
  ///   Provides clients with a correct <see cref="IrcMessage" /> for a given raw message string.
  /// </summary>
  public sealed class IrcMessageFactory {
    private static readonly IrcMessageFactory _factory = new IrcMessageFactory();

    private const int MIN_MESSAGE_LENGTH = 1;
    private const int MAX_MESSAGE_LENGTH = 512;

    private readonly LinkedList<IrcMessage> _numerics;
    private readonly LinkedList<IrcMessage> _commands;
    private readonly LinkedList<IrcMessage> _ctcps;
    private readonly LinkedList<IrcMessage> _customs;

    /// <summary>
    ///   Initializes a new instance of the <see cref="IrcMessageFactory" /> class.
    /// </summary>
    /// <remarks>
    ///   This is private because this class is a Singleton.
    /// </remarks>
    private IrcMessageFactory() {
      _numerics = new LinkedList<IrcMessage>();
      _commands = new LinkedList<IrcMessage>();
      _ctcps = new LinkedList<IrcMessage>();
      _customs = new LinkedList<IrcMessage>();

      Type ircMessageType = typeof(IrcMessage);
      foreach (Type type in ircMessageType.Assembly.GetTypes().Where(t => t.IsSubclassOf(ircMessageType))) {
        if (type.IsSubclassOf(typeof(CommandMessage))) {
          if (!type.IsAbstract) {
            _commands.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        } else if (type.IsSubclassOf(typeof(NumericMessage))) {
          if (!type.IsAbstract && type != typeof(GenericNumericMessage) && type != typeof(GenericErrorMessage)) {
            _numerics.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        } else if (type.IsSubclassOf(typeof(CtcpMessage))) {
          if (!type.IsAbstract && type != typeof(GenericCtcpRequestMessage) && type != typeof(GenericCtcpReplyMessage)) {
            _ctcps.AddLast((IrcMessage) Activator.CreateInstance(type));
          }
        }
      }
    }

    /// <summary>
    ///   Adds a custom message to consider for parsing raw messages received from the server.
    /// </summary>
    public static void AddCustomMessage(IrcMessage msg) {
      _factory._customs.AddLast(msg);
    }

    /// <summary>
    ///   Parses the given string into an <see cref="IrcMessage" />.
    /// </summary>
    /// <param name="unparsedMessage">The string to parse.</param>
    /// <returns>An <see cref="IrcMessage" /> which represents the given string.</returns>
    public static IrcMessage Parse(string unparsedMessage) {
      if (unparsedMessage == null) {
        unparsedMessage = string.Empty;
      }
      if (unparsedMessage.Length < MIN_MESSAGE_LENGTH || MAX_MESSAGE_LENGTH < unparsedMessage.Length) {
        string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.MessageEmptyOrTooLong, unparsedMessage.Length);
        throw new InvalidMessageException(errorMessage, unparsedMessage);
      }

      IrcMessage msg;
      try {
        msg = _factory.determineMessage(unparsedMessage);
        msg.Parse(unparsedMessage);
      } catch (InvalidMessageException) {
        throw;
      } catch (Exception ex) {
        throw new InvalidMessageException(Resources.CouldNotParseMessage, unparsedMessage, ex);
      }
      return msg;
    }

    /// <summary>
    ///   Determines and instantiates the correct subclass of <see cref="IrcMessage" /> for the
    ///   given string.
    /// </summary>
    private IrcMessage determineMessage(string unparsedMessage) {
      IrcMessage msg;

      if (_customs.Count != 0) {
        msg = getMessage(unparsedMessage, _customs);
        if (msg != null) {
          return msg;
        }
      }

      string command = MessageUtil.GetCommand(unparsedMessage);
      if (Char.IsDigit(command[0])) {
        msg = getMessage(unparsedMessage, _numerics);
        if (msg == null) {
          int numeric = Convert.ToInt32(command, CultureInfo.InvariantCulture);
          if (NumericMessage.IsError(numeric)) {
            msg = new GenericErrorMessage();
          } else {
            msg = new GenericNumericMessage();
          }
        }
      } else {
        if (CtcpUtil.IsCtcpMessage(unparsedMessage)) {
          msg = getMessage(unparsedMessage, _ctcps);
          if (msg == null) {
            if (CtcpUtil.IsRequestMessage(unparsedMessage)) {
              msg = new GenericCtcpRequestMessage();
            } else {
              msg = new GenericCtcpReplyMessage();
            }
          }
        } else {
          msg = getMessage(unparsedMessage, _commands);
          if (msg == null) {
            msg = new GenericMessage();
          }
        }
      }

      return msg;
    }

    private static IrcMessage getMessage(string unparsedMessage, LinkedList<IrcMessage> potentialHandlers) {
      IrcMessage msg = potentialHandlers.FirstOrDefault(m => m.CanParse(unparsedMessage));
      if (msg != null) {
        // prioritize this IrcMessage by moving it to the beginning of the list
        if (msg != potentialHandlers.First.Value) {
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
