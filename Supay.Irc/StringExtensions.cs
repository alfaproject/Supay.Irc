using System;

namespace Supay.Irc
{
    public static class StringExtensions
    {
        /// <summary>
        ///   Reports the zero-based index of the nth occurrence of the specified string in this instance. The search
        ///   starts at a specified character position.
        /// </summary>
        /// <returns>
        ///   The zero-based index position of <paramref name="value"/> if that string is found, or -1 if it is not.
        ///   If <paramref name="value"/> is <see cref="String.Empty"/>, the return value is <paramref name="startIndex"/>.
        /// </returns>
        /// <param name="text">The string to search in.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="occurrence">The nth occurrence to return.</param>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <b>null</b>.</exception>
        public static int IndexOfOccurrence(this string text, string value, int startIndex, int occurrence)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var result = text.IndexOf(value, startIndex, StringComparison.Ordinal);
            while (--occurrence > 0 && result != -1)
            {
                result = text.IndexOf(value, result + 1, StringComparison.Ordinal);
            }
            return result;
        }
    }
}
