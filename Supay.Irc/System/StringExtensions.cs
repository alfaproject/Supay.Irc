namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        ///   Reports the zero-based index of the nth occurrence of the specified Unicode character in the string.
        ///   The search starts at a specified character position.
        /// </summary>
        /// <returns>
        ///   The zero-based index position of <paramref name="value"/> if that character is found, or -1 if it is not.
        /// </returns>
        /// <param name="string">The string to perform the search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="occurrence">The nth occurrence to seek.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than 0 (zero) or greater than the length of the string.</exception>
        public static int IndexOfOccurrence(this string @string, char value, int startIndex, int occurrence)
        {
            var result = @string.IndexOf(value, startIndex);
            while (--occurrence > 0 && result != -1)
            {
                result = @string.IndexOf(value, result + 1);
            }
            return result;
        }
    }
}
