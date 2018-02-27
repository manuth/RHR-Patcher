/// -----------------------------------------------------------------------
/// <copyright file="Crc32.cs" company="manuth">
///      Copyright (c) Damien Guard.  All rights reserved.
///      Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
///      You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
///      Originally published at http://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net
///
///      Copyright (c) manuth. All rights reserved.
///      Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
///      You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
/// </copyright>
/// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DamienG.Security.Cryptography
{
    /// <summary>
    /// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
    /// </summary>
    /// <remarks>
    /// CRC32 should only be used for backward compatibility with older file formats
    /// and algorithms. It is not secure enough for new applications.
    /// If you need to call multiple times for the same data either use the HashAlgorithm
    /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
    /// being passed in as the seed for the next Compute call.
    /// </remarks>
    public sealed class Crc32 : HashAlgorithm
    {
        /// <summary>
        /// The default polynomial used for calculating CRC32-hashes.
        /// </summary>
        public const uint DefaultPolynomial = 0xedb88320u;

        /// <summary>
        /// The default seed used for calculating CRC32-hashes.
        /// </summary>
        public const uint DefaultSeed = 0xffffffffu;

        /// <summary>
        /// The default table of hash-values used for calculating CRC32-hashes.
        /// </summary>
        private static uint[] defaultTable;

        /// <summary>
        /// The seed of the CRC32-hash.
        /// </summary>
        private readonly uint seed;

        /// <summary>
        /// The table of hash-values used for calculating this CRC32-hash.
        /// </summary>
        private readonly uint[] table;

        /// <summary>
        /// The currently calculated hash.
        /// </summary>
        private uint hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="Crc32"/> class.
        /// </summary>
        public Crc32() : this(DefaultPolynomial, DefaultSeed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crc32"/> class with a polynomial and a seed.
        /// </summary>
        /// <param name="polynomial">The polynomial to use to calculate the hash.</param>
        /// <param name="seed">The seed to use to calculate the hash.</param>
        public Crc32(uint polynomial, uint seed)
        {
            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        /// <summary>
        /// Gets the size, in bits, of the computed hash code.
        /// </summary>
        public override int HashSize
        {
            get
            {
                return 32;
            }
        }

        /// <summary>
        /// Computes the CRC32-hash of the buffer.
        /// </summary>
        /// <param name="buffer">The buffer whose CRC32-hash is to be calculated.</param>
        /// <returns>The CRC32-hash.</returns>
        public static uint Compute(byte[] buffer)
        {
            return Compute(DefaultSeed, buffer);
        }

        /// <summary>
        /// Computes the CRC32-hash of the buffer with a seed.
        /// </summary>
        /// <param name="seed">The seed to use to calculate the CRC32-hash.</param>
        /// <param name="buffer">The buffer whose CRC32-hash is to be calculated.</param>
        /// <returns>The CRC32-hash.</returns>
        public static uint Compute(uint seed, byte[] buffer)
        {
            return Compute(DefaultPolynomial, seed, buffer);
        }

        /// <summary>
        /// Computes the CRC32-hash of the buffer with a seed and a polynomial.
        /// </summary>
        /// <param name="polynomial">The polynomial to use to calculate the CRC32-hash.</param>
        /// <param name="seed">The seed to use to calculate the CRC32-hash.</param>
        /// <param name="buffer">The buffer whose CRC32-hash is to be calculated.</param>
        /// <returns>The CRC32-hash.</returns>
        public static uint Compute(uint polynomial, uint seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Converts an <see cref="UInt32"/>-value to Big-Endian bytes.
        /// </summary>
        /// <param name="uint32">The value that is to be converted.</param>
        /// <returns>The Big-Endian bytes representing the value.</returns>
        public static byte[] UInt32ToBigEndianBytes(uint uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }

        /// <summary>
        /// Initializes the CRC32-hash.
        /// </summary>
        public override void Initialize()
        {
            hash = seed;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            hash = CalculateHash(table, hash, array, ibStart, cbSize);
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            var hashBuffer = UInt32ToBigEndianBytes(~hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        /// <summary>
        /// Initializes a table of hash-values for calculating CRC32-hashes with a specific polynomial.
        /// </summary>
        /// <param name="polynomial">The polynomial whose hash-values are to be calculated.</param>
        /// <returns>A table of hash-values for calculating CRC32-hashes.</returns>
        private static uint[] InitializeTable(uint polynomial)
        {
            if (polynomial == DefaultPolynomial && defaultTable != null)
            {
                return defaultTable;
            }

            var createTable = new uint[256];

            for (var i = 0; i < 256; i++)
            {
                var entry = (uint)i;

                for (var j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ polynomial;
                    }
                    else
                    {
                        entry = entry >> 1;
                    }
                }

                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
            {
                defaultTable = createTable;
            }

            return createTable;
        }

        /// <summary>
        /// Calculates the hash of the buffer.
        /// </summary>
        /// <param name="table">A table of hash-values to use for calculating the CRC32-hash.</param>
        /// <param name="seed">The seed to use to calculate the CRC32-hash.</param>
        /// <param name="buffer">The buffer whose CRC32-hash is to be calculated.</param>
        /// <param name="start">The zero-based index of the first byte to calculate the CRC32-hash.</param>
        /// <param name="size">The number of bytes to calculate the CRC32-hash.</param>
        /// <returns>The CRC32-hash.</returns>
        private static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size)
        {
            var hash = seed;

            for (var i = start; i < start + size; i++)
            {
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            }

            return hash;
        }
    }
}