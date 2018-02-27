using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a version-type.
    /// </summary>
    [DataContract]
    public class VersionType : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the version-type.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the version-type.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/VersionTypes";
            }
        }
    }
}
