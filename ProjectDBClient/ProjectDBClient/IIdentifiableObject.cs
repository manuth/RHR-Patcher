using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents an <see cref="IAPIObject"/> which has an Identifier.
    /// </summary>
    public interface IIdentifiableObject : IAPIObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the object.
        /// </summary>
        int? ID { get; set; }
    }
}
