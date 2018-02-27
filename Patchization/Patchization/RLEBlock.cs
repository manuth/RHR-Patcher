using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a run-length encoded block of a patch.
    /// </summary>
    public class RLEBlock : PatchBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RLEBlock"/> class.
        /// </summary>
        public RLEBlock() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RLEBlock"/> class with an offset, a position and a length.
        /// </summary>
        /// <param name="offset">The offset of the block inside the patch.</param>
        /// <param name="position">The position to apply the block to.</param>
        /// <param name="size">The size of the block.</param>
        /// <param name="rle">A value indicating whether the block is run-length encoded.</param>
        public RLEBlock(long offset, long position, int size, bool rle) : base(offset, position, size)
        {
            IsRLE = rle;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the block is run-length encoded.
        /// </summary>
        public bool IsRLE { get; set; } = false;
    }
}
