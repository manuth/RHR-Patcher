using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents an object which is queryable from the Project-Database.
    /// </summary>
    [DataContract]
    public abstract class APIObject : IAPIObject
    {
        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public abstract string Page { get; }

        /// <summary>
        /// Clones the object and all its properties to another object.
        /// </summary>
        /// <param name="obj">The object to copy this object to.</param>
        public void CloneTo(IAPIObject obj)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo property in GetType().GetProperties(flags))
            {
                if (
                    property.CanRead &&
                    obj.GetType().GetProperty(property.Name, flags) != null)
                {
                    property.SetValue(obj, property.GetValue(this));
                }
            }
        }
    }
}
