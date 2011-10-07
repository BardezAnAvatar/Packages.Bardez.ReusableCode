using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.ReusableCode
{
    public class IntExtension
    {
        /// <summary>
        ///     Determines if the inquiry offset data falls within start (inclusive) until end (exclusive).
        ///     Exceptions: If start == end, returns false. Also returns false if the size is zero.
        /// </summary>
        /// <param name="inquiry">Offset in question</param>
        /// <param name="size">Size of offset data</param>
        /// <param name="start">Start value of range</param>
        /// <param name="end">End value of range</param>
        /// <returns>A Boolean indicating whethe or not inquiry falls within the range</returns>
        public static Boolean Between(Int64 inquiry, Int64 size, Int64 start, Int64 end)
        {
            Boolean between = false;

            if (start < end && size > 0L)
                between = inquiry == start | (inquiry > start && inquiry < end);

            return between;
        }
    }
}