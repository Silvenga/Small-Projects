using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SlightLibrary.Extensions {

    /// <summary>
    /// Contains Exstentions for the String object
    /// </summary>
    public static class StringExtensions {

        private static readonly char[] WhiteSpace = { ' ' };

        /// <summary>
        /// Returns if is Palindrome
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPalindrome(this string str) {

            str = str.ToLower();
            str = str.RemoveChars(WhiteSpace);
            return str == new string(str.Reverse().ToArray());
        }

        /// <summary>
        /// Copies the string, removes the provided charArray from the new string
        /// Returns the new String
        /// TODO: Use generics
        /// </summary>
        /// <param name="str"></param>
        /// <param name="charToRmeove"></param>
        /// <returns></returns>
        public static string RemoveChars(this string str, IEnumerable<char> charToRmeove) {

            return str.Where(c => !charToRmeove.Contains(c)).Aggregate("", (current, c) => current + c);
        }

        /// <summary>
        /// Converts string to Title case using the current lanuage set.
        /// Source: http://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase.aspx
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string str) {

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }

}