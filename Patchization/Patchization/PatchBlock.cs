using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a block of a patch.
    /// </summary>
    public class PatchBlock : IPatchBlock
    {
        /// <summary>
        /// The offset of the block inside the patch.
        /// </summary>
        private long offset = 0;

        /// <summary>
        /// The position to apply the block to.
        /// </summary>
        private long position = 0;

        /// <summary>
        /// The size of the block.
        /// </summary>
        private int size = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchBlock"/> class.
        /// </summary>
        public PatchBlock()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchBlock"/> class with an offset, a position and a size.
        /// </summary>
        /// <param name="offset">The offset of the block inside the patch.</param>
        /// <param name="position">The position to apply the block to.</param>
        /// <param name="size">The size of the block.</param>
        public PatchBlock(long offset, long position, int size)
        {
            this.offset = offset;
            this.position = position;
            this.size = size;
        }

        /// <summary>
        /// Gets or sets the offset of the block inside the patch.
        /// </summary>
        public long Offset
        {
            get
            {
                return offset;
            }

            set
            {
                offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the position to apply the block to.
        /// </summary>
        public long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }
    }
}
