using System;
using System.Collections.Generic;
using System.Linq;

namespace Bardez.Projects.ReusableCode
{
    public static class StringExtension
    {
        public static Boolean ContainsWhiteSpace(this String me)
        {
            Boolean contains = false;

            foreach (Char c in me)
            {
                contains = Char.IsWhiteSpace(c);

                if (contains)
                    break;
            }

            return contains;
        }

        public static Boolean Contains(this List<String> list, String value, Boolean ignoreCase = true )
        {
            return ignoreCase ? list.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase)) :
                list.Contains(value);
        }

        public static Boolean IsInt64(this String me)
        {
            return Int64.TryParse(me, out Int64 bogus);
        }

        public static Boolean IsInt32(this String me)
        {
            return Int32.TryParse(me, out Int32 bogus);
        }

        public static Boolean IsInt16(this String me)
        {
            return Int16.TryParse(me, out Int16 bogus);
        }

        public static Boolean IsByte(this String me)
        {
            return Byte.TryParse(me, out Byte bogus);
        }

        public static Boolean IsDouble(this String me)
        {
            return Double.TryParse(me, out Double bogus);
        }

        public static Boolean IsSingle(this String me)
        {
            return Single.TryParse(me, out Single bogus);
        }

        public static Boolean IsDecimal(this String me)
        {
            return Decimal.TryParse(me, out Decimal bogus);
        }
    }
}