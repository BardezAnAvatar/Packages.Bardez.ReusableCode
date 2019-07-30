using System;
using System.IO;
using System.Runtime.Remoting;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Represents a sub-stream of an existing stream</summary>
    public class SubStream : Stream
    {
        #region Fields
        protected Int64 StartPosition { get; set; }
        protected Stream Source { get; set; }
        #endregion


        #region Properties
        /// <summary>Exposes the stream's position</summary>
        public override Int64 Position
        {
            get { return Source.Position - this.StartPosition; }
            set { this.Source.Position = value + this.StartPosition; }
        }
        #endregion


        #region Construction
        /// <summary>Definition constructor</summary>
        /// <param name="source">Source stream to read from</param>
        public SubStream(Stream source) : this(source, source.Position) { }

        /// <summary>Definition constructor</summary>
        /// <param name="source">Source stream to read from</param>
        /// <param name="startPosition">Starting position of the substream within the source stream</param>
        public SubStream(Stream source, Int64 startPosition)
        {
            this.Source = source;
            this.StartPosition = startPosition;
        }
        #endregion


        #region Stream overridden methods
        public override Boolean CanRead
        {
            get { return this.Source.CanRead; }
        }

        public override Boolean CanSeek
        {
            get { return this.Source.CanSeek; }
        }

        public override Boolean CanTimeout
        {
            get { return this.Source.CanTimeout; }
        }

        public override Boolean CanWrite
        {
            get { return this.Source.CanWrite; }
        }

        public override Int64 Length
        {
            get { return this.Source.Length - this.StartPosition; }
        }

        public override Int32 ReadTimeout
        {
            get { return this.Source.ReadTimeout; }
            set { this.Source.ReadTimeout = value; }
        }

        public override Int32 WriteTimeout
        {
            get { return this.Source.WriteTimeout; }
            set { this.Source.WriteTimeout = value; }
        }
        #endregion


        #region Stream overridden methods
        public override IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)
        {
            return this.Source.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state)
        {
            return this.Source.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            this.Source.Close();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return this.Source.CreateObjRef(requestedType);
        }

        public override Int32 EndRead(IAsyncResult asyncResult)
        {
            return this.Source.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.Source.EndWrite(asyncResult);
        }

        public override Boolean Equals(Object obj)
        {
            return this.Source.Equals(obj);
        }

        public override void Flush()
        {
            this.Source.Flush();
        }

        public override Int32 GetHashCode()
        {
            return this.Source.GetHashCode();
        }

        public override Object InitializeLifetimeService()
        {
            return this.Source.InitializeLifetimeService();
        }

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return this.Source.Read(buffer, offset, count);
        }

        public override Int32 ReadByte()
        {
            return this.Source.ReadByte();
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            return this.Source.Seek(offset + this.StartPosition, origin) - this.StartPosition;
        }

        public override void SetLength(Int64 value)
        {
            this.Source.SetLength(value);
        }

        public override String ToString()
        {
            return this.Source.ToString();
        }

        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            this.Source.Write(buffer, offset, count);
        }

        public override void WriteByte(Byte value)
        {
            this.Source.WriteByte(value);
        }
        #endregion
    }
}