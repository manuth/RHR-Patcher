using ManuTh.Patchization.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization
{
    /// <summary>
    /// Provides methods to encode and decode Variable Length Encoded values.
    /// </summary>
    public static class VariableLengthEncoding
    {
        /// <summary>
        /// Encodes a value using variable-length encoding
        /// </summary>
        /// <param name="value">The value to encode</param>
        /// <returns>The encoded value</returns>
        public static byte[] VLEEncode(this long value)
        {
            List<byte> result = new List<byte>();
            long part = 0;

            do
            {
                part = value & 0x7F;
                value >>= 7;

                if (value == 0)
                {
                    part |= 0x80;
                }

                result.Add((byte)part);
                value--;
            }
            while (part < 0x80);

            return result.ToArray();
        }

        /// <summary>
        /// Decodes a variable-length encoded value.
        /// </summary>
        /// <param name="stream">The stream that contains the variable-length encoded value.</param>
        /// <returns>The decoded value.</returns>
        public static long VLEDecode(this Stream stream)
        {
            return stream.ToEnumerable().VLEDecode();
        }

        /// <summary>
        /// Decodes a variable-length encoded value.
        /// </summary>
        /// <param name="collection">The collection that contains the variable-length encoded value.</param>
        /// <returns>The decoded value.</returns>
        public static long VLEDecode(this IEnumerable<byte> collection)
        {
            IEnumerator<byte> enumerator = collection.GetEnumerator();
            List<byte> value = new List<byte>();

            do
            {
                enumerator.MoveNext();
                value.Add(enumerator.Current);
            }
            while (enumerator.Current < 0x80);

            return VLEDecode(value.ToArray());
        }

        /// <summary>
        /// Decodes a variable-length encoded value.
        /// </summary>
        /// <param name="value">The variable-length encoded value.</param>
        /// <returns>The decoded value.</returns>
        public static long VLEDecode(this byte[] value)
        {
            long result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                int part = value[i] & 0x7F;
                if (i > 0)
                    part++;
                result += part << (i * 7);
            }
            return result;
        }
    }
}
