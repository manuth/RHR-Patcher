using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a base of a project.
    /// </summary>
    [DataContract]
    public class Base : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the base.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the base.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets the gamecode of the base.
        /// </summary>
        [DataMember]
        public string GameCode { get; set; } = null;

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Bases";
            }
        }
    }
}
