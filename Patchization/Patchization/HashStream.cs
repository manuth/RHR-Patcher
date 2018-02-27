using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a stream which provides the functionality of hash-calculation.
    /// </summary>
    /// <typeparam name="TAlgorithm">The <see cref="HashAlgorithm"/> to calculate.</typeparam>
    public class HashStream<TAlgorithm> : Stream where TAlgorithm : HashAlgorithm, new()
    {
        /// <summary>
        /// The buffer of the stream.
        /// </summary>
        List<byte> buffer = new List<byte>();

        /// <summary>
        /// The <see cref="HashAlgorithm"/> of the stream.
        /// </summary>
        private TAlgorithm hashAlgorithm;

        /// <summary>
        /// A value indicating whether the stream is initialized.
        /// </summary>
        private bool initialized = true;

        /// <summary>
        /// The difference to the end of the latest transformed block.
        /// </summary>
        private int difference = 0;

        /// <summary>
        /// A value indicating whether the hash-calculation's finished.
        /// </summary>
        private bool hasFlushedFinalBlock = false;

        /// <summary>
        /// The stream to read from/write to.
        /// </summary>
        private Stream baseStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashStream{TAlgorithm}"/> class.
        /// </summary>
        /// <param name="stream">The stream whose hash is to be calculated.</param>
        public HashStream(Stream stream) : this(stream, new TAlgorithm())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashStream{TAlgorithm}"/> class.
        /// </summary>
        /// <param name="stream">The stream whose hash is to be calculated.</param>
        /// <param name="hashAlgorithm">An instance of the <see cref="TAlgorithm"/> to calculate the hash.</param>
        public HashStream(Stream stream, TAlgorithm hashAlgorithm)
        {
            baseStream = stream;
            this.hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Gets the size of the buffer of the stream.
        /// </summary>
        protected virtual int BufferSize
        {
            get
            {
                return (1024 * 4) * 20; // 4 KB * 20 => 80 KB
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                return baseStream.CanSeek;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return baseStream.CanRead;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return baseStream.CanWrite;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        public override long Length
        {
            get
            {
                return baseStream.Length;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get
            {
                return baseStream.Position;
            }

            set
            {
                int difference = (int)(value - baseStream.Position);

                if (difference > 0)
                {
                    Read(new byte[difference], 0, difference);
                }
                else if (difference < 0 && !initialized)
                {
                    this.difference = difference * -1;
                }
                baseStream.Position = value;
            }
        }

        /// <summary>
        /// Gets the stream wrapped by the <see cref="HashStream{TAlgorithm}"/>.
        /// </summary>
        public Stream BaseStream
        {
            get
            {
                return baseStream;
            }
        }

        /// <summary>
        /// Gets the computed hash of the stream.
        /// </summary>
        public byte[] Hash
        {
            get
            {
                return hashAlgorithm.Hash;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the hash's been calculated.
        /// </summary>
        public bool HasFlushedFinalBlock
        {
            get
            {
                return hasFlushedFinalBlock;
            }
        }

        /// <summary>
        /// Initializes the implementation of the <see cref="HashStream{TAlgorithm}"/>-class.
        /// </summary>
        public virtual void Initialize()
        {
            hashAlgorithm.Initialize();
            difference = 0;
            initialized = true;
            buffer.Clear();
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = baseStream.Read(buffer, offset, count);
            this.buffer.AddRange(buffer.Skip(offset + difference).Take(result - difference));
            initialized = false;

            if (this.buffer.Count > BufferSize)
            {
                byte[] array = this.buffer.ToArray();
                this.buffer.Clear();
                hashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
            }

            return result;
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long position = Position;
            long result = baseStream.Seek(offset, origin);
            int difference = (int)(position - Position);

            if (difference > 0 && !initialized)
            {
                this.difference = difference;
            }

            return result;
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.buffer.AddRange(buffer.Skip(offset + difference).Take(count - difference));
            initialized = false;

            if (this.buffer.Count > BufferSize)
            {
                byte[] array = this.buffer.ToArray();
                this.buffer.Clear();
                hashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
            }

            baseStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>The unsigned byte cast to an <see cref="Int32"/>, or -1 if at the end of the stream.</returns>
        public override int ReadByte()
        {
            int result = baseStream.ReadByte();

            if (difference > 0)
            {
                difference--;
            }
            else if (result >= 0)
            {
                buffer.Add((byte)result);
                initialized = false;
            }

            if (buffer.Count > BufferSize)
            {
                byte[] array = buffer.ToArray();
                hashAlgorithm.TransformBlock(array, 0, array.Length, array, 0);
                buffer.Clear();
            }

            return result;
        }

        /// <summary>
        /// Updates the underlying data source or repository with the current state of the buffer, then clears the buffer.
        /// </summary>
        public void FlushFinalBlock()
        {
            byte[] array = buffer.ToArray();
            hashAlgorithm.TransformFinalBlock(array, 0, array.Length);
            hashAlgorithm.Initialize();
            buffer.Clear();
            hasFlushedFinalBlock = true;
        }
    }
}
