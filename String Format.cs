using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Provides varios formatting helpers for Strings</summary>
    public static class StringFormat
    {
        /// <summary>Appends a leading \n\t\t to the string to append to a StringBuilder</summary>
        /// <param name="sb">StringBuilder object to Append to</param>
        /// <param name="condition">Boolean condition on which to actually do the append operation</param>
        /// <param name="append">String to be conditionally appended.</param>
        public static void AppendSubItem(StringBuilder sb, Boolean condition, String append)
        {
            if (condition)
            {
                sb.Append("\n\t\t");
                sb.Append(append);
            }
        }

        /// <summary>Outputs the byte array to screen as a string of hexidecimal characters for each byte</summary>
        /// <param name="data">Byte array to print</param>
        /// <returns>A String intended for print to console.</returns>
        public static String ReservedToStringHex(Byte[] data)
        {
            StringBuilder hexData = new StringBuilder();

            for (Int32 i = 0; i < data.Length; ++i)
            {
                //Int32 positionEnd = (((i + 1) * 3) % 64);
                //if (positionEnd > 1 && positionEnd < 5)
                //    hexData.Append("\n\t\t");
                if (i % 21 == 0)
                    hexData.Append("\n\t\t");

                hexData.Append(String.Format("{0:X2} ", data[i]));
            }

            return hexData.ToString();
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor)
        {
            return ToStringAlignment(descriptor, 1);
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading abs in the indented descriptor</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor, Int32 tabs)
        {
            StringBuilder indent = new StringBuilder();
            indent.Append("\n");

            for (Int32 i = 0; i < tabs; ++i)
                indent.Append("\t");

            return indent.ToString() + String.Format("{0:44}", descriptor + ":");
        }
    }
}