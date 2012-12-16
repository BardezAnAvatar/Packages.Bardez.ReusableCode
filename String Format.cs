using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
                StringFormat.ToStringAlignment(append, 2);
        }

        /// <summary>Outputs the byte array to screen as a string of hexidecimal characters for each byte</summary>
        /// <param name="data">Byte array to print</param>
        /// <returns>A String intended for print to console.</returns>
        public static String ByteArrayToHexString(Byte[] data)
        {
            StringBuilder hexData = new StringBuilder();

            for (Int32 i = 0; i < data.Length; ++i)
            {
                if (i % 21 == 0)
                    hexData.Append("\r\n\t\t");

                hexData.Append(String.Format("{0:X2} ", data[i]));
            }

            return hexData.ToString();
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor)
        {
            return StringFormat.ToStringAlignment(descriptor, 1);
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor, Int32 tabs)
        {
            return StringFormat.ReturnAndIndent(String.Format("{0, -48}", descriptor + ":"), tabs);
        }
        
        /// <summary>Formats the string to be indented with a leading newline and specified number of tabs</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <returns>The formatted string, with a leading newline and tab</returns>
        public static String ReturnAndIndent(String value, Int32 tabs)
        {
            StringBuilder indent = new StringBuilder();
            indent.Append("\r\n");

            for (Int32 i = 0; i < tabs; ++i)
                indent.Append("\t");

            indent.Append(value);

            return indent.ToString();
        }

        /// <summary>Formats the string such that all lines in the string passed in have a given number of leading tabs</summary>
        /// <param name="source">Source String to be aligned</param>
        /// <param name="tabCount">Number of leading tabs in the indented descriptor</param>
        /// <returns>The formatted string, with a leading tabs</returns>
        public static String IndentAllLines(String source, Int32 tabCount)
        {
            StringBuilder indent = new StringBuilder();
            using (StringReader reader = new StringReader(source))
            {
                String line = null;
                while ((line = reader.ReadLine()) != null)
                    indent.Append(StringFormat.ReturnAndIndent(line, tabCount));
            }

            return indent.ToString();
        }
    }
}