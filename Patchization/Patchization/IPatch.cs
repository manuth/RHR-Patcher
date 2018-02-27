using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a patch.
    /// </summary>
    public interface IPatch : IDisposable
    {
        /// <summary>
        /// Gets or sets the stream of the patch.
        /// </summary>
        Stream BaseStream { get; set; }

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        void Apply(Stream baseStream, Stream outputStream);

        /// <summary>
        /// Creates the patch based on the difference between two files.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the file to save the patch to.</param>
        /// <param name="baseFileName">The fully qualified name of the untouched file.</param>
        /// <param name="modFileName">The fully qualified name of the modified file.</param>
        void Create(string fileName, string baseFileName, string modFileName);

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="stream">The stream to save the patch to.</param>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        void Create(Stream stream, Stream baseStream, Stream modStream);

        /// <summary>
        /// Creates the patch based on the difference between two files.
        /// </summary>
        /// <param name="baseFileName">The fully qualified name of the untouched file.</param>
        /// <param name="modFileName">The fully qualified name of the modified file.</param>
        void Create(string baseFileName, string modFileName);

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        void Create(Stream baseStream, Stream modStream);

        /// <summary>
        /// Writes the patch to a file.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the file to save the patch to.</param>
        void Save(string fileName);

        /// <summary>
        /// Writes the patch to a stream.
        /// </summary>
        /// <param name="stream">The stream to write the file to.</param>
        void Save(Stream stream);
    }
}
