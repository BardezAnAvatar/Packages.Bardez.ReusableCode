using System;
using System.Collections.Generic;
using System.Linq;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Extension class for <see cref="IEnumerable{T}" /></summary>
    public static class IEnumerableExtensions
    {
        /// <summary>Returns true if enumerable is null or contains no elements.</summary>
        /// <typeparam name="T">Type of enumerable collection</typeparam>
        /// <param name="enumerable">Enumerable collection to evaluate</param>
        /// <returns>True if null or empty; otherwise false</returns>
        /// <remarks>From http://stackoverflow.com/a/5047370/103058 </remarks>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        /// <summary>Returns a single comma-delimited <see cref="String" /> representing the enumerable collection, each item quoted</summary>
        /// <param name="enumerable">Enumerable collection of strings to concatenate</param>
        /// <returns>A single comma-delimited <see cref="String" /> representing the enumerable collection, each item quoted</returns>
        public static String QuoteAndJoin(this IEnumerable<String> enumerable)
        {
            return String.Join(", ", enumerable.Select(s => String.Format("\"{0}\"", s)));
        }
    }
}