using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Describes the most common approaches to storing memory in a computer.</summary>
    public enum Endianness
    {
        BigEndian,
        LittleEndian
    }

    /// <summary>This public class is just a collection of static methods that can be reused from many different code locations.</summary>
    /// <remarks>This code asumes reading values saved from little-endianness unless the input is othewise specified.</remarks>
    public static class ReusableIO
    {
        #region Basic Stream reading
        /// <summary>This public method reads a specified number of Bytes from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="readLength">Length of binary data to read</param>
        /// <returns>a Byte array containing the read data</returns>
        public static Byte[] BinaryRead(Stream input, Int32 readLength)
        {
            return BinaryRead(input, Convert.ToInt64(readLength));
        }

        /// <summary>This public method reads a specified number of Bytes from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="readLength">Length of binary data to read</param>
        /// <returns>a Byte array containing the read data</returns>
        public static Byte[] BinaryRead(Stream input, Int64 readLength)
        {
            Int32 offset = 0;
            Int64 remainingLength = readLength;
            Byte[] returnArr = new Byte[readLength];

            //read repeatedly from the stream. this accounts for reading that stops before eof, if the stream is buffered by a slow drive of network, etc.
            while (remainingLength > 0L)
            {
                //Stream.Read cannot take anything larger than an Int32; no UInt32 or any Int64, so shorten it as/if necessary
                Int32 readSize = remainingLength > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(remainingLength);

                //do the read
                Int32 readCount = input.Read(returnArr, offset, readSize);

                //if there was a problem with the read
                if (readCount < 1)
                    throw new EndOfStreamException(String.Format("End of stream reached with {0} bytes left to read", remainingLength));

                remainingLength -= readCount;
                offset += readCount;
            }

            return returnArr;
        }

        /// <summary>This public method reads a single Byte from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>a Byte containing the read value</returns>
        public static Byte BinaryReadByte(Stream input)
        {
            Int32 value = input.ReadByte();
            if (value < 0)
                throw new EndOfStreamException("Could not read the next Byte.");

            return Convert.ToByte(value); ;
        }

        /// <summary>This public method seeks the data stream to an appropriate position if the stream is not currently positioned there.</summary>
        /// <param name="DataStream">Stream that needs to seek</param>
        /// <param name="SeekPosition">Target position of the stream</param>
        /// <param name="SeekOrientation">SeekOrigin enumerator for where in the Stream to seek from</param>
        /// <remarks>This does not yet properly support any seek operation other than Begin</remarks>
        public static void SeekIfAble(Stream DataStream, Int64 SeekPosition, SeekOrigin SeekOrientation = SeekOrigin.Begin)
        {
            if (DataStream.Position != SeekPosition && DataStream.CanSeek)
                DataStream.Seek(Convert.ToInt64(SeekPosition), SeekOrientation);
            else if (DataStream.Position != SeekPosition && !DataStream.CanSeek)
                throw new InvalidOperationException("Stream cannot seek and position is not correct.");
        }
        #endregion


        #region Read ... From Byte Array
        /// <summary>This public method reads an Int16 from the source array</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="offset">The offset within the source array to read from</param>
        /// <returns>The Int16 read from the array</returns>
        public static Int16 ReadInt16FromArray(Byte[] source, Int64 offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(source, 2, offset);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int16 from the source array</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="offset">The offset within the source array to read from</param>
        /// <returns>The UInt16 read from the array</returns>
        public static UInt16 ReadUInt16FromArray(Byte[] source, Int64 offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(source, 2, offset);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>This public method reads an Int32 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Int32 read from the array</returns>
        public static Int32 ReadInt32FromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 4, Offset);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int32 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The UInt32 read from the array</returns>
        public static UInt32 ReadUInt32FromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 4, Offset);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>This public method reads an Int64 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Int64 read from the array</returns>
        public static Int64 ReadInt64FromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 8, Offset);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int64 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The UInt64 read from the array</returns>
        public static UInt64 ReadUInt64FromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 8, Offset);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>This public method reads a Single from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Single read from the array</returns>
        public static Single ReadSingleFromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 4, Offset);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>This public method reads a Double from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <returns>The Double read from the array</returns>
        public static Double ReadDoubleFromArray(Byte[] Source, Int32 Offset)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 8, Offset);
            return BitConverter.ToDouble(buffer, 0);
        }

        #region Endianness
        /// <summary>This public method reads an Int16 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int16 read from the array</returns>
        public static Int16 ReadInt16FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReusableIO.ReadVariableFromArray(Source, 2, Offset, endianness);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int16 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt16 read from the array</returns>
        public static UInt16 ReadUInt16FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 2, Offset, endianness);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>This public method reads an Int32 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int32 read from the array</returns>
        public static Int32 ReadInt32FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 4, Offset, endianness);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int32 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt32 read from the array</returns>
        public static UInt32 ReadUInt32FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 4, Offset, endianness);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>This public method reads an Int64 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int64 read from the array</returns>
        public static Int64 ReadInt64FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 8, Offset, endianness);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>This public method reads an unsigned Int64 from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt64 read from the array</returns>
        public static UInt64 ReadUInt64FromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 8, Offset, endianness);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>This public method reads a Single from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Single read from the array</returns>
        public static Single ReadSingleFromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 4, Offset, endianness);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>This public method reads a Double from the source array</summary>
        /// <param name="Source">The source array to read from</param>
        /// <param name="Offset">The offset within the source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Double read from the array</returns>
        public static Double ReadDoubleFromArray(Byte[] Source, Int32 Offset, Endianness endianness)
        {
            Byte[] buffer = ReadVariableFromArray(Source, 8, Offset, endianness);
            return BitConverter.ToDouble(buffer, 0);
        }
        #endregion

        /// <summary>This public method reads a string from a byte array of an ASCII-encoding</summary>
        /// <param name="Source">Byte array to read from</param>
        /// <param name="Offset">Offset within the byte array to read from</param>
        /// <param name="cultureRef">String describing the culture info for ASCII encoding</param>
        /// <param name="Length">Optional parameter indicating the length of the string to read. The default value is 8, for resource references.</param>
        /// <returns>The string read from the byte array</returns>
        public static String ReadStringFromByteArray(Byte[] Source, Int64 Offset, String CultureRef, Int64 Length = 8)
        {
            Byte[] temp = new Byte[Length];
            Array.Copy(Source, Offset, temp, 0, Length);

            CultureInfo culture = new CultureInfo(CultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);
            return encoding.GetString(temp);
        }
        #endregion


        #region Read ... From Stream
        /// <summary>This public method reads an Int16 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int16 read from the Stream</returns>
        public static Int16 ReadInt16FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 2);
            return ReusableIO.ReadInt16FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an Int16 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int16 read from the Stream</returns>
        public static Int16 ReadInt16FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 2);
            return ReusableIO.ReadInt16FromArray(bin, 0);
        }

        /// <summary>This public method reads an UInt16 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt16 read from the Stream</returns>
        public static UInt16 ReadUInt16FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 2);
            return ReusableIO.ReadUInt16FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an UInt16 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt16 read from the Stream</returns>
        public static UInt16 ReadUInt16FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 2);
            return ReusableIO.ReadUInt16FromArray(bin, 0);
        }

        /// <summary>This public method reads an Int32 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int32 read from the Stream</returns>
        public static Int32 ReadInt32FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 4);
            return ReusableIO.ReadInt32FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an Int32 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int32 read from the Stream</returns>
        public static Int32 ReadInt32FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 4);
            return ReusableIO.ReadInt32FromArray(bin, 0);
        }

        /// <summary>This public method reads an UInt32 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt32 read from the Stream</returns>
        public static UInt32 ReadUInt32FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 4);
            return ReusableIO.ReadUInt32FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an UInt32 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt32 read from the Stream</returns>
        public static UInt32 ReadUInt32FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 4);
            return ReusableIO.ReadUInt32FromArray(bin, 0);
        }

        /// <summary>This public method reads an Int64 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int64 read from the Stream</returns>
        public static Int64 ReadInt64FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 8);
            return ReusableIO.ReadInt64FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an Int64 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The Int64 read from the Stream</returns>
        public static Int64 ReadInt64FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 8);
            return ReusableIO.ReadInt64FromArray(bin, 0);
        }

        /// <summary>This public method reads an UInt64 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt64 read from the Stream</returns>
        public static UInt64 ReadUInt64FromStream(Stream source, Endianness endianness)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 8);
            return ReusableIO.ReadUInt64FromArray(bin, 0, endianness);
        }

        /// <summary>This public method reads an UInt64 from the source Stream</summary>
        /// <param name="source">The source array to read from</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>The UInt64 read from the Stream</returns>
        public static UInt64 ReadUInt64FromStream(Stream source)
        {
            Byte[] bin = ReusableIO.BinaryRead(source, 8);
            return ReusableIO.ReadUInt64FromArray(bin, 0);
        }
        #endregion


        #region Generate String's Byte Array
        /// <summary>This public method writes a string to a byte array of an ASCII-encoding</summary>
        /// <param name="Source">String to write</param>
        /// <param name="cultureRef">String describing the culture info for ASCII encoding</param>
        /// <returns>A Byte array containing the bytes of the string</returns>
        public static Byte[] GetStringByteArray(String Source, String CultureRef)
        {
            CultureInfo culture = new CultureInfo(CultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);

            return encoding.GetBytes(Source);
        }

        /// <summary>This public method writes a string to a byte array of an ASCII-encoding</summary>
        /// <param name="Source">String to write</param>
        /// <param name="cultureRef">String describing the culture info for ASCII encoding</param>
        /// <param name="Length">Optional parameter indicating the length of the byte array to return. The default value is 8, for resource references.</param>
        /// <returns>A Byte array containing the bytes of the string</returns>
        public static Byte[] GetStringByteArray(String Source, String CultureRef, Int32 Length = 8)
        {
            //The return array
            Byte[] returnArray = new Byte[Length];
            for (Int32 i = 0; i < Length; ++i)
                returnArray[i] = 0;

            CultureInfo culture = new CultureInfo(CultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);

            Byte[] stringBytes = encoding.GetBytes(Source);
            Array.Copy(stringBytes, returnArray, stringBytes.Length < Length ? stringBytes.Length : Length);

            return returnArray;
        }

        /// <summary>This public method writes a string to a byte array of an ASCII-encoding</summary>
        /// <param name="Source">String to write</param>
        /// <param name="Length">Optional parameter indicating the length of the byte array to return. The default value is 8, for resource references.</param>
        /// <returns>A Byte array containing the bytes of the string</returns>
        public static Byte[] GetStringByteArray(String Source, Int32 Length = 8)
        {
            //The return array
            Byte[] returnArray = new Byte[Length];
            for (Int32 i = 0; i < Length; ++i)
                returnArray[i] = 0;

            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] stringBytes = encoding.GetBytes(Source);
            Array.Copy(stringBytes, returnArray, stringBytes.Length < Length ? stringBytes.Length : Length);

            return returnArray;
        }
        #endregion


        #region Write primitive to Byte Array
        /// <summary>This public method writes a string to a byte array of an ASCII-encoding</summary>
        /// <param name="source">String to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="cultureRef">String describing the culture info for ASCII encoding</param>
        /// <param name="entireString">Boolean indicating whether or not to write the full String binary contents</param>
        /// <param name="requiredLength">Length of the character array being written to. -1 means full string, default = 8</param>
        /// <returns>A Byte array containing the bytes of the string</returns>
        public static void WriteStringToArray(String source, Byte[] output, Int64 position, String cultureRef, Boolean entireString = false, Int32 requiredLength = 8)
        {
            CultureInfo culture = new CultureInfo(cultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);

            Byte[] temp;
            if (entireString)
                temp = encoding.GetBytes(source ?? String.Empty);
            else
            {
                temp = new Byte[requiredLength];
                Byte[] temp2 = encoding.GetBytes(source ?? String.Empty);
                Int32 length = temp2.Length > temp.Length ? temp.Length : temp2.Length;
                Array.Copy(temp2, temp, length);
            }

            ReusableIO.WriteVariableToArray(output, position, temp);
        }

        /// <summary>This public method writes a signed byte to an output Byte array</summary>
        /// <param name="datum">SByte to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteSByteToArray(SByte datum, Byte[] output, Int64 position)
        {
            output[position] = (Byte)datum;
        }

        /// <summary>This public method writes a signed Int16 to an output Byte array</summary>
        /// <param name="datum">Int16 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteInt16ToArray(Int16 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes a signed Int16 to an output Byte array</summary>
        /// <param name="datum">Int16 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt16ToArray(Int16 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed Int32 to an output Byte array</summary>
        /// <param name="datum">Int32 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteInt32ToArray(Int32 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes a signed Int32 to an output Byte array</summary>
        /// <param name="datum">Int32 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt32ToArray(Int32 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed Int64 to an output Byte array</summary>
        /// <param name="datum">Int64 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteInt64ToArray(Int64 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes a signed Int64 to an output Byte array</summary>
        /// <param name="datum">Int64 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt64ToArray(Int64 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes an unsigned UInt16 to an output Byte array</summary>
        /// <param name="datum">IntU16 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteUInt16ToArray(UInt16 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes an unsigned UInt16 to an output Byte array</summary>
        /// <param name="datum">IntU16 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt16ToArray(UInt16 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes an unsigned UInt32 to an output Byte array</summary>
        /// <param name="datum">UInt32 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteUInt32ToArray(UInt32 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes an unsigned UInt32 to an output Byte array</summary>
        /// <param name="datum">UInt32 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt32ToArray(UInt32 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes an unsigned UInt64 to an output Byte array</summary>
        /// <param name="datum">UInt64 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteUInt64ToArray(UInt64 datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes an unsigned UInt64 to an output Byte array</summary>
        /// <param name="datum">UInt64 to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt64ToArray(UInt64 datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes a Single to an output Byte array</summary>
        /// <param name="datum">Single to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteSingleToArray(Single datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes a Single to an output Byte array</summary>
        /// <param name="datum">Single to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteSingleToArray(Single datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }

        /// <summary>This public method writes a Double to an output Byte array</summary>
        /// <param name="datum">Double to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        public static void WriteDoubleToArray(Double datum, Byte[] output, Int64 position)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes);
        }

        /// <summary>This public method writes a Double to an output Byte array</summary>
        /// <param name="datum">Double to write</param>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteDoubleToArray(Double datum, Byte[] output, Int64 position, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToArray(output, position, writeBytes, endianness);
        }
        #endregion


        #region Write primitive to Stream
        /// <summary>This public method writes a string to a byte array of an ASCII-encoding</summary>
        /// <param name="source">String to write</param>
        /// <param name="output">Stream into which to write the string</param>
        /// <param name="cultureRef">String describing the culture info for ASCII encoding</param>
        /// <param name="entireString">Boolean indicating whether or not to write the full String binary contents</param>
        /// <param name="requiredLength">Length of the character array being written to. -1 means full string, default = 8</param>
        /// <returns>A Byte array containing the bytes of the string</returns>
        public static void WriteStringToStream(String source, Stream output, String cultureRef, Boolean entireString = false, Int32 requiredLength = 8)
        {
            CultureInfo culture = new CultureInfo(cultureRef);
            Encoding encoding = Encoding.GetEncoding(culture.TextInfo.ANSICodePage);

            Byte[] temp;
            if (entireString)
                temp = encoding.GetBytes(source ?? String.Empty);
            else
            {
                temp = new Byte[requiredLength];
                Byte[] temp2 = encoding.GetBytes(source ?? String.Empty);
                Int32 length = temp2.Length > temp.Length ? temp.Length : temp2.Length;
                Array.Copy(temp2, temp, length);
            }

            output.Write(temp, 0, temp.Length);
        }

        /// <summary>This public method writes a signed byte to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">SByte to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteSByteToStream(SByte datum, Stream output)
        {
            output.WriteByte((Byte)datum);
        }

        /// <summary>This public method writes a signed Int16 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int16 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteInt16ToStream(Int16 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed Int16 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int16 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt16ToStream(Int16 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed Int32 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int32 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteInt32ToStream(Int32 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed Int32 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int32 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt32ToStream(Int32 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed Int64 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int64 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteInt64ToStream(Int64 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed Int64 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Int64 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteInt64ToStream(Int64 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed UInt16 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">IntU16 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteUInt16ToStream(UInt16 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed UInt16 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">IntU16 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt16ToStream(UInt16 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed UInt32 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">UInt32 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteUInt32ToStream(UInt32 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed UInt32 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">UInt32 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt32ToStream(UInt32 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a signed UInt64 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">UInt64 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteUInt64ToStream(UInt64 datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a signed UInt64 to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">UInt64 to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteUInt64ToStream(UInt64 datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a Single to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Single to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteSingleToStream(Single datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a Single to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Single to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteSingleToStream(Single datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }

        /// <summary>This public method writes a Double to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Double to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        public static void WriteDoubleToStream(Double datum, Stream output)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            output.Write(writeBytes, 0, writeBytes.Length);
        }

        /// <summary>This public method writes a Double to an output <see cref="System.IO.Stream" /></summary>
        /// <param name="datum">Double to write</param>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="endianness">The desired endianness to be written</param>
        public static void WriteDoubleToStream(Double datum, Stream output, Endianness endianness)
        {
            Byte[] writeBytes = BitConverter.GetBytes(datum);
            ReusableIO.WriteVariableToStream(output, writeBytes, endianness);
        }
        #endregion


        #region File IO
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
        /// <param name="source">The original data array</param>
        /// <param name="length">The length of binary data to be extracted</param>
        /// <param name="offset">The offset within the source array to start reading at</param>
        /// <returns>a Byte array containing the extracted data</returns>
        private static Byte[] ReadVariableFromArray(Byte[] source, Int32 length, Int64 offset)
        {
            Byte[] buffer = new Byte[length];
            Array.Copy(source, offset, buffer, 0, length);

            //This is stupid, but I am eyeing the iPhone... it makes me *aware* of such issues.
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }

        /// <summary>
        ///     This private method will read binary data of a given length from an array and return that sub-array.
        ///     If the system is Big-Endian, the resultant array will be flipped.
        /// </summary>
        /// <param name="Source">The original data array</param>
        /// <param name="Length">The length of binary data to be extracted</param>
        /// <param name="Offset">The offset within the source array to start reading at</param>
        /// <param name="endianness">The desired endianness to be read</param>
        /// <returns>a Byte array containing the extracted data</returns>
        /// <remarks>This is so heavily hit that I want to avoid usage unless I really need it</remarks>
        private static Byte[] ReadVariableFromArray(Byte[] Source, Int32 Length, Int32 Offset, Endianness endianness = Endianness.LittleEndian)
        {
            Byte[] buffer = new Byte[Length];
            Array.Copy(Source, Offset, buffer, 0, Length);

            //if the current endianness does not match the desired endianness
            if (BitConverter.IsLittleEndian != (endianness == Endianness.LittleEndian ? true : false))
                Array.Reverse(buffer);

            return buffer;
        }

        /// <summary>
        ///     This private method will write binary data to an output stream
        ///     If the endianness does match that of the current system, the source array will be flipped.
        /// </summary>
        /// <param name="output"><see cref="System.IO.Stream" /> to write to.</param>
        /// <param name="data">The original data array</param>
        /// <param name="endianness">The desired endianness to be written</param>
        /// <remarks>This is so heavily hit that I want to avoid usage unless I really need it</remarks>
        private static void WriteVariableToStream(Stream output, Byte[] data, Endianness endianness)
        {
            //if the current endianness does not match the desired endianness
            if (BitConverter.IsLittleEndian != (endianness == Endianness.LittleEndian ? true : false))
                Array.Reverse(data);

            output.Write(data, 0, data.Length);
        }

        /// <summary>
        ///     This private method will write binary data to an output binary array
        ///     If the endianness does match that of the current system, the source array will be flipped.
        /// </summary>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="data">The original data array</param>
        private static void WriteVariableToArray(Byte[] output, Int64 position, Byte[] data)
        {
            //This is stupid, but I am eyeing the iPhone... it makes me *aware* of such issues.
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);

            Int32 len = data.Length;
            for (Int32 i = 0; i < len; ++i)
                output[position + i] = data[i];
        }

        /// <summary>
        ///     This private method will write binary data to an output binary array
        ///     If the endianness does match that of the current system, the source array will be flipped.
        /// </summary>
        /// <param name="output"><see cref="System.Array" /> to write to.</param>
        /// <param name="position">Position of the output array to write to</param>
        /// <param name="data">The original data array</param>
        /// <param name="endianness">The desired endianness to be written</param>
        /// <remarks>This is so heavily hit that I want to avoid usage unless I really need it</remarks>
        private static void WriteVariableToArray(Byte[] output, Int64 position, Byte[] data, Endianness endianness)
        {
            //if the current endianness does not match the desired endianness
            if (BitConverter.IsLittleEndian != (endianness == Endianness.LittleEndian))
                Array.Reverse(data);

            Int32 len = data.Length;
            for (Int32 i = 0; i < len; ++i)
                output[position + i] = data[i];
        }
        #endregion
    }
}