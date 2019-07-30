using System;
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
                StringFormat.ToStringAlignment(append, 2, sb, false);
        }

        /// <summary>Outputs the byte array to screen as a string of hexidecimal characters for each byte</summary>
        /// <param name="data">Byte array to print</param>
        /// <returns>A String intended for print to console.</returns>
        public static String ByteArrayToHexString(Byte[] data)
        {
            StringBuilder hexData = new StringBuilder();

            StringFormat.ByteArrayToHexString(data, hexData);

            return hexData.ToString();
        }

        /// <summary>Outputs the byte array to screen as a string of hexidecimal characters for each byte</summary>
        /// <param name="data">Byte array to print</param>
        /// <param name="builder">StringBuilder to write to</param>
        public static void ByteArrayToHexString(Byte[] data, StringBuilder builder)
        {
            for (Int32 i = 0; i < data.Length; ++i)
            {
                if (i % 21 == 0)
                    builder.Append("\r\n\t\t");

                builder.Append(String.Format("{0:X2} ", data[i]));
            }
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="appendColon">Flag indicating whether or not to append a colon</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor, Boolean appendColon = true)
        {
            return StringFormat.ToStringAlignment(descriptor, 1);
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="builder">StringBuilder to write to</param>
        /// <param name="appendColon">Flag indicating whether or not to append a colon</param>
        public static void ToStringAlignment(String descriptor, StringBuilder builder, Boolean appendColon = true)
        {
            StringFormat.ToStringAlignment(descriptor, 1, builder);
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <param name="appendColon">Flag indicating whether or not to append a colon</param>
        /// <returns>the formatted string, with a leading newline and tab</returns>
        public static String ToStringAlignment(String descriptor, Int32 tabs, Boolean appendColon = true)
        {
            String colon = appendColon ? ":" : String.Empty;
            return StringFormat.ReturnAndIndent(String.Format("{0, -48}", descriptor + colon), tabs);
        }

        /// <summary>Formats the string to be indented to a uniform length</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <param name="appendColon">Flag indicating whether or not to append a colon</param>
        /// <param name="builder">StringBuilder to write to</param>
        public static void ToStringAlignment(String descriptor, Int32 tabs, StringBuilder builder, Boolean appendColon = true)
        {
            String colon = appendColon ? ":" : String.Empty;
            StringFormat.ReturnAndIndent(String.Format("{0, -48}", descriptor + colon), tabs, builder);
        }

        /// <summary>Formats the string to be indented with a leading newline and specified number of tabs</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <returns>The formatted string, with a leading newline and tab</returns>
        public static String ReturnAndIndent(String value, Int32 tabs)
        {
            StringBuilder indent = new StringBuilder();

            StringFormat.ReturnAndIndent(value, tabs, indent);

            return indent.ToString();
        }

        /// <summary>Formats the string to be indented with a leading newline and specified number of tabs</summary>
        /// <param name="descriptor">String to be aligned</param>
        /// <param name="tabs">Number of leading tabs in the indented descriptor</param>
        /// <param name="builder">StringBuilder to write to</param>
        public static void ReturnAndIndent(String value, Int32 tabs, StringBuilder builder)
        {
            builder.Append("\r\n");

            for (Int32 i = 0; i < tabs; ++i)
                builder.Append("\t");

            builder.Append(value);
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
                    StringFormat.ReturnAndIndent(line, tabCount, indent);
            }

            return indent.ToString();
        }

        /// <summary>Formats the string such that all lines in the string passed in have a given number of leading tabs</summary>
        /// <param name="source">Source String to be aligned</param>
        /// <param name="tabCount">Number of leading tabs in the indented descriptor</param>
        /// <returns>The formatted string, with a leading tabs</returns>
        public static void IndentAllLines(String source, Int32 tabCount, StringBuilder builder)
        {
            using (StringReader reader = new StringReader(source))
            {
                String line = null;
                while ((line = reader.ReadLine()) != null)
                    StringFormat.ReturnAndIndent(line, tabCount, builder);
            }
        }
    }
}