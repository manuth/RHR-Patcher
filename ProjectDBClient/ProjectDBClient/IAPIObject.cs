using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents an object which is queryable from the Project-Database.
    /// </summary>
    public interface IAPIObject
    {
        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        string Page { get; }

        /// <summary>
        /// Clones the object and all its properties to another object.
        /// </summary>
        /// <param name="obj">The object to copy this object to.</param>
        void CloneTo(IAPIObject obj);
    }
}
