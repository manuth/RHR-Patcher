using DamienG.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ManuTh.Patchization.IO;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a Patch which uses the Universal Patching System.
    /// </summary>
    public class UPSPatch : HashPatch<PatchBlock, Crc32>
    {
        /// <summary>
        /// The size of the untouched stream.
        /// </summary>
        private long validBaseLength = 0;

        /// <summary>
        /// The size of the modified stream.
        /// </summary>
        private long validModLength = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="UPSPatch"/> class.
        /// </summary>
        public UPSPatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UPSPatch"/> class with a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the patch-file.</param>
        public UPSPatch(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UPSPatch"/> class with a stream.
        /// </summary>
        /// <param name="stream">The stream of the patch.</param>
        public UPSPatch(Stream stream) : base(stream)
        {
        }

        /// <summary>
        /// Gets the header of the patch.
        /// </summary>
        public override byte[] Header => Encoding.ASCII.GetBytes("UPS1");

        /// <summary>
        /// Gets a value indicating whether the end of the content is reached.
        /// </summary>
        public override bool EndOfContent => !(BaseStream.Position < BaseStream.Length - 12);

        /// <summary>
        /// Returns the size of untouched stream.
        /// </summary>
        public long ValidBaseLength
        {
            get
            {
                return validBaseLength;
            }

            set
            {
                validBaseLength = value;
            }
        }

        /// <summary>
        /// Returns the size of the modified stream.
        /// </summary>
        public long ValidModLength
        {
            get
            {
                return validModLength;
            }

            set
            {
                validModLength = value;
            }
        }

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        public override void Apply(HashStream<Crc32> baseStream, HashStream<Crc32> outputStream)
        {
            if (baseStream.Length != ValidBaseLength && baseStream.Length == ValidModLength)
            {
                long length = ValidBaseLength;
                ValidBaseLength = ValidModLength;
                ValidModLength = length;

                byte[] hash = ValidBaseHash;
                ValidBaseHash = ValidModHash;
                ValidModHash = hash;
            }
            outputStream.SetLength(ValidModLength);
            base.Apply(baseStream, outputStream);
        }

        /// <summary>
        /// Analyzes the header of the patch.
        /// </summary>
        protected override void AnalyzeHeader()
        {
            base.AnalyzeHeader();
            ValidBaseLength = BaseStream.VLEDecode();
            ValidModLength = BaseStream.VLEDecode();
        }

        /// <summary>
        /// Reads the next <see cref="TBlock"/>.
        /// </summary>
        /// <returns>The next <see cref="TBlock"/> inside the patch.</returns>
        protected override PatchBlock ReadBlock()
        {
            PatchBlock block = new PatchBlock()
            {
                Position = BaseStream.VLEDecode(),
                Offset = BaseStream.Position
            };

            using (BinaryReader reader = Reader)
            {
                do
                {
                    block.Size++;
                }
                while (reader.ReadByte() != 0x00);
            }

            PatchBlock lastBlock = Blocks.LastOrDefault();
            block.Position += (lastBlock?.Position + lastBlock?.Size) ?? 0;
            return block;
        }

        /// <summary>
        /// Analyzes the footer of the patch.
        /// </summary>
        protected override void AnalyzeFooter()
        {
            base.AnalyzeFooter();

            using (BinaryReader reader = Reader)
            {
                ValidBaseHash = reader.ReadBytes(4).Reverse().ToArray();
                ValidModHash = reader.ReadBytes(4).Reverse().ToArray();
            }
        }

        /// <summary>
        /// Analyzes the verification-footer.
        /// </summary>
        /// <param name="patchStream">The stream of the patch.</param>
        protected override void AnalyzeVerificationFooter(HashStream<Crc32> patchStream)
        {
            base.AnalyzeVerificationFooter(patchStream);
            patchStream.FlushFinalBlock();

            using (BinaryReader reader = Reader)
            {
                ValidHash = reader.ReadBytes(4).Reverse().ToArray();
            }
        }

        /// <summary>
        /// Applies a block.
        /// </summary>
        /// <param name="baseReader">The <see cref="BinaryReader"/> that contains the untouched stream.</param>
        /// <param name="outputWriter">The <see cref="BinaryWriter"/> that contains the stream to write the patched data to.</param>
        /// <param name="block">The <see cref="PatchBlock"/> to apply.</param>
        protected override void Apply(BinaryReader baseReader, BinaryWriter outputWriter, PatchBlock block)
        {
            IEnumerator<byte> baseEnumerator = ReadSequence(baseReader.BaseStream.ToEnumerable()).GetEnumerator();
            IEnumerator<byte> patchEnumerator = ReadSequence(BaseStream.ToEnumerable()).GetEnumerator();

            for (int i = 0; i < block.Size; i++)
            {
                baseEnumerator.MoveNext();
                patchEnumerator.MoveNext();

                outputWriter.Write((byte)(baseEnumerator.Current ^ patchEnumerator.Current));
            }
        }

        /// <summary>
        /// Reads the sequence returning only zeros after the end is reached.
        /// </summary>
        /// <param name="sequence">The sequence to read.</param>
        /// <returns>An <see cref="IEnumerable{byte}"/> that returns 0x00 after the end is reached.</returns>
        private IEnumerable<byte> ReadSequence(IEnumerable<byte> sequence)
        {
            for (IEnumerator<byte> enumerator = sequence.GetEnumerator(); enumerator.MoveNext(); )
            {
                yield return enumerator.Current;
            }

            while (true)
            {
                yield return 0x00;
            }
        }

        /// <summary>
        /// Verifies the hashes of the streams.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        protected override void Verify(HashStream<Crc32> baseStream, HashStream<Crc32> outputStream)
        {
            if (ValidBaseLength == ValidModLength && !baseStream.Hash.SequenceEqual(ValidBaseHash))
            {
                byte[] hash = ValidBaseHash;
                ValidBaseHash = ValidModHash;
                ValidModHash = hash;
            }
            base.Verify(baseStream, outputStream);
        }

        /// <summary>
        /// Looks for different sequences inside the untouched stream and the modified stream.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <returns>The <see cref="BlockInfo"/>s describing the different sequences.</returns>
        protected override BlockInfo<PatchBlock>[] FindBlocks(Stream baseStream, Stream modStream)
        {
            List<BlockInfo<PatchBlock>> blocks = new List<BlockInfo<PatchBlock>>();
            List<byte> baseSequence = new List<byte>();
            List<byte> modSequence = new List<byte>();
            long position = 0;

            using (BinaryReader baseReader = new BinaryReader(baseStream, Encoding.UTF8, true))
            using (BinaryReader modReader = new BinaryReader(modStream, Encoding.UTF8, true))
            {
                while (baseStream.Position < baseStream.Length && modStream.Position < modStream.Length)
                {
                    int size = Math.Min((int)Math.Min(baseStream.Length - baseStream.Position, modStream.Length - modStream.Position), BufferSize);
                    position = baseStream.Position;

                    byte[] baseBytes = baseReader.ReadBytes(size);
                    byte[] modBytes = modReader.ReadBytes(size);

                    for (int i = 0; i < size; i++)
                    {
                        if (baseBytes[i] == modBytes[i] && modSequence.Count > 0)
                        {
                            blocks.Add(
                                new BlockInfo<PatchBlock>(
                                    new PatchBlock
                                    {
                                        Offset = 0,
                                        Position = (position + i) - modSequence.Count,
                                        Size = modSequence.Count
                                    },
                                    baseSequence.ToArray(),
                                    modSequence.ToArray()));
                            baseSequence.Clear();
                            modSequence.Clear();
                        }
                        else if (baseBytes[i] != modBytes[i])
                        {
                            baseSequence.Add(baseBytes[i]);
                            modSequence.Add(modBytes[i]);
                        }
                    }
                }

                if (modSequence.Count > 0)
                {
                    blocks.Add(
                        new BlockInfo<PatchBlock>(
                            new PatchBlock
                            {
                                Offset = 0,
                                Position = baseStream.Position - baseSequence.Count,
                                Size = baseSequence.Count
                            },
                            baseSequence.ToArray(),
                            modSequence.ToArray()));
                }

            }

            for (int i = blocks.Count - 1; i > 0; i--)
            {
                blocks[i].Block.Position -= blocks[i - 1].Block.Position + blocks[i - 1].Block.Size + 1;
            }

            return blocks.ToArray();
        }

        /// <summary>
        /// Writes the header to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected override void WriteHeader(Stream baseStream, Stream modStream)
        {
            base.WriteHeader(baseStream, modStream);
            using (BinaryWriter writer = Writer)
            {
                writer.Write(baseStream.Length.VLEEncode());
                writer.Write(modStream.Length.VLEEncode());
            }
        }

        /// <summary>
        /// Writes a block to the patch.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <param name="blockInfo">The <see cref="BlockInfo{PatchBlock}"/> to write to the patch.</param>
        protected override void WriteBlock(Stream baseStream, Stream modStream, BlockInfo<PatchBlock> blockInfo)
        {
            using (BinaryWriter writer = Writer)
            {
                writer.Write(blockInfo.Block.Position.VLEEncode());

                IEnumerator<byte> baseEnumerator = ReadSequence(blockInfo.BaseSequence).GetEnumerator();
                IEnumerator<byte> modEnumerator = ReadSequence(blockInfo.ModSequence).GetEnumerator();

                for (int i = 0; i < blockInfo.Block.Size; i++)
                {
                    baseEnumerator.MoveNext();
                    modEnumerator.MoveNext();

                    writer.Write((byte)(baseEnumerator.Current ^ modEnumerator.Current));
                }

                writer.Write((byte)0x00);
            }
        }

        /// <summary>
        /// Writes a header that can be used for verifying the integrity of the streams.
        /// </summary>
        /// <param name="patchStream">The stream of the patch.</param>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected override void WriteVerificationFooter(HashStream<Crc32> patchStream, HashStream<Crc32> baseStream, HashStream<Crc32> modStream)
        {
            base.WriteVerificationFooter(patchStream, baseStream, modStream);

            using (BinaryWriter writer = Writer)
            {
                writer.Write(baseStream.Hash.Reverse().ToArray());
                writer.Write(modStream.Hash.Reverse().ToArray());
                patchStream.FlushFinalBlock();
                writer.Write(patchStream.Hash.Reverse().ToArray());
            }
        }
    }
}
