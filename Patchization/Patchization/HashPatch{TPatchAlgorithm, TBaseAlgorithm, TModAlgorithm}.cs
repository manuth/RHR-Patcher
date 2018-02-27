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
    /// <typeparam name="TPatchAlgorithm">The <see cref="System.Security.Cryptography.HashAlgorithm"/> to calculate the hash of the patch.</typeparam>
    /// <typeparam name="TBaseAlgorithm">The <see cref="System.Security.Cryptography.HashAlgorithm"/> to calculate the hash of the untouched stream.</typeparam>
    /// <typeparam name="TModAlgorithm">The <see cref="System.Security.Cryptography.HashAlgorithm"/> to calculate the hash of the modified stream.</typeparam>
    public abstract class HashPatch<TBlock, TPatchAlgorithm, TBaseAlgorithm, TModAlgorithm> : Patch<TBlock> where TBlock : IPatchBlock where TPatchAlgorithm : HashAlgorithm, new() where TBaseAlgorithm : HashAlgorithm, new() where TModAlgorithm : HashAlgorithm, new()
    {
        /// <summary>
        /// The <see cref="System.Security.Cryptography.HashAlgorithm"/> that is used to calculate the hash of the patch.
        /// </summary>
        protected TPatchAlgorithm HashAlgorithm = new TPatchAlgorithm();

        /// <summary>
        /// The value to match the computed hash of the patch.
        /// </summary>
        private byte[] validHash = new byte[] { };

        /// <summary>
        /// The value to match the computed hash of the untouched stream.
        /// </summary>
        private byte[] validBaseHash = new byte[] { };

        /// <summary>
        /// The value to match the computed hash of the modified stream.
        /// </summary>
        private byte[] validModHash = new byte[] { };

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, TPatchAlgorithm, TBaseAlgorithm, TModAlgorithm}"/> class.
        /// </summary>
        public HashPatch()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, TPatchAlgorithm, TBaseAlgorithm, TModAlgorithm}"/> class with a file path.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the patch-file.</param>
        public HashPatch(string fileName) : base(fileName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashPatch{TBlock, TPatchAlgorithm, TBaseAlgorithm, TModAlgorithm}"/> class with a stream.
        /// </summary>
        /// <param name="stream">The stream of the patch.</param>
        public HashPatch(Stream stream) : base(stream)
        { }

        /// <summary>
        /// Gets the hash of the patch.
        /// </summary>
        public byte[] Hash
        {
            get
            {
                return HashAlgorithm.Hash;
            }
        }

        /// <summary>
        /// Gets or sets the value to match the computed hash of the patch.
        /// </summary>
        public byte[] ValidHash
        {
            get
            {
                return validHash;
            }

            protected set
            {
                validHash = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to match the computed hash of the untouched stream.
        /// </summary>
        public byte[] ValidBaseHash
        {
            get
            {
                return validBaseHash;
            }

            protected set
            {
                validBaseHash = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to match the computed hash of the modified stream.
        /// </summary>
        public byte[] ValidModHash
        {
            get
            {
                return validModHash;
            }

            protected set
            {
                validModHash = value;
            }
        }

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        public override sealed void Apply(Stream baseStream, Stream outputStream)
        {
            if (!ValidHash.SequenceEqual(Hash))
            {
                throw new ChecksumException(Hash, ValidHash);
            }

            using (HashStream<TPatchAlgorithm> patchHashStream = new HashStream<TPatchAlgorithm>(BaseStream, HashAlgorithm))
            using (HashStream<TBaseAlgorithm> baseHashStream = new HashStream<TBaseAlgorithm>(baseStream))
            using (HashStream<TModAlgorithm> outputHashStream = new HashStream<TModAlgorithm>(outputStream))
            {
                Apply(baseHashStream, outputHashStream);

                if (!baseHashStream.HasFlushedFinalBlock)
                {
                    baseHashStream.Position = baseHashStream.Length;
                    baseHashStream.FlushFinalBlock();
                }

                if (!outputHashStream.HasFlushedFinalBlock)
                {
                    outputHashStream.Position = outputHashStream.Length;
                    outputHashStream.FlushFinalBlock();
                }

                Verify(baseHashStream, outputHashStream);
            }
        }

        /// <summary>
        /// Applies the patch.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        public virtual void Apply(HashStream<TBaseAlgorithm> baseStream, HashStream<TModAlgorithm> outputStream)
        {
            base.Apply(baseStream, outputStream);
        }

        /// <summary>
        /// Analyzes the entire patch.
        /// </summary>
        protected override void Analyze()
        {
            using (HashStream<TPatchAlgorithm> patchHashStream = new HashStream<TPatchAlgorithm>(BaseStream, HashAlgorithm))
            {
                BaseStream = patchHashStream;
                patchHashStream.Initialize();
                base.Analyze();
                AnalyzeVerificationFooter(patchHashStream);

                if (!patchHashStream.HasFlushedFinalBlock)
                {
                    patchHashStream.FlushFinalBlock();
                }

                BaseStream = patchHashStream.BaseStream;
            }
        }

        /// <summary>
        /// Analyzes the verification-footer.
        /// </summary>
        /// <param name="patchStream">The stream of the patch.</param>
        protected virtual void AnalyzeVerificationFooter(HashStream<TPatchAlgorithm> patchStream)
        {
        }

        /// <summary>
        /// Verifies the hashes of the streams.
        /// </summary>
        /// <param name="baseStream">The  stream to read the data to patch from.</param>
        /// <param name="outputStream">The stream to write the patched data to.</param>
        protected virtual void Verify(HashStream<TBaseAlgorithm> baseStream, HashStream<TModAlgorithm> outputStream)
        {
            if (!ValidBaseHash.SequenceEqual(baseStream.Hash))
            {
                throw new ChecksumException(baseStream.Hash, ValidBaseHash);
            }

            if (!ValidModHash.SequenceEqual(outputStream.Hash))
            {
                throw new ChecksumException(outputStream.Hash, ValidModHash);
            }
        }

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        public override sealed void Create(Stream baseStream, Stream modStream)
        {
            using (HashStream<TPatchAlgorithm> patchHashStream = new HashStream<TPatchAlgorithm>(BaseStream, HashAlgorithm))
            using (HashStream<TBaseAlgorithm> baseHashStream = new HashStream<TBaseAlgorithm>(baseStream))
            using (HashStream<TModAlgorithm> modHashStream = new HashStream<TModAlgorithm>(modStream))
            {
                BaseStream = patchHashStream;
                patchHashStream.Initialize();
                Create(patchHashStream, baseHashStream, modHashStream);

                if (!baseHashStream.HasFlushedFinalBlock)
                {
                    baseHashStream.Position = baseHashStream.Length;
                    baseHashStream.FlushFinalBlock();
                }

                if (!modHashStream.HasFlushedFinalBlock)
                {
                    modHashStream.Position = modHashStream.Length;
                    modHashStream.FlushFinalBlock();
                }

                WriteVerificationFooter(patchHashStream, baseHashStream, modHashStream);

                BaseStream = patchHashStream.BaseStream;
            }
        }

        /// <summary>
        /// Creates the patch based on the difference between two streams.
        /// </summary>
        /// <param name="patchStream">The stream of the patch.</param>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected virtual void Create(HashStream<TPatchAlgorithm> patchStream, HashStream<TBaseAlgorithm> baseStream, HashStream<TModAlgorithm> modStream)
        {
            base.Create(baseStream, modStream);
        }

        /// <summary>
        /// Writes a header that can be used for verifying the integrity of the streams.
        /// </summary>
        /// <param name="patchStream">The stream of the patch.</param>
        /// <param name="baseStream">The untouched stream.</param>
        /// <param name="modStream">The modified stream.</param>
        protected virtual void WriteVerificationFooter(HashStream<TPatchAlgorithm> patchStream, HashStream<TBaseAlgorithm> baseStream, HashStream<TModAlgorithm> modStream)
        {
        }
    }
}
