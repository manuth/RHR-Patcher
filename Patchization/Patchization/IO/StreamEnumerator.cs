using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.Patchization.IO
{
    /// <summary>
    /// Provides useful methods for handling streams.
    /// </summary>
    public static class StreamEnumerator
    {
        /// <summary>
        /// Converts a stream to an <see cref="IEnumerable{byte}"/>.
        /// </summary>
        /// <param name="stream">The stream that is to be converted.</param>
        /// <returns>The stream as an <see cref="IEnumerable{byte}"/></returns>
        public static IEnumerable<byte> ToEnumerable(this Stream stream)
        {
            for (int current = stream.ReadByte(); current >= 0; current = stream.ReadByte())
            {
                yield return (byte)current;
            }

            yield break;
        }
    }
}
