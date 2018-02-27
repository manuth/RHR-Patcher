using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a translation.
    /// </summary>
    [DataContract]
    public class Translation : APIObject
    {
        /// <summary>
        /// Gets or sets the version to which the translation belongs to.
        /// </summary>
        public Version Version { get; set; } = new Version();

        /// <summary>
        /// Gets or sets the language of the translation.
        /// </summary>
        [DataMember]
        public Language Language { get; set; } = new Language();

        /// <summary>
        /// Gets or sets the state of the translation.
        /// </summary>
        [DataMember]
        public TranslationState State { get; set; } = new TranslationState();

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Translations";
            }
        }

        /// <summary>
        /// Gets or sets the id of the version to which the translation belongs to.
        /// </summary>
        [DataMember(Name = "VersionID")]
        private int? VersionID
        {
            get
            {
                return Version?.ID;
            }

            set
            {
                Version = Version ?? new Version();
                Version.ID = value;
            }
        }
    }
}
