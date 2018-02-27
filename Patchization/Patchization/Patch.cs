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
    public abstract class Patch : Patch<PatchBlock>
    {
        /// <summary>
        /// Looks for different sequences inside the untouched stream and the modified stream.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        /// <returns>The <see cref="BlockInfo"/>s describing the different sequences.</returns>
        protected override BlockInfo<PatchBlock>[] FindBlocks(Stream baseStream, Stream modStream)
        {
            {
                using (BinaryReader baseReader = new BinaryReader(baseStream, Encoding.UTF8, true))
                using (BinaryReader modReader = new BinaryReader(modStream, Encoding.UTF8, true))
                {
                    List<BlockInfo<PatchBlock>> blocks = new List<BlockInfo<PatchBlock>>();
                    List<byte> baseSequence = new List<byte>();
                    List<byte> modSequence = new List<byte>();
                    long position = 0;

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

                    return blocks.ToArray();
                }
            }
        }
    }
}
