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
    /// <typeparam name="TBlock">The type of the blocks inside the patch.</typeparam>
    public abstract class Patch<TBlock> : IPatch<TBlock> where TBlock : IPatchBlock
    {
        /// <summary>
        /// The stream of the patch.
        /// </summary>
        private Stream baseStream = new MemoryStream();

        /// <summary>
        /// The blocks of the patch.
        /// </summary>
        private List<TBlock> blocks = new List<TBlock>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class.
        /// </summary>
        public Patch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class with a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the patch-file.</param>
        public Patch(string fileName) : this(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class with a stream.
        /// </summary>
        /// <param name="stream">The stream of the patch.</param>
        public Patch(Stream stream)
        {
            stream.CopyTo(BaseStream);
            BaseStream.Position = 0;
            if (BaseStream.Length > 0)
            {
                Analyze();
            }
        }

        /// <summary>
        /// Gets or sets the stream of the patch.
        /// </summary>
        public Stream BaseStream
        {
            get
            {
                return baseStream;
            }

            set
            {
                baseStream = value;
            }
        }

        /// <summary>
        /// Gets or sets or sets the buffer-size of the patch.
        /// </summary>
        public int BufferSize { get; set; } = 1024 * 40; /* 40 KB */

        /// <summary>
        /// Gets the header of the patch.
        /// </summary>
        public virtual byte[] Header
        {
            get
            {
                return new byte[] { };
            }
        }

        /// <summary>
        /// Gets the footer of the patch.
        /// </summary>
        public virtual byte[] Footer
        {
            get
            {
                return new byte[] { };
            }
        }

        /// <summary>
        /// Gets a value indicating whether the end of the content is reached.
        /// </summary>
        public abstract bool EndOfContent { get; }

        /// <summary>
        /// Gets or sets the blocks of the patch.
        /// </summary>
        public virtual List<TBlock> Blocks
        {
            get
            {
                return blocks;
            }

            set
            {
                blocks = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="BinaryReader"/> that contains the <see cref="BaseStream"/>.
        /// </summary>
        protected BinaryReader Reader
        {
            get
            {
                return new BinaryReader(BaseStream, Encoding.UTF8, true);
            }
        }

        /// <summary>
        /// Gets a <see cref="BinaryWriter"/> that contains the <see cref="BaseStream"/>.
        /// </summary>
        protected BinaryWriter Writer
        {
            get
            {
                return new BinaryWriter(BaseStream, Encoding.UTF8, true);
            }
        }
        
        /// <summary>
        /// Gets the Block at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block to fetch.</param>
        /// <returns>The block inside the patch at the specified index.</returns>
        public TBlock this[int index] => throw new NotImplementedException();

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        public virtual void Apply(Stream baseStream, Stream outputStream)
        {
            int size = 0;
            List<TBlock> blocksToApply = new List<TBlock>();

            outputStream.SetLength(baseStream.Length);
            
            using (BinaryReader reader = new BinaryReader(baseStream, Encoding.UTF8, true))
            using (BinaryWriter writer = new BinaryWriter(outputStream, Encoding.UTF8, true))
            {
                foreach (TBlock block in Blocks)
                {
                    blocksToApply.Add(block);
                    size = (int)((block.Position - outputStream.Position) + block.Size);

                    if (size > BufferSize)
                    {
                        Apply(size, reader, writer, blocksToApply.ToArray());
                        blocksToApply.Clear();
                        size = 0;
                    }
                }

                Apply(size, reader, writer, blocksToApply.ToArray());
                baseStream.CopyTo(outputStream);
            }
        }

        /// <summary>
        /// Creates the patch based on the difference between two files.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the file to save the patch to.</param>
        /// <param name="baseFileName">The fully qualified name of the untouched file.</param>
        /// <param name="modFileName">The fully qualified name of the modified file.</param>
        public virtual void Create(string fileName, string baseFileName, string modFileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (Stream baseStream = File.Open(baseFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (Stream modStream = File.Open(modFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Create(stream, baseStream, modStream);
            }
        }

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="stream">The stream to save the patch to.</param>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        public virtual void Create(Stream stream, Stream baseStream, Stream modStream)
        {
            Create(baseStream, modStream);
            Save(stream);
        }

        /// <summary>
        /// Creates the patch based on the difference between two files.
        /// </summary>
        /// <param name="baseFileName">The fully qualified name of the untouched file.</param>
        /// <param name="modFileName">The fully qualified name of the modified file.</param>
        public virtual void Create(string baseFileName, string modFileName)
        {
            using (FileStream basefile = File.Open(baseFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream modfile = File.Open(modFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Create(
                    basefile,
                    modfile);
            }
        }

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        public virtual void Create(Stream baseStream, Stream modStream)
        {
            BlockInfo<TBlock>[] blocks = FindBlocks(baseStream, modStream);
            WriteHeader(baseStream, modStream);
            WriteBody(baseStream, modStream, blocks);
            WriteFooter(baseStream, modStream);
        }

        /// <summary>
        /// Writes the patch to a file.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the file to save the patch to.</param>
        public virtual void Save(string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Writes the patch to a stream.
        /// </summary>
        /// <param name="stream">The stream to write the file to.</param>
        public virtual void Save(Stream stream)
        {
            BaseStream.Position = 0;
            BaseStream.CopyTo(stream);
        }

        /// <summary>
        /// Analyzes the entire patch.
        /// </summary>
        protected virtual void Analyze()
        {
            AnalyzeHeader();
            AnalyzeBody();
            AnalyzeFooter();
        }

        /// <summary>
        /// Analyzes the header of the patch.
        /// </summary>
        protected virtual void AnalyzeHeader()
        {
            using (BinaryReader reader = Reader)
            {
                byte[] header = reader.ReadBytes(Header.Length);
                if (!header.SequenceEqual(Header))
                {
                    throw new InvalidDataException($"Unexcepted file-header: {Encoding.ASCII.GetString(header)}.");
                }
            }
        }

        /// <summary>
        /// Analyzes the entire body of the patch.
        /// </summary>
        protected virtual void AnalyzeBody()
        {
            while (!EndOfContent)
            {
                Blocks.Add(ReadBlock());
            }

            Blocks = Blocks.OrderBy(block => block.Position).ToList();
        }

        /// <summary>
        /// Analyzes the footer of the patch.
        /// </summary>
        protected virtual void AnalyzeFooter()
        {
        }

        /// <summary>
        /// Reads the next <see cref="TBlock"/>.
        /// </summary>
        /// <returns>The next <see cref="TBlock"/> inside the patch.</returns>
        protected abstract TBlock ReadBlock();

        /// <summary>
        /// Applies a set of blocks.
        /// </summary>
        /// <param name="size">The size of all blocks.</param>
        /// <param name="baseReader">The <see cref="BinaryReader"/> that contains the untouched stream.</param>
        /// <param name="outputWriter">The <see cref="BinaryWriter"/> that contains the stream to write the patched data to.</param>
        /// <param name="blocks">The <see cref="PatchBlock"/>s to apply.</param>
        protected virtual void Apply(int size, BinaryReader baseReader, BinaryWriter outputWriter, TBlock[] blocks)
        {
            {
                long position = outputWriter.BaseStream.Position;
                foreach (TBlock block in blocks)
                {
                    block.Position -= position;
                }
            }

            using (MemoryStream inputBuffer = new MemoryStream())
            using (MemoryStream outputBuffer = new MemoryStream())
            {
                long position = outputWriter.BaseStream.Position;
                if (baseReader.BaseStream.Position < baseReader.BaseStream.Length)
                {
                    byte[] data = baseReader.ReadBytes(size);
                    inputBuffer.Write(data, 0, data.Length);
                    inputBuffer.Position = 0;
                    inputBuffer.CopyTo(outputBuffer);
                    inputBuffer.Position = outputBuffer.Position = 0;
                }

                foreach (TBlock block in blocks)
                {
                    using (BinaryReader bufferReader = new BinaryReader(inputBuffer, Encoding.UTF8, true))
                    using (BinaryWriter bufferWriter = new BinaryWriter(outputBuffer, Encoding.UTF8, true))
                    {
                        BaseStream.Position = block.Offset;
                        inputBuffer.Position = outputBuffer.Position = block.Position;
                        Apply(bufferReader, bufferWriter, block);
                    }
                }

                outputBuffer.Position = 0;
                outputBuffer.CopyTo(outputWriter.BaseStream);
            }
        }

        /// <summary>
        /// Applies a block.
        /// </summary>
        /// <param name="baseReader">The <see cref="BinaryReader"/> that contains the untouched stream.</param>
        /// <param name="outputWriter">The <see cref="BinaryWriter"/> that contains the stream to write the patched data to.</param>
        /// <param name="block">The <see cref="PatchBlock"/> to apply.</param>
        protected abstract void Apply(BinaryReader baseReader, BinaryWriter outputWriter, TBlock block);

        /// <summary>
        /// Looks for different sequences inside the untouched stream and the modified stream.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <returns>The <see cref="BlockInfo"/>s describing the different sequences.</returns>
        protected abstract BlockInfo<TBlock>[] FindBlocks(Stream baseStream, Stream modStream);

        /// <summary>
        /// Writes the header to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected virtual void WriteHeader(Stream baseStream, Stream modStream)
        {
            using (BinaryWriter writer = Writer)
            {
                writer.Write(Header);
            }
        }

        /// <summary>
        /// Writes the blocks to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <param name="blocks">The <see cref="BlockInfo"/>s to write to the patch.</param>
        protected virtual void WriteBody(Stream baseStream, Stream modStream, BlockInfo<TBlock>[] blocks)
        {
            foreach (BlockInfo<TBlock> block in blocks)
            {
                WriteBlock(baseStream, modStream, block);
            }
        }

        /// <summary>
        /// Writes a block to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <param name="blockInfo">The <see cref="BlockInfo{TBlock}"/> to write to the patch.</param>
        protected abstract void WriteBlock(Stream baseStream, Stream modStream, BlockInfo<TBlock> blockInfo);

        /// <summary>
        /// Writes the footer to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected virtual void WriteFooter(Stream baseStream, Stream modStream)
        {
            using (BinaryWriter writer = Writer)
            {
                writer.Write(Footer);
            }
        }

        #region IDisposable Support
        #region Component Designer generated code

        /// <summary>
        /// Dient zur Erkennung redundanter Aufrufe.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Patch"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Patch"/> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see cref="true"/> to release both managed and unmanaged resources; <see cref="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    BaseStream.Dispose();
                }

                blocks = null;
                disposedValue = true;
            }
        }
        #endregion
        #endregion
    }
}