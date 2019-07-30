using System;
using System.IO;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Provides accessors for MemoryStream to let it behave akin to an array</summary>
    public static class MemoryStreamExtension
    {
        /// <summary>Reads a Byte from the input Stream. Throws an error if the resulting value is out of range</summary>
        /// <param name="input">Stream to read from</param>
        /// <returns>A Byte value read from the input Stream</returns>
        public static Byte ReadByteOrThrow(this MemoryStream input)
        {
            Int32 value = input.ReadByte();
            if (value < 0 || value > 255)
                throw new ArgumentOutOfRangeException(String.Format("Value '{0}' read from Stream input is not valid for a Byte.", value));

            return (Byte)value;
        }

        /// <summary>Seeks and reads a Byte from the input Stream. Throws an error if the resulting value is out of range</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset from which to read the Byte</param>
        /// <returns>A Byte value read from the input Stream</returns>
        public static Byte ReadByteAtOffset(this MemoryStream input, Int64 offset)
        {
            ReusableIO.SeekIfAble(input, offset);  //seek
            return ReadByteOrThrow(input);
        }

        /// <summary>Seeks and reads Bytes from the input Stream</summary>
        /// <param name="input">Stream to read from</param>
        /// <param name="offset">Offset from which to read the Bytes</param>
        /// <param name="length">Length of Bytes to read</param>
        /// <returns>A Byte value read from the input Stream</returns>
        public static Byte[] ReadBytesAtOffset(this MemoryStream input, Int32 offset, Int32 length)
        {
            ReusableIO.SeekIfAble(input, offset);  //seek
            Byte[] array = new Byte[length];
            input.Read(array, 0, length);
            return array;
        }

        /// <summary>Seeks and writes Bytes to the output Stream</summary>
        /// <param name="output">Stream to read from</param>
        /// <param name="offset">Offset at which to write the Bytes</param>
        /// <param name="buffer">Bytes to write</param>
        public static void WriteAtOffset(this MemoryStream output, Int32 offset, Byte[] buffer)
        {
            MemoryStreamExtension.WriteAtOffset(output, offset, buffer, buffer.Length);
        }

        /// <summary>Seeks and writes Bytes to the output Stream</summary>
        /// <param name="output">Stream to read from</param>
        /// <param name="offset">Offset at which to write the Bytes</param>
        /// <param name="buffer">Bytes to write</param>
        /// <param name="length">Length of bytes to write</param>
        public static void WriteAtOffset(this MemoryStream output, Int32 offset, Byte[] buffer, Int32 length)
        {
            ReusableIO.SeekIfAble(output, offset);  //seek
            output.Write(buffer, 0, length);
        }

        /// <summary>Reads the contents of the this Stream to the end</summary>
        /// <param name="input">input data Stream to read from</param>
        /// <returns>A Byte array of the data in the Stream</returns>
        [Obsolete("This method should be worthless. It's intent is superceded by MemoryStream.ToArray()")]
        public static Byte[] ReadToEnd(this MemoryStream input)
        {
            //store the position prior to read
            Int64 position = input.Position;

            Byte[] buffer = new Byte[UInt16.MaxValue];  //64K too much?
            Byte[] result = null;

            using (MemoryStream memStream = new MemoryStream())
            {
                Int32 dataRead;

                while ((dataRead = input.Read(buffer, 0, UInt16.MaxValue)) > 0)
                    memStream.Write(buffer, 0, dataRead);

                result = memStream.ToArray();
            }

            //rest the position
            ReusableIO.SeekIfAble(input, position);

            return result;
        }
    }
}