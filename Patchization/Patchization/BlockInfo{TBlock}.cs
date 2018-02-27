using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Provides information about a block of a patch.
    /// </summary>
    public class BlockInfo<TBlock> where TBlock : IPatchBlock
    {
        /// <summary>
        /// The block described by this <see cref="BlockInfo{TBlock}"/>.
        /// </summary>
        private TBlock block;

        /// <summary>
        /// The byte-sequence of the block of the untouched stream.
        /// </summary>
        private byte[] baseSequence = new byte[] { };

        /// <summary>
        /// The byte-sequence of the block of the modified stream.
        /// </summary>
        private byte[] modSequence = new byte[] { };

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInfo{TBlock}"/> class.
        /// </summary>
        public BlockInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockInfo{TBlock}"/> class with a block and byte-sequences of the blocks of the untouched and the modified stream.
        /// </summary>
        /// <param name="block">The block which is to be described by this <see cref="BlockInfo{TBlock}"/>.</param>
        /// <param name="baseSequence">The byte-sequence of the block of the untouched stream.</param>
        /// <param name="modSequence">The byte-sequence of the block of the modified stream.</param>
        public BlockInfo(TBlock block, byte[] baseSequence, byte[] modSequence)
        {
            this.block = block;
            this.baseSequence = baseSequence;
            this.modSequence = modSequence;
        }

        /// <summary>
        /// Gets or sets the block described by this <see cref="BlockInfo{TBlock}"/>.
        /// </summary>
        public virtual TBlock Block
        {
            get
            {
                return block;
            }

            set
            {
                block = value;
            }
        }

        /// <summary>
        /// Gets or sets the byte-sequence of the block of the untouched stream.
        /// </summary>
        public virtual byte[] BaseSequence
        {
            get
            {
                return baseSequence;
            }

            set
            {
                baseSequence = value;
            }
        }

        /// <summary>
        /// Gets or sets the byte-sequence of the block of the modified stream.
        /// </summary>
        public virtual byte[] ModSequence
        {
            get
            {
                return modSequence;
            }

            set
            {
                modSequence = value;
            }
        }
    }
}
