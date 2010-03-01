using System;
using System.Collections.Generic;
using System.Globalization;

namespace Supay.Irc.Messages {

  /// <summary>
  ///   Provides clients with a correct <see cref="IrcMessage"/> for a given raw message string. </summary>
  public sealed class MessageParserService {

    private static readonly MessageParserService _service = new MessageParserService();

    private const int MinMessageLength = 1;
    private const int MaxMessageLength = 512;

    private PrioritizedMessageList _numerics;
    private PrioritizedMessageList _commands;
    private PrioritizedMessageList _ctcps;
    private PrioritizedMessageList _customs;

    /// <summary>
    ///   Explicit static constructor to tell C# compiler not to mark type as 'beforefieldinit'. </summary>
    static MessageParserService() {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="MessageParserService"/> class. </summary>
    /// <remarks>
    ///   This is private because this class is a Singleton.
    ///   Use the <see cref="Service"/> to get the only instance of this class. </remarks>
    private MessageParserService() {
      _numerics = new PrioritizedMessageList();
      _commands = new PrioritizedMessageList();
      _ctcps = new PrioritizedMessageList();

      foreach (Type type in this.GetType().Assembly.GetTypes()) {
        if (type.IsSubclassOf(typeof(IrcMessage))) {
          if (type.IsSubclassOf(typeof(CommandMessage))) {
            if (!type.IsAbstract) {
              _commands.AddLast((IrcMessage)Activator.CreateInstance(type));
            }
          } else if (type.IsSubclassOf(typeof(NumericMessage))) {
            if (!type.IsAbstract && type != typeof(GenericNumericMessage) && type != typeof(GenericErrorMessage)) {
              _numerics.AddLast((IrcMessage)Activator.CreateInstance(type));
            }
          } else if (type.IsSubclassOf(typeof(CtcpMessage))) {
            if (!type.IsAbstract && type != typeof(GenericCtcpRequestMessage) && type != typeof(GenericCtcpReplyMessage)) {
              _ctcps.AddLast((IrcMessage)Activator.CreateInstance(type));
            }
          }
        }
      }
    }

    /// <summary>
    ///   Provides access to clients to lone instance of the <see cref="MessageParserService"/>. </summary>
    /// <returns>
    ///   The Singleton-patterned service. </returns>
    public static MessageParserService Service {
      get {
        return _service;
      }
    }

    /// <summary>
    ///   Adds a custom message to consider for parsing raw messages received from the server. </summary>
    public void AddCustomMessage(IrcMessage msg) {
      if (_customs == null) {
        _customs = new PrioritizedMessageList();
      }
      _customs.AddLast(msg);
    }

    /// <summary>
    ///   Parses the given string into an <see cref="IrcMessage"/>. </summary>
    /// <param name="unparsedMessage">
    ///   The string to parse. </param>
    /// <returns>
    ///   An <see cref="IrcMessage"/> which represents the given string. </returns>
    public IrcMessage Parse(string unparsedMessage) {
      if (unparsedMessage == null) {
        unparsedMessage = string.Empty;
      }
      if (unparsedMessage.Length < MinMessageLength || MaxMessageLength < unparsedMessage.Length) {
        string errorMessage = string.Format(CultureInfo.InvariantCulture, Properties.Resources.MessageEmptyOrTooLong, unparsedMessage.Length);
        throw new InvalidMessageException(errorMessage, unparsedMessage);
      }

      IrcMessage msg = null;
      try {
        msg = this.DetermineMessage(unparsedMessage);
        msg.Parse(unparsedMessage);
      } catch (InvalidMessageException) {
        throw;
      } catch (Exception ex) {
        throw new InvalidMessageException(Properties.Resources.CouldNotParseMessage, unparsedMessage, ex);
      }
      return msg;
    }

    /// <summary>
    ///   Determines and instantiates the correct subclass of <see cref="IrcMessage"/> for the given string. </summary>
    private IrcMessage DetermineMessage(string unparsedMessage) {
      IrcMessage msg = null;

      if (_customs != null) {
        msg = GetMessage(unparsedMessage, _customs);
        if (msg != null) {
          return msg;
        }
      }

      string command = MessageUtil.GetCommand(unparsedMessage);
      if (Char.IsDigit(command[0])) {
        msg = GetMessage(unparsedMessage, _numerics);
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
          msg = GetMessage(unparsedMessage, _ctcps);
          if (msg == null) {
            if (CtcpUtil.IsRequestMessage(unparsedMessage)) {
              msg = new GenericCtcpRequestMessage();
            } else {
              msg = new GenericCtcpReplyMessage();
            }
          }
        } else {
          msg = GetMessage(unparsedMessage, _commands);
          if (msg == null) {
            msg = new GenericMessage();
          }
        }
      }
      return msg;
    }

    private static IrcMessage GetMessage(string unparsedMessage, PrioritizedMessageList potentialHandlers) {
      IrcMessage handler = null;
      LinkedListNode<IrcMessage> nodeToPrioritize = null;

      LinkedListNode<IrcMessage> node = potentialHandlers.First;
      if (node != null) {
        do {
          IrcMessage msg = node.Value;

          try {
            if (msg.CanParse(unparsedMessage)) {
              nodeToPrioritize = node;
              handler = msg.CreateInstance();
              break;
            }
            node = node.Next;
          } catch {
            System.Diagnostics.Trace.WriteLine("Error testing CanParse on { " + unparsedMessage + " }", "Parse Error");
            throw;
          }
        }
        while (node != null && node.Next != potentialHandlers.First);
      }

      if (nodeToPrioritize != null) {
        potentialHandlers.Prioritize(nodeToPrioritize);
      }

      return handler;
    }

  } //class MessageParserService
} //namespace Supay.Irc.Messages