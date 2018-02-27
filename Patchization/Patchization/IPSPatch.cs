using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a Patch which uses the International Patching System.
    /// </summary>
    public class IPSPatch : Patch<RLEBlock>
    {
        /// <summary>
        /// The value of the EOF-bytes.
        /// </summary>
        private int eofValue = 0x454F46;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSPatch"/> class.
        /// </summary>
        public IPSPatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSPatch"/> class with a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the patch-file.</param>
        public IPSPatch(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSPatch"/> class with a stream.
        /// </summary>
        /// <param name="stream">The stream of the patch.</param>
        public IPSPatch(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Gets the header of the patch.
        /// </summary>
        public override byte[] Header
        {
            get
            {
                return Encoding.ASCII.GetBytes("PATCH");
            }
        }

        /// <summary>
        /// Gets the footer of the patch.
        /// </summary>
        public override byte[] Footer
        {
            get
            {
                return Encoding.ASCII.GetBytes("EOF");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the end of the content is reached.
        /// </summary>
        public override bool EndOfContent
        {
            get
            {
                bool result;
                long position = BaseStream.Position;

                using (BinaryReader reader = Reader)
                {
                    result = reader.ReadBytes(Footer.Length).SequenceEqual(Footer);
                }

                BaseStream.Position = position;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the size of the patched stream.
        /// </summary>
        public long? OutputSize
        { get; set; } = null;

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        public override void Apply(Stream baseStream, Stream outputStream)
        {
            if (OutputSize == null)
            {
                OutputSize = baseStream.Length;
            }

            base.Apply(baseStream, outputStream);
            outputStream.SetLength(OutputSize.Value);
        }

        /// <summary>
        /// Analyzes the footer of the patch.
        /// </summary>
        protected override void AnalyzeFooter()
        {
            base.AnalyzeFooter();
            BaseStream.Position += 3;
            if (BaseStream.Position <= BaseStream.Length - 3)
            {
                using (BinaryReader reader = Reader)
                {
                    OutputSize = GetInt32();
                }
            }
        }

        /// <summary>
        /// Reads the next <see cref="RLEBlock"/>.
        /// </summary>
        /// <returns>The next <see cref="RLEBlock"/> inside the patch.</returns>
        protected override RLEBlock ReadBlock()
        {
            RLEBlock block = new RLEBlock();

            using (BinaryReader reader = Reader)
            {
                block.Position = GetInt32();
                if ((block.Size = GetInt16()) == 0)
                {
                    block.IsRLE = true;
                    block.Size = GetInt16();
                }
            }

            block.Offset = BaseStream.Position;

            if (block.IsRLE)
            {
                BaseStream.Position++;
            }
            else
            {
                BaseStream.Position += block.Size;
            }

            return block;
        }

        /// <summary>
        /// Applies a set of blocks.
        /// </summary>
        /// <param name="size">The size of all blocks.</param>
        /// <param name="baseReader">The <see cref="BinaryReader"/> that contains the untouched stream.</param>
        /// <param name="outputWriter">The <see cref="BinaryWriter"/> that contains the stream to write the patched data to.</param>
        /// <param name="blocks">The <see cref="RLEBlock"/>s to apply.</param>
        protected override void Apply(int size, BinaryReader baseReader, BinaryWriter outputWriter, RLEBlock[] blocks)
        {
            if (outputWriter.BaseStream.Position < OutputSize)
            {
                base.Apply(size, baseReader, outputWriter, blocks);
            }
        }

        /// <summary>
        /// Applies a block.
        /// </summary>
        /// <param name="baseReader">The <see cref="BinaryReader"/> that contains the untouched stream.</param>
        /// <param name="outputWriter">The <see cref="BinaryWriter"/> that contains the stream to write the patched data to.</param>
        /// <param name="block">The <see cref="RLEBlock"/> to apply.</param>
        protected override void Apply(BinaryReader baseReader, BinaryWriter outputWriter, RLEBlock block)
        {
            using (BinaryReader reader = Reader)
            {
                if (!block.IsRLE)
                {
                    outputWriter.Write(reader.ReadBytes(block.Size));
                }
                else
                {
                    outputWriter.Write(Enumerable.Repeat(reader.ReadByte(), block.Size).ToArray());
                }
            }
        }

        /// <summary>
        /// Looks for different sequences inside the untouched stream and the modified stream.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <returns>The <see cref="BlockInfo"/>s describing the different sequences.</returns>
        protected override BlockInfo<RLEBlock>[] FindBlocks(Stream baseStream, Stream modStream)
        {
            List<BlockInfo<RLEBlock>> blocks = new List<BlockInfo<RLEBlock>>();
            using (BinaryReader baseReader = new BinaryReader(baseStream, Encoding.UTF8, true))
            using (BinaryReader modReader = new BinaryReader(modStream, Encoding.UTF8, true))
            {
                List<byte> baseSequence = new List<byte>();
                List<byte> modSequence = new List<byte>();
                int fixOffset = 0x454F46;

                long length = Math.Min(baseStream.Length, modStream.Length);

                for (long position = baseStream.Position; position < length; position++)
                {
                    byte baseByte = baseReader.ReadByte();
                    byte modByte = modReader.ReadByte();

                    if (baseByte == modByte)
                    {
                        if (baseStream.Position == fixOffset)
                        {
                            byte baseFix = baseByte;
                            byte modFix = modByte;
                            baseByte = baseReader.ReadByte();
                            modByte = modReader.ReadByte();
                            position++;

                            if (baseByte != modByte)
                            {
                                baseSequence.Add(baseFix);
                                baseSequence.Add(baseByte);

                                modSequence.Add(modFix);
                                modSequence.Add(modByte);
                            }
                        }

                        if (modSequence.Count > 0)
                        {
                            blocks.AddRange(FindBlocks(position - baseSequence.Count, baseSequence.ToArray(), modSequence.ToArray()));
                            baseSequence.Clear();
                            modSequence.Clear();
                        }
                    }
                    else
                    {
                        baseSequence.Add(baseByte);
                        modSequence.Add(modByte);
                    }
                }

                if (modSequence.Count > 0)
                {
                    blocks.AddRange(FindBlocks(baseStream.Length - baseSequence.Count, baseSequence.ToArray(), modSequence.ToArray()));
                    baseSequence.Clear();
                    modSequence.Clear();
                }
            }

            return blocks.ToArray();
        }

        /// <summary>
        /// Looks for different sequences inside the untouched stream and the modified stream.
        /// </summary>
        /// <param name="position">The position of the sequences.</param>
        /// <param name="baseSequence">The byte-sequence of the untouched stream.</param>
        /// <param name="modSequence">The byte-sequence of the modified stream.</param>
        /// <returns>The <see cref="BlockInfo"/>s describing the different sequences.</returns>
        protected virtual BlockInfo<RLEBlock>[] FindBlocks(long position, byte[] baseSequence, byte[] modSequence)
        {
            List<BlockInfo<RLEBlock>> blocks = new List<BlockInfo<RLEBlock>>();
            {
                int size = 0;
                int rleSize = 1;
                int startposition = 0;

                for (int i = 1; i < modSequence.Length; i++)
                {
                    if (
                        modSequence[i] == modSequence[i - 1] &&
                        (position + i != eofValue - 1 || modSequence.Length < i + 1 || modSequence[i] == modSequence[i + 1]) &&
                        (position + i != eofValue || rleSize > 1))
                    {
                        rleSize++;
                    }
                    else
                    {
                        if (rleSize >= 5)
                        {
                            if (size > 0)
                            {
                                blocks.Add(
                                    new BlockInfo<RLEBlock>(
                                        new RLEBlock(0, position + startposition, size, false),
                                        baseSequence.Skip(startposition).Take(size).ToArray(),
                                        modSequence.Skip(startposition).Take(size).ToArray()));
                                startposition += size;
                                size = 0;
                            }

                            blocks.Add(
                                new BlockInfo<RLEBlock>(
                                    new RLEBlock(0, position + startposition, rleSize, true),
                                    baseSequence.Skip(startposition).Take(rleSize).ToArray(),
                                    modSequence.Skip(startposition).Take(rleSize).ToArray()));
                        }
                        else
                        {
                            size += rleSize;
                        }

                        rleSize = 1;
                    }
                }

                if (rleSize >= 5)
                {
                    if (startposition < modSequence.Length - rleSize)
                    {
                        size = (modSequence.Length - rleSize) - startposition;
                        blocks.Add(
                            new BlockInfo<RLEBlock>(
                                new RLEBlock(0, position + startposition, size, false),
                                baseSequence.Skip(startposition).Take(size).ToArray(),
                                modSequence.Skip(startposition).Take(size).ToArray()));
                        startposition += size;
                        size = 0;
                    }
                    
                    blocks.Add(
                        new BlockInfo<RLEBlock>(
                            new RLEBlock(0, position + startposition, rleSize, true),
                            baseSequence.Skip(startposition).Take(rleSize).ToArray(),
                            modSequence.Skip(startposition).Take(rleSize).ToArray()));
                }
                else
                {
                    size += rleSize;
                    blocks.Add(
                        new BlockInfo<RLEBlock>(
                            new RLEBlock(0, position + startposition, size, false),
                            baseSequence.Skip(startposition).Take(size).ToArray(),
                            modSequence.Skip(startposition).Take(size).ToArray()));
                }
            }

            return blocks.ToArray();
        }

        /// <summary>
        /// Writes a block to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <param name="blockInfo">The <see cref="BlockInfo"/> to write to the patch.</param>
        protected override void WriteBlock(Stream baseStream, Stream modStream, BlockInfo<RLEBlock> blockInfo)
        {
            if (blockInfo.Block.Size > 0xFFFF)
            {
                for (int i = 0; i < blockInfo.Block.Size; i += 0xFFFF)
                {
                    int size = Math.Min(0xFFFF, blockInfo.ModSequence.Skip(i).ToArray().Length);
                    WriteBlock(
                        baseStream,
                        modStream,
                        new BlockInfo<RLEBlock>(
                            new RLEBlock(blockInfo.Block.Offset, blockInfo.Block.Position, size, blockInfo.Block.IsRLE),
                            blockInfo.BaseSequence.Skip(i).Take(size).ToArray(),
                            blockInfo.ModSequence.Skip(i).Take(size).ToArray()));
                }
            }

            WriteInt32((int)blockInfo.Block.Position);
            if (blockInfo.Block.IsRLE)
            {
                WriteInt16(0);
            }

            WriteInt16(blockInfo.Block.Size);

            using (BinaryWriter writer = Writer)
            {
                if (blockInfo.Block.IsRLE)
                {
                    writer.Write(blockInfo.ModSequence.First());
                }
                else
                {
                    writer.Write(blockInfo.ModSequence);
                }
            }
        }

        /// <summary>
        /// Writes the footer to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected override void WriteFooter(Stream baseStream, Stream modStream)
        {
            base.WriteFooter(baseStream, modStream);
            if (baseStream.Length != modStream.Length)
            {
                WriteInt32((int)modStream.Length);
            }
        }

        /// <summary>
        /// Reads a set of bytes from the patch and converts them to an integer.
        /// </summary>
        /// <returns>Returns the converted integer.</returns>
        protected int GetInt32()
        {
            using (BinaryReader reader = Reader)
            {
                List<byte> list = reader.ReadBytes(3).ToList();
                list.Reverse();
                list.Add(0x00);
                return BitConverter.ToInt32(list.ToArray(), 0);
            }
        }

        /// <summary>
        /// Converts an integer to a byte-array and writes it to the patch.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        protected void WriteInt32(int value)
        {
            using (BinaryWriter writer = Writer)
            {
                List<byte> list = BitConverter.GetBytes(value).ToList();
                list.RemoveAt(list.Count - 1);
                list.Reverse();

                writer.Write(list.ToArray());
            }
        }

        /// <summary>
        /// Reads a set of bytes from the patch and converts them to an integer.
        /// </summary>
        /// <returns>Returns the converted integer.</returns>
        protected int GetInt16()
        {
            using (BinaryReader reader = Reader)
            {
                List<byte> list = reader.ReadBytes(2).ToList();
                list.Reverse();
                return BitConverter.ToUInt16(list.ToArray(), 0);
            }
        }

        /// <summary>
        /// Converts an integer to a byte-array and writes it to the patch.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        protected void WriteInt16(int value)
        {
            using (BinaryWriter writer = Writer)
            {
                List<byte> list = BitConverter.GetBytes((ushort)value).ToList();
                list.Reverse();

                writer.Write(list.ToArray());
            }
        }
    }
}
