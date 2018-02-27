using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a language.
    /// </summary>
    [DataContract]
    public class Language : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the language.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the culture of the language.
        /// </summary>
        public CultureInfo Culture { get; set; } = null;

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Languages";
            }
        }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        [DataMember(Name = "Name")]
        private string CultureName
        {
            get
            {
                return Culture.Name;
            }

            set
            {
                Culture = new CultureInfo(value);
            }
        }
    }
}
