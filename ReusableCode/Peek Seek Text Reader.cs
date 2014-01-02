using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>
    ///     This class wraps the functionality of a TextReader and expands it so that you can
    ///     peek a certain number of characters ahead as well as seek within the data.
    ///     The source data (a TextReader) reads the entire input source to the end.
    /// </summary>
    public class PeekSeekTextReader : TextReader
    {
        #region Fields
        /// <summary>This is the internal buffer to use while seeking and peeking</summary>
        protected Char[] data;

        /// <summary>This is the index within the buffer that the reader is currently pointing at</summary>
        protected Int32 index;
        #endregion


        #region Construction
        /// <summary>Definition constructor. Reads the input source to the end.</summary>
        /// <param name="source">Source text reader being wrapped and to read from</param>
        public PeekSeekTextReader(TextReader source)
        {
            String fullRead = source.ReadToEnd();
            data = fullRead.ToCharArray();

            this.index = 0;
        }
        #endregion


        #region TextReader Methods
        /// <summary>Releases unmanaged reources used by the PeekSeekTextReader and optionally releases the managed resources</summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources</param>
        protected override void Dispose(Boolean disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                this.data = null;
        }

        /// <summary>
        ///     Reads the next character without changing the state of the reader or the character source.
        ///     Returns the next available character without reading it from the input stream.
        /// </summary>
        /// <returns>An integer representing the next character to be read, or -1 if no more characters are available.</returns>
        public override Int32 Peek()
        {
            return this.PeekAt(this.index);
        }

        /// <summary>Reads the next character and advances the character position by one character.</summary>
        /// <returns>An integer representing the character read, or -1 if no more characters are available.</returns>
        public override Int32 Read()
        {
            Int32 readCharacter = -1;

            if (this.data != null && this.index < this.data.Length)
            {
                readCharacter = Convert.ToInt32(this.data[index]);
                ++index;
            }

            return readCharacter;
        }

        /// <summary>
        ///     Reads a maximum of count characters fromt he current stream
        ///     and writes the data to the buffer, beginning at index.
        /// </summary>
        /// <param name="buffer">
        ///     When this method returns, contains the specified character array between index and (index + count -1)
        ///     replaced by the characters read from the source.
        /// </param>
        /// <param name="index">The position within buffer at which to begin writing.</param>
        /// <param name="count">
        ///     The maximum number of characters to read.
        ///     If the end of the stream is reached before count of characters is read into buffer, this method returns.
        /// </param>
        /// <returns>
        ///     The number of characters that have been read. The number will be less than or equal to count,
        ///     depending on whether the data is available within the reader.
        ///     This method returns 0 (zero) if it is called when no more characters are left to read.
        /// </returns>
        public override Int32 Read(Char[] buffer, Int32 index, Int32 count)
        {
            Int32 charactersRead = -1;

            if (buffer == null)
                throw new ArgumentNullException("buffer", "The buffer parameter was null.");
            else if (index >= buffer.Length)
                throw new ArgumentOutOfRangeException("index", "The index was out of range of the buffer.");
            else if ((buffer.Length - index) < count)
                throw new ArgumentException("The end index was out of range of the buffer.", "count");

            Int32 charactersAvailable = this.data.Length - this.index;
            if (charactersAvailable < count)
                charactersRead = charactersAvailable;
            else
                charactersRead = count;

            //copy data
            Array.Copy(this.data, this.index, buffer, index, charactersRead);

            //update the index
            this.index += charactersRead;

            return charactersRead;
        }

        /// <summary>
        ///     Reads a specified maximum number of characters from the current text reader
        ///     and writes the data to a buffer, beginning at the specified index.
        /// </summary>
        /// <param name="buffer">
        ///     When this method returns, this parameter contains the specified character array
        ///     with the values between index and (index + count -1) replaced by the characters
        ///     read from the current source.
        /// </param>
        /// <param name="index">The position in buffer at which to begin writing.</param>
        /// <param name="count">The maximum number of characters to read. </param>
        /// <returns>
        ///     The number of characters that have been read.
        ///     The number will be less than or equal to count, depending on whether all input characters have been read.
        /// </returns>
        public override Int32 ReadBlock(Char[] buffer, Int32 index, Int32 count)
        {
            return this.Read(buffer, index, count);
        }

        /// <summary>Reads a line of characters from the text reader and returns the data as a string.</summary>
        /// <returns>The next line from the reader, or null if all characters have been read.</returns>
        public override String ReadLine()
        {
            String output = null;

            if (this.data != null && this.index < this.data.Length)
            {
                Int32 startIndex = this.index;
                Boolean keepReading = true;

                while (keepReading && this.index < this.data.Length)
                {
                    switch (this.data[this.index])
                    {
                        case '\r':
                        case '\n':
                            keepReading = false;
                            break;
                    }
                }

                output = new String(this.data, startIndex, this.index - startIndex);
            }

            return output;
        }

        /// <summary>Reads all characters from the current position to the end of the text reader and returns them as one string.</summary>
        /// <returns>A string that contains all characters from the current position to the end of the text reader.</returns>
        public override String ReadToEnd()
        {
            String output = null;

            if (this.data != null && this.index < this.data.Length)
                output = new String(this.data, this.index, this.data.Length - this.index);

            //update the index
            this.index = this.data.Length;

            return output;
        }

        /// <summary>Reads all characters from the current position to the end of the text reader and returns them as one string.</summary>
        /// <returns>A string that contains all characters from the current position to the end of the text reader.</returns>
        public IList<Char> ReadToEndIList()
        {
            List<Char> output = new List<Char>();

            if (this.data != null && this.index < this.data.Length)
            {
                for (; this.index < this.data.Length; ++index)
                    output.Add(this.data[this.index]);
            }

            return output;
        }
        #endregion


        #region Added methods
        /// <summary>
        ///     Reads the character at the relative index to the current position
        ///     without changing the state of the reader or the character source.
        ///     Returns it without reading it from the input stream.
        /// </summary>
        /// <returns>An integer representing the relative character or -1 if no relative character available.</returns>
        /// <param name="relativePosition">Index relative to the PeekSeekReader's current position</param>
        /// <returns>An integer representing the character to be peeked, or -1 if no relative character available.</returns>
        public Int32 Peek(Int32 relativePosition)
        {
            return this.PeekAt(this.index + relativePosition);
        }

        /// <summary>Seeks to the specified offset from the specified origin</summary>
        /// <param name="offset">Offset to seek to</param>
        /// <param name="origin">Position from which to start seeking</param>
        public void Seek(Int32 offset, SeekOrigin origin = SeekOrigin.Current)
        {
            if (this.data == null)
                throw new NullReferenceException("The underlying data was null, so could not seek.");

            //validate
            if (origin == SeekOrigin.Begin || origin == SeekOrigin.End)
            {
                if (offset < 0)
                    throw new ArgumentOutOfRangeException("offset", String.Format("The specified offset ({0}) is less than 0.", offset));
                else if (offset > this.data.Length)
                    throw new ArgumentOutOfRangeException("offset", String.Format("The specified offset ({0}) is beyond the length of the data ({1}).", offset, this.data.Length));
            }
            else if (origin == SeekOrigin.Current)
            {
                if ((this.index + offset) < 0)
                    throw new ArgumentOutOfRangeException("offset", String.Format("The specified offset ({0}) relative to the current position ({1}) is less than 0.", offset, this.index));
                else if ((this.index + offset) > this.data.Length)
                    throw new ArgumentOutOfRangeException("offset", String.Format("The specified offset ({0}) relative to the current position ({1}) is beyond the length of the data ({2}).", offset, this.index, this.data.Length));
            }

            //assign index
            if (origin == SeekOrigin.Begin)
                this.index = offset;
            else if (origin == SeekOrigin.End)
                this.index = (this.data.Length - offset);
            else if (origin == SeekOrigin.Current)
                this.index += offset;
        }
        #endregion


        #region Helper Methods
        /// <summary>Peeks at the character specified</summary>
        /// <param name="peekIndex">Index of the characer at which to peek</param>
        /// <returns>An integer representing the character to be peeked at, or -1 if no such character available.</returns>
        protected Int32 PeekAt(Int32 peekIndex)
        {
            Int32 peekCharacter = -1;

            if (this.data != null && peekIndex < this.data.Length)
                peekCharacter = Convert.ToInt32(this.data[peekIndex]);

            return peekCharacter;
        }
        #endregion
    }
}