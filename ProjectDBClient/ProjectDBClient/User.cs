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
    /// Represents a user.
    /// </summary>
    [DataContract]
    public class User : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the identification-number of the user.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        [DataMember]
        public string Avatar { get; set; } = null;

        /// <summary>
        /// Gets or sets the website of the user.
        /// </summary>
        public Uri Website { get; set; } = null;

        /// <summary>
        /// Gets or sets the date of the creation of the user.
        /// </summary>
        public DateTime? CreationDate { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the user has administrative rights.
        /// </summary>
        [DataMember]
        public bool Admin { get; set; } = false;

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Users";
            }
        }

        /// <summary>
        /// Gets or sets the website of the user as a string.
        /// </summary>
        [DataMember(Name = "Website")]
        private string WebsitePath
        {
            get
            {
                return Website?.ToString();
            }

            set
            {
                Website = new Uri(value);
            }
        }

        /// <summary>
        /// Gets or sets the date of the creation of the user as a string.
        /// </summary>
        [DataMember(Name = "CreationDate")]
        private string CreationDateString
        {
            get
            {
                return CreationDate?.ToString(CultureInfo.InvariantCulture.DateTimeFormat.SortableDateTimePattern);
            }

            set
            {
                CreationDate = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current object.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
