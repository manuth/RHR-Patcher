using System;
using System.Linq;
using System.Runtime.Serialization;

namespace ManuTh.Patchization
{
    [Serializable]
    internal class ChecksumException : Exception
    {
        /// <summary>
        /// The computed hash.
        /// </summary>
        private byte[] hash = new byte[] { };

        /// <summary>
        /// The excepted hashes.
        /// </summary>
        private byte[][] validHashes = new byte[][] { };

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumException"/> class.
        /// </summary>
        public ChecksumException()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumException"/> with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ChecksumException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumException"/> class with a specified hash and specified excepted hashes.
        /// </summary>
        /// <param name="hash">The computed hash.</param>
        /// <param name="validHashes">The excepted hashes.</param>
        public ChecksumException(byte[] hash, params byte[][] validHashes) : this(
            $"Unexcepted hash {string.Join(string.Empty, hash.ToList().ConvertAll(part => part.ToString("X2")))}.\n" +
            $"Excepting {string.Join(" or ", validHashes.ToList().ConvertAll(validHash => string.Join(string.Empty, validHash.ToList().ConvertAll(part => part.ToString("X2")))))}"
        )
        {
            this.hash = hash;
            this.validHashes = validHashes;
        }

        /// <summary>
        /// Initializes a new instance of the ChecksumException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ChecksumException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ChecksumException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}