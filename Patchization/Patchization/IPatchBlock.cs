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
    public interface IPatchBlock
    {
        /// <summary>
        /// Gets or sets the offset of the block inside the patch.
        /// </summary>
        long Offset { get; set; }

        /// <summary>
        /// Gets or sets the position to apply the block to.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        int Size { get; set; }
    }
}
