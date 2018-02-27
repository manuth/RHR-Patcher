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
    public interface IPatch<TBlock> : IPatch where TBlock : IPatchBlock
    {
        /// <summary>
        /// Gets the Block at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block to fetch.</param>
        /// <returns>The block inside the patch at the specified index.</returns>
        TBlock this[int index] { get; }
    }
}
