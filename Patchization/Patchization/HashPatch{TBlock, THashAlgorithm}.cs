using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Represents a patch which provides of hash-verification.
    /// </summary>
    /// <typeparam name="TBlock">The type of the blocks inside the patch.</typeparam>
    /// <typeparam name="THashAlgorithm">The <see cref="THashAlgorithm"/> to calculate the hash of the streams.</typeparam>
    public abstract class HashPatch<TBlock, THashAlgorithm> : HashPatch<TBlock, THashAlgorithm, THashAlgorithm, THashAlgorithm> where TBlock : IPatchBlock where THashAlgorithm : HashAlgorithm, new()
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, THashAlgorithm}"/> class.
        /// </summary>
        public HashPatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, THashAlgorithm}"/> class with a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the patch-file.</param>
        public HashPatch(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, THashAlgorithm}"/> class with a stream.
        /// </summary>
        /// <param name="stream">The stream of the patch.</param>
        public HashPatch(Stream stream) : base(stream)
        {
        }
    }
}
