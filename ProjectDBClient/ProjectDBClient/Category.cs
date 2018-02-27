using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a category.
    /// </summary>
    [DataContract]
    public class Category : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the category.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets the category to which the category belongs to.
        /// </summary>
        [DataMember(Name = "Category")]
        public Category Parent { get; set; } = null;

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Categories";
            }
        }
    }
}
