using System;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Wraps an Array of type Byte in accessing code to read or write the data by specified groups of bits</summary>
    /// <remarks>When reading, the first byte's bits are the least significant bits, and succeeding bytes' bits are more significant</remarks>
    public class BitStream
    {
        #region Fields
        /// <summary>Represents the position within the bitstream</summary>
        protected Int64 position;

        /// <summary>Binary byte array</summary>
        protected Byte[] data;
        #endregion

        #region Properties
        /// <summary>Represents the position within the bitstream</summary>
        public Int64 Position
        {
            get
            {
                if (this.position < 0)
                    throw new IndexOutOfRangeException(String.Format("The member 'position' is out of range, being less than 0: value of {0} exists.", this.position));

                return this.position;
            }
            set { this.position = value; }
        }

        /// <summary>Binary byte array</summary>
        public Byte[] Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                this.Position = 0;
            }
        }

        /// <summary>Returns the length of this BitStream</summary>
        public Int64 Length
        {
            get
            {
                Int64 value = 0;
                if (this.data != null)
                    value = this.data.Length * 8;
                
                return value;
            }
        }

        /// <summary>etermines if the BitStream's position is at or beyond the last bit.</summary>
        public Boolean EndOfStream
        {
            get { return position > (Length - 1); }
        }

        /// <summary>Retrieves the number of bits yet remaining in the BitBuffer</summary>
        public Int64 BitsRemaining
        {
            get { return this.Length - this.Position; }
        }
        #endregion

        /// <summary>Default constructor</summary>
        public BitStream()
        {
            this.Position = -1;
            this.data = null;
        }

        /// <summary>Default constructor</summary>
        public BitStream(Byte[] dataSource)
        {
            this.Data = dataSource;
        }

        /// <summary>Gets the remaining bits of the current byte, and returns them as a Int64</summary>
        /// <param name="shift">Output value indicating the number of bits shifted</param>
        /// <returns>An Int64 representing the bits left in the current Byte within Data based off of position</returns>
        protected UInt64 GetPositionBits(out Int32 shift)
        {
            //based on the position, we only have x number of bits available, so shift lower bits away
            shift = Convert.ToInt32(this.Position % 8);   //has to be Int32 due to CLR
            UInt64 temp = Convert.ToUInt64(this.Data[this.Position / 8]) >> shift;
            shift = (shift == 0 ? 0 : 8 - shift);
            //temp now has x number of bits, based on position.

            return temp;
        }

        #region Conversion Methods
        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A Byte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual Byte ReadByte(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 8)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for a Byte (8 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((Byte)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual SByte ReadSByte(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 8)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for a signed Byte (8 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((SByte)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual UInt16 ReadUInt16(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 16)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for an unsigned Int16 (16 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((UInt16)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual Int16 ReadInt16(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 16)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for a signed Int16 (16 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((Int16)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual UInt32 ReadUInt32(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 32)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for an unsigned Int32 (32 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((UInt32)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual Int32 ReadInt32(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 32)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for a signed Int32 (32 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((Int32)GetBits(bitCount));
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual UInt64 ReadUInt64(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 64)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for an unsigned Int64 (64 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return GetBits(bitCount);
        }

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A SByte containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>Intenrally, uses UInt64 due to CLR restrictions on the shift operators</remarks>
        public virtual Int64 ReadInt64(Int32 bitCount)
        {
            if (bitCount < 0 || bitCount > 64)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for a signed Int64 (64 bits): value of {0} exists.", bitCount));

            //unchecked binary reinterpretation of memory
            return unchecked((Int64)GetBits(bitCount));
        }
        #endregion

        /// <summary>Reads the specified number of bits from this BitStream</summary>
        /// <param name="bitCount">Count of bits to read</param>
        /// <returns>A UInt64 containing the bits read, aligned to the least significant bit</returns>
        /// <remarks>
        ///     Internally, uses UInt64 due to CLR restrictions on the shift operators.
        ///     Protected, as the return value is intended to be casted between various types.
        ///     Does no error checking, as this method is meant to be wrapped by others.
        /// </remarks>
        protected virtual UInt64 GetBits(Int32 bitCount)
        {            
            if ((this.Position + bitCount) > this.Length)
                throw new ArgumentOutOfRangeException("bitCount", bitCount, String.Format("The parameter 'bitCount' is out of range for reading a byte from the stream: reading {0} bits from index of {1} when the length of the BitStream is {2} .", bitCount, this.Position, this.Length));

            Int32 shift, remainingBits = bitCount, bitsRead = 0, previousTotal = 0;
            UInt64 temp, value = 0;
            do
            {
                temp = this.GetPositionBits(out shift);

                if (remainingBits > 0 && shift == 0) //presently byte-aligned
                    shift = 8;

                bitsRead += (shift < remainingBits) ? shift : remainingBits;
                this.Position += (shift < remainingBits) ? shift : remainingBits;

                UInt64 mask = ~(0xFFFFFFFFFFFFFFFF << (shift < remainingBits ? shift : remainingBits));
                temp &= mask;
                temp <<= previousTotal;
                    
                previousTotal += shift;
                remainingBits = shift < remainingBits ? remainingBits - shift : 0;
                    
                value |= temp;

            } while (remainingBits > 0);

            return value;
        }
    }
}