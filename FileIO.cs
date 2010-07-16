using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace InfinityPlus1.ReusableCode
{
    /// <summary>This public class is just a collection of static methods that can be reused from many different code locations.</summary>
    public static class ReusableIO
    {
        #region Public Methods
        /// <summary>This public method reads</summary>
        /// <param name="Input">Stream to read from</param>
        /// <param name="ReadLength">Length of binary data to read</param>
        /// <returns>a Byte array containing the read data</returns>
        public static Byte[] BinaryRead(Stream Input, Int32 ReadLength)
        {
            Int32 offset = 0;
            Int32 remainingLength = ReadLength;
            Byte[] returnArr = new Byte[ReadLength];

            //read repeatedly from the stream. this accounts for reading that stops before eof, if the stream is buffered by a slow drive of network, etc.
            while (remainingLength > 0)
            {
                //do the read
                Int32 readCount = Input.Read(returnArr, offset, remainingLength);

                //if there was a problem with the read
                if (readCount < 1)
                    throw new EndOfStreamException(String.Format("End of string reached with {0} bytes left to read", remainingLength));

                remainingLength -= readCount;
                offset += readCount;
            }

            return returnArr;
        }

        /// <summary>This public method reads an Int16 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Int16 read from the array</returns>
        public static Int16 ReadInt16FromArray(ref Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReadVariableFromArray(ref Source, 2, Offset);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>This public method reads an Int32 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Int32 read from the array</returns>
        public static Int32 ReadInt32FromArray(ref Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReadVariableFromArray(ref Source, 4, Offset);
            return BitConverter.ToInt32(buffer, 0);
        }
        
        /// <summary>This public method reads an Int64 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Int64 read from the array</returns>
        public static Int64 ReadInt64FromArray(ref Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReadVariableFromArray(ref Source, 8, Offset);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>This public method reads a string from a byte array of an ASCII-encoding</summary>
        /// <param name="Source">Byte array to read from</param>
        /// <param name="Offset">Offset within the byte array to read from</param>
        /// <param name="Length">Optional parameter indicating the length of the string to read. The default value is 8, for resource references.</param>
        /// <returns>The string read from the byte array</returns>
        public static String ReadStringFromByteArray(ref Byte[] Source, Int32 Offset, String CultureRef, Int32 Length = 8)
        {
            Byte[] temp = new Byte[Length];
            Array.Copy(Source, Offset, temp, 0, Length);

            CultureInfo culture = new CultureInfo(CultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);
            return encoding.GetString(temp);
        }

        /// <summary>This public method returns a FileStream from a given FilePath</summary>
        /// <param name="FilePath">String indicating the path of the file to open</param>
        /// <returns>A FileStream object</returns>
        /// <remarks>Remember that FileStream is IDisposable</remarks>
        public static FileStream OpenFile(String FilePath)
        {
            return OpenFile(FilePath, FileAccess.Read);
        }

        /// <summary>This public method returns a FileStream from a given FilePath</summary>
        /// <param name="FilePath">String indicating the path of the file to open</param>
        /// <param name="AccessFlags">AccessFlags enumerator describing how toaccess the file</param>
        /// <returns>A FileStream object</returns>
        /// <remarks>Remember that FileStream is IDisposable</remarks>
        public static FileStream OpenFile(String FilePath, FileAccess AccessFlags)
        {
            return OpenFile(FilePath, FileMode.Open, AccessFlags);
        }

        /// <summary>This public method returns a FileStream from a given FilePath</summary>
        /// <param name="FilePath">String indicating the path of the file to open</param>
        /// <param name="FileOpenMode">FileOpenMode enumerator describing how to deal with opening the file (creation, append, open, etc.)</param>
        /// <param name="AccessFlags">AccessFlags enumerator describing how toaccess the file</param>
        /// <returns>A FileStream object</returns>
        /// <remarks>Remember that FileStream is IDisposable</remarks>
        public static FileStream OpenFile(String FilePath, FileMode FileOpenMode, FileAccess AccessFlags)
        {
            return new FileStream(FilePath, FileOpenMode, AccessFlags);
        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        ///     This private method will read binary data of a given length from an array and return that sub-array.
        ///     If the system is Big-Endian, the resultant array will be flipped.
        /// </summary>
        /// <param name="Source">The original data array</param>
        /// <param name="Length">The length of binary data to be extracted</param>
        /// <param name="Offset">The offset within the source array to start reading at</param>
        /// <returns>a Byte array containing the extracted data</returns>
        private static Byte[] ReadVariableFromArray(ref Byte[] Source, Int32 Length, Int32 Offset)
        {
            Byte[] buffer = new Byte[Length];
            Array.Copy(Source, Offset, buffer, 0, Length);

            //This is stupid, but I am eyeing the iPhone... it makes me *aware* of such issues
            if (! BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        #endregion
    }
}