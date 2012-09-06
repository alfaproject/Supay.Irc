using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        private static readonly Regex ircMessageRegex = new Regex(@"^(:(?<p>[^ ]+) )?(?<c>\w+)(( (:(?<a>.+)|(?<a>[^ ]+)))+)?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private static string cachedRawMessage;
        private static Match cachedMatchedMessage;

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
            if (!HasValidChannelPrefix(result))
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
            return channelName[0] == '#' || channelName[0] == '&' || channelName[0] == '+' || channelName[0] == '!';
        }

        /// <summary>
        ///   Extracts the Prefix from a string message.
        /// </summary>
        public static string GetPrefix(string rawMessage)
        {
            if (rawMessage != cachedRawMessage)
            {
                cachedRawMessage = rawMessage;
                cachedMatchedMessage = ircMessageRegex.Match(rawMessage);
            }

            return cachedMatchedMessage.Groups[@"p"].Value;
        }

        /// <summary>
        ///   Extracts the Command from a string message.
        /// </summary>
        public static string GetCommand(string rawMessage)
        {
            if (rawMessage != cachedRawMessage)
            {
                cachedRawMessage = rawMessage;
                cachedMatchedMessage = ircMessageRegex.Match(rawMessage);
            }

            return cachedMatchedMessage.Groups[@"c"].Value;
        }

        /// <summary>
        ///   Gets the parameters of the raw message.
        /// </summary>
        /// <param name="rawMessage">The message string which has the parameters.</param>
        public static IList<string> GetParameters(string rawMessage)
        {
            if (rawMessage != cachedRawMessage)
            {
                cachedRawMessage = rawMessage;
                cachedMatchedMessage = ircMessageRegex.Match(rawMessage);
            }

            return (from Capture argument in cachedMatchedMessage.Groups[@"a"].Captures
                    select argument.Value).ToList();
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
