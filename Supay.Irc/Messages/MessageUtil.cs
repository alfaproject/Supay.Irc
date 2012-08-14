using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace Supay.Irc.Messages
{
    /// <summary>
    ///   Provides simple utilities for parsing and generating messages.
    /// </summary>
    /// <remarks>
    ///   Client code will probably not need to use most of these routines.
    /// </remarks>
    public static class MessageUtil
    {
        private static string cachedRawMessage = string.Empty;
        private static Collection<string> cachedParams;

        /// <summary>
        ///   Takes the given channel name, and returns a name that is valid according to the given server support.
        /// </summary>
        /// <param name="channelName">The channel name to examine.</param>
        /// <param name="support">The feature support of an IRC server.</param>
        /// <returns>A valid channel name on the given server.</returns>
        public static string EnsureValidChannelName(string channelName, ServerSupport support)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                return "#irc";
            }
            if (support == null)
            {
                support = ServerSupport.DefaultSupport;
            }

            string result = channelName.Replace(' ', '_').Replace(',', '_').Replace(':', '_');
            char firstChar = result[0];
            if (firstChar != '#' && firstChar != '&' && firstChar != '+' && firstChar != '!')
            {
                result = '#' + result;
            }

            return result.Length > support.MaxChannelNameLength ? result.Substring(0, support.MaxChannelNameLength) : result;
        }

        /// <summary>
        ///   Determines if the given channel name has a valid namespace prefix.
        /// </summary>
        /// <remarks>
        ///   This is according to the IRC specification, and is not representative of what a
        ///   particular server may support.
        /// </remarks>
        public static bool HasValidChannelPrefix(string channelName)
        {
            return !string.IsNullOrEmpty(channelName) && (channelName[0] == '#' || channelName[0] == '&' || channelName[0] == '+' || channelName[0] == '!');
        }

        /// <summary>
        ///   Extracts the Prefix from a string message.
        /// </summary>
        public static string GetPrefix(string rawMessage)
        {
            if (!string.IsNullOrEmpty(rawMessage) && rawMessage[0] == ':')
            {
                return rawMessage.Substring(1, rawMessage.IndexOf(' ', 1) - 1);
            }
            return string.Empty;
        }

        /// <summary>
        ///   Extracts the Command from a string message.
        /// </summary>
        public static string GetCommand(string rawMessage)
        {
            if (string.IsNullOrEmpty(rawMessage))
            {
                return string.Empty;
            }

            // ignore prefix
            var indexOfCommand = 0;
            if (rawMessage[0] == ':')
            {
                indexOfCommand = rawMessage.IndexOf(' ', 1) + 1;
                if (indexOfCommand == 0)
                {
                    return string.Empty;
                }
            }

            // the first token is the command
            var indexOfParameters = rawMessage.IndexOf(' ', indexOfCommand);
            return rawMessage.Substring(indexOfCommand, (indexOfParameters == -1 ? rawMessage.Length : indexOfParameters) - indexOfCommand);
        }

        /// <summary>
        ///   Gets the parameters of the raw message.
        /// </summary>
        /// <param name="rawMessage">The message string which has the parameters.</param>
        public static Collection<string> GetParameters(string rawMessage)
        {
            if (string.IsNullOrEmpty(rawMessage))
            {
                return new Collection<string>();
            }

            int startIndex;
            if (rawMessage.StartsWith(":", StringComparison.Ordinal))
            {
                // then the params start after space 2
                startIndex = NthIndexOf(rawMessage, " ", 0, 2) + 1;
            }
            else
            {
                // then the params start after space 1
                startIndex = NthIndexOf(rawMessage, " ", 0, 1) + 1;
            }

            return startIndex == 0 ? new Collection<string>() : Tokenize(rawMessage, startIndex);
        }

        /// <summary>
        ///   Separates the given space-delimited parameter string into a collection.
        /// </summary>
        public static Collection<string> Tokenize(string rawMessage, int startIndex)
        {
            if (rawMessage == null)
            {
                throw new ArgumentNullException("rawMessage");
            }

            if (rawMessage == cachedRawMessage)
            {
                return cachedParams;
            }

            var parameters = new Collection<string>();
            var param = new StringBuilder();
            for (int i = startIndex; i < rawMessage.Length; i++)
            {
                char c = rawMessage[i];
                if (c == ' ')
                {
                    parameters.Add(param.ToString());
                    param = new StringBuilder();
                }
                else if (i + 1 == rawMessage.Length)
                {
                    param.Append(c);
                    parameters.Add(param.ToString());
                    param = new StringBuilder();
                }
                else if (c == ':' && param.Length == 0)
                {
                    parameters.Add(rawMessage.Substring(i + 1));
                    break;
                }
                else
                {
                    param.Append(c);
                }
            }

            cachedRawMessage = rawMessage;
            cachedParams = parameters;

            return parameters;
        }

        /// <summary>
        ///   Gets the last parameter in the parameters collection of the given unparsed message.
        /// </summary>
        public static string GetLastParameter(string rawMessage)
        {
            IList<string> p = GetParameters(rawMessage);
            return p.Count > 0 ? p[p.Count - 1] : string.Empty;
        }

        /// <summary>
        ///   Gets the nth parameter in the parameters collection of the given unparsed message.
        /// </summary>
        public static string GetParameter(string rawMessage, int index)
        {
            IList<string> p = GetParameters(rawMessage);
            return p.Count > index ? p[index] : string.Empty;
        }

        /// <summary>
        ///   Gets the substring in the input string existing between the given two strings.
        /// </summary>
        /// <param name="input">The string to search in.</param>
        /// <param name="before">The string before the one you want.</param>
        /// <param name="after">The string after the one you want.</param>
        public static string StringBetweenStrings(string input, string before, string after)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (before == null)
            {
                throw new ArgumentNullException("before");
            }
            if (after == null)
            {
                throw new ArgumentNullException("after");
            }

            int startOfBetween = 0;
            if (before.Length != 0)
            {
                int indexOfBefore = input.IndexOf(before, StringComparison.Ordinal);
                if (indexOfBefore > -1)
                {
                    startOfBetween = indexOfBefore + before.Length;
                }
            }

            if (after.Length == 0)
            {
                return input.Substring(startOfBetween);
            }
            int startOfEnd = input.LastIndexOf(after, StringComparison.Ordinal);
            if (startOfEnd == -1)
            {
                return input.Substring(startOfBetween);
            }
            int lengthOfBetween = startOfEnd - startOfBetween;
            return input.Substring(startOfBetween, lengthOfBetween);
        }

        /// <summary>
        ///   Gets the index of the nth time that <paramref name="searchValue" /> shows up in text.
        /// </summary>
        /// <param name="text">The string to search in.</param>
        /// <param name="searchValue">The string to search for.</param>
        /// <param name="startIndex">The place to start looking.</param>
        /// <param name="nthItem">The item to stop at.</param>
        public static int NthIndexOf(string text, string searchValue, int startIndex, int nthItem)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            int result = -1;
            int currentStartIndex = startIndex;
            for (int i = 0; i < nthItem; i++)
            {
                result = text.IndexOf(searchValue, currentStartIndex, StringComparison.Ordinal);
                if (result == -1)
                {
                    return result;
                }
                currentStartIndex = result + 1;
            }
            return result;
        }

        /// <summary>
        ///   Turns a <see cref="DateTime" /> into the integer representation as is commonly used on Unix machines.
        /// </summary>
        public static int ConvertToUnixTime(DateTime dt)
        {
            var unixEpochStartDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ticksSinceEpochStart = dt.Ticks - unixEpochStartDate.Ticks;
            var ts = new TimeSpan(ticksSinceEpochStart);
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <summary>
        ///   Turns the given Unix representation of a date/time into a <see cref="DateTime" />.
        /// </summary>
        public static DateTime ConvertFromUnixTime(int ut)
        {
            var unixEpochStartDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return unixEpochStartDate.AddSeconds(ut);
        }

        /// <summary>
        ///   Converts the given Unix representation of a date/time into a <see cref="Nullable&lt;DateTime&gt;" />.
        /// </summary>
        /// <remarks>
        ///   If the string can't be parsed, a null <see cref="DateTime" /> is returned.
        /// </remarks>
        public static DateTime? ConvertFromUnixTime(string unixTimeString)
        {
            if (string.IsNullOrEmpty(unixTimeString))
            {
                return null;
            }

            int unixTime;
            if (int.TryParse(unixTimeString, NumberStyles.Integer, CultureInfo.InvariantCulture, out unixTime))
            {
                return ConvertFromUnixTime(unixTime);
            }
            return null;
        }

        /// <summary>
        /// Creates a list of IRC parameters from the given collection of strings.
        /// </summary>
        public static string ParametersToString(IEnumerable<string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            var result = new StringBuilder();
            using (var enumerator = parameters.GetEnumerator())
            {
                var isCurrent = enumerator.MoveNext();
                while (isCurrent)
                {
                    var parameter = enumerator.Current;
                    isCurrent = enumerator.MoveNext();
                    if (isCurrent)
                    {
                        result.Append(parameter);
                        result.Append(' ');
                    }
                    else
                    {
                        if (parameter.IndexOf(' ') > 0)
                        {
                            result.Append(':');
                        }
                        result.Append(parameter);
                    }
                }
            }
            return result.ToString();
        }
    }
}
