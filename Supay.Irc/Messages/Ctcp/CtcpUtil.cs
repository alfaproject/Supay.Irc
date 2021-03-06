using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Utility class for Ctcp messages.
    /// </summary>
    /// <remarks>
    ///   This most likely doesn't need to be used from lib-user code.
    /// </remarks>
    public static class CtcpUtil
    {
        /// <summary>
        ///   The character used to indicate the start and end of an extended data section in a CTCP message.
        /// </summary>
        public const char EXTENDED_DATA_MARKER = '\x0001';

        /// <summary>
        ///   Escapes the given text for use in a ctcp message.
        /// </summary>
        public static string Escape(string text)
        {
            string escaper = '\x0014'.ToString(CultureInfo.InvariantCulture);
            string nul = '\x0000'.ToString(CultureInfo.InvariantCulture);
            string result = text;
            result = Regex.Replace(result, escaper, escaper + escaper);
            result = Regex.Replace(result, nul, escaper + "0");
            result = Regex.Replace(result, "\r", escaper + "r");
            result = Regex.Replace(result, "\n", escaper + "n");
            return result;
        }

        /// <summary>
        ///   Unescapes the given text for use outside a ctcp message.
        /// </summary>
        public static string Unescape(string text)
        {
            string escaper = '\x0014'.ToString(CultureInfo.InvariantCulture);
            string nul = '\x0000'.ToString(CultureInfo.InvariantCulture);
            string result = text;

            result = Regex.Replace(result, escaper + "0", nul);
            result = Regex.Replace(result, escaper + "r", "\r");
            result = Regex.Replace(result, escaper + "n", "\n");
            result = Regex.Replace(result, escaper + escaper, escaper);

            return result;
        }

        /// <summary>
        ///   Extracts the TransportCommand from the given message string.
        /// </summary>
        /// <remarks>
        ///   A Ctcp TransportCommand is the irc command used to send the message to another user,
        ///   and is not to be confused with the Ctcp Command.
        ///   It is always either PRIVMSG or NOTICE for a valid ctcp command.
        /// </remarks>
        public static string GetTransportCommand(string rawMessage)
        {
            return MessageUtil.GetCommand(rawMessage);
        }

        /// <summary>
        ///   Extracts the actual Ctcp command from the given message string.
        /// </summary>
        public static string GetInternalCommand(string rawMessage)
        {
            var ctcpMessage = MessageUtil.GetParameters(rawMessage).Last();
            var indexOfSpace = ctcpMessage.IndexOf(' ');
            return ctcpMessage.Substring(1, indexOfSpace == -1 ? ctcpMessage.Length - 2 : indexOfSpace - 1);
        }

        /// <summary>
        ///   Extracts the extended data section of a Ctcp message.
        /// </summary>
        public static string GetExtendedData(string rawMessage)
        {
            var ctcpMessage = MessageUtil.GetParameters(rawMessage).Last();
            var indexOfSpace = ctcpMessage.IndexOf(' ');
            return Unescape(indexOfSpace == -1 ? string.Empty : ctcpMessage.Substring(indexOfSpace + 1, ctcpMessage.Length - indexOfSpace - 2));
        }

        /// <summary>
        ///   Determines if the given message is a Ctcp message.
        /// </summary>
        public static bool IsCtcpMessage(string rawMessage)
        {
            IList<string> p = MessageUtil.GetParameters(rawMessage);
            if (p.Count != 2)
            {
                return false;
            }
            string payLoad = p[1];
            return payLoad.StartsWith(EXTENDED_DATA_MARKER.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal) && payLoad.EndsWith(EXTENDED_DATA_MARKER.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
        }

        /// <summary>
        ///   Determines if the given ctcp message is a request message.
        /// </summary>
        public static bool IsRequestMessage(string rawMessage)
        {
            return GetTransportCommand(rawMessage) == "PRIVMSG";
        }

        /// <summary>
        ///   Determines if the given ctcp message is a reply message.
        /// </summary>
        public static bool IsReplyMessage(string rawMessage)
        {
            return GetTransportCommand(rawMessage) == "NOTICE";
        }
    }
}
