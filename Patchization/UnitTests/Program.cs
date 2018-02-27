using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManuTh.Patchization;
using ManuTh.Tasks;

namespace ManuTh.Patchization.UnitTests
{
    /// <summary>
    /// The Unit-Tests for <see cref="Patchization"/>.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Runs the Unit-Tests for <see cref="Patchization"/>.
        /// </summary>
        /// <param name="args">The specified arguments.</param>
        internal static void Main(string[] args)
        {
            /*====================
             * 1. Executing a patch.
             *====================
             */
            using (IPatch patch = new UPSPatch("SG.ups"))
            using (FileStream input = new FileStream("BPRE.gba", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream output = new FileStream("SG.gba", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                patch.Apply(input, output);
            }

            /*====================
             * 2. Creating a patch.
             *====================
             */
            using (IPatch patch = new UPSPatch())
            using (FileStream input = new FileStream("BPRE.gba", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (FileStream output = new FileStream("SG.gba", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                patch.Create(input, output);
                patch.Save("SG.ups");
            }
        }
    }
}
