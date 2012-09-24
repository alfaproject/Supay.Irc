using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Supay.Irc.Properties;

namespace Supay.Irc.Messages
{
    /// <summary>
    /// Provides clients with a correct <see cref="IrcMessage" /> for a given raw message string.
    /// </summary>
    public sealed class IrcMessageFactory
    {
        private static readonly Lazy<IrcMessageFactory> instance = new Lazy<IrcMessageFactory>(() => new IrcMessageFactory());

        private const int MIN_MESSAGE_LENGTH = 1;
        private const int MAX_MESSAGE_LENGTH = 512;

        private readonly LinkedList<IrcMessage> _commands;
        private readonly LinkedList<IrcMessage> _numerics;
        private readonly LinkedList<IrcMessage> _ctcps;
        private readonly LinkedList<IrcMessage> _customs;

        /// <summary>
        /// Initializes a new instance of the <see cref="IrcMessageFactory" /> class.
        /// </summary>
        private IrcMessageFactory()
        {
            this._commands = new LinkedList<IrcMessage>();
            this._numerics = new LinkedList<IrcMessage>();
            this._ctcps = new LinkedList<IrcMessage>();
            this._customs = new LinkedList<IrcMessage>();

            var ircMessageType = typeof(IrcMessage);
            var ircMessages = from type in ircMessageType.Assembly.GetTypes()
                              where !type.IsAbstract && type != typeof(GenericNumericMessage) && type != typeof(GenericErrorMessage) && type != typeof(GenericCtcpRequestMessage) && type != typeof(GenericCtcpReplyMessage) && type.IsSubclassOf(ircMessageType)
                              select (IrcMessage) Activator.CreateInstance(type);
            foreach (var msg in ircMessages)
            {
                if (msg.GetType().IsSubclassOf(typeof(CommandMessage)))
                {
                    this._commands.AddLast(msg);
                }
                else if (msg.GetType().IsSubclassOf(typeof(NumericMessage)))
                {
                    this._numerics.AddLast(msg);
                }
                else if (msg.GetType().IsSubclassOf(typeof(CtcpMessage)))
                {
                    this._ctcps.AddLast(msg);
                }
            }
        }

        /// <summary>
        /// Adds a custom message to consider for parsing raw messages received from the server.
        /// </summary>
        public static void AddCustomMessage(IrcMessage msg)
        {
            instance.Value._customs.AddLast(msg);
        }

        /// <summary>
        /// Parses the given string into an <see cref="IrcMessage" />.
        /// </summary>
        /// <param name="unparsedMessage">The string to parse.</param>
        /// <returns>An <see cref="IrcMessage" /> which represents the given string.</returns>
        public static IrcMessage Parse(string unparsedMessage)
        {
            Debug.Assert(unparsedMessage != null);

            if (unparsedMessage.Length < MIN_MESSAGE_LENGTH || MAX_MESSAGE_LENGTH < unparsedMessage.Length)
            {
                string errorMessage = string.Format(CultureInfo.InvariantCulture, Resources.MessageEmptyOrTooLong, unparsedMessage.Length);
                throw new InvalidMessageException(errorMessage, unparsedMessage);
            }

            try
            {
                IrcMessage msg = instance.Value.DetermineMessage(unparsedMessage);
                msg.Parse(unparsedMessage);
                return msg;
            }
            catch (InvalidMessageException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidMessageException(Resources.CouldNotParseMessage, unparsedMessage, ex);
            }
        }

        /// <summary>
        /// Determines and instantiates the correct subclass of <see cref="IrcMessage" /> for the given string.
        /// </summary>
        private IrcMessage DetermineMessage(string unparsedMessage)
        {
            IrcMessage msg;

            if (this._customs.Count != 0)
            {
                msg = GetMessage(unparsedMessage, this._customs);
                if (msg != null)
                {
                    return msg;
                }
            }

            string command = MessageUtil.GetCommand(unparsedMessage);
            if (char.IsDigit(command[0]))
            {
                msg = GetMessage(unparsedMessage, this._numerics);
                if (msg == null)
                {
                    int numeric = int.Parse(command, CultureInfo.InvariantCulture);
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
                    msg = GetMessage(unparsedMessage, this._ctcps);
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
                    msg = GetMessage(unparsedMessage, this._commands) ?? new GenericMessage();
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
