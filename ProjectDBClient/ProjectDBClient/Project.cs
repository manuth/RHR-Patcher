using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a project.
    /// </summary>
    [DataContract]
    public class Project : APIObject, IIdentifiableObject
    {
        /// <summary>
        /// Gets or sets the versions of the project.
        /// </summary>
        private VersionCollection versions = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            Versions = new VersionCollection(this);
        }

        /// <summary>
        /// Gets or sets the identification-number of the project.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        [DataMember]
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets the base of the project.
        /// </summary>
        [DataMember]
        public Base Base { get; set; } = new Base();

        /// <summary>
        /// Gets or sets the category of the project.
        /// </summary>
        [DataMember]
        public Category Category { get; set; } = new Category();

        /// <summary>
        /// Gets or sets the author of the project.
        /// </summary>
        [DataMember]
        public User Author { get; set; } = new User();

        /// <summary>
        /// Gets or sets the website of the project.
        /// </summary>
        public Uri Website { get; set; } = null;

        /// <summary>
        /// Gets or sets the board-thread of the project.
        /// </summary>
        public Uri Board { get; set; } = null;

        /// <summary>
        /// Gets or sets the image-location of the banner of the project.
        /// </summary>
        public Uri Banner { get; set; } = null;

        /// <summary>
        /// Gets or sets the image-location of the cover of the project.
        /// </summary>
        public Uri Cover { get; set; } = null;

        /// <summary>
        /// Gets or sets a design color to draw the project nicely.
        /// </summary>
        public Color Color1 { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets a design color to draw the project nicely.
        /// </summary>
        public Color Color2 { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets a value indicating whether the project is popular.
        /// </summary>
        [DataMember]
        public bool Popular { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the project is enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the date of the creation of the project.
        /// </summary>
        public DateTime? CreationDate { get; set; } = null;

        /// <summary>
        /// Gets or sets the versions of the project.
        /// </summary>
        public VersionCollection Versions
        {
            get
            {
                return versions;
            }

            set
            {
                value.Project = this;
                versions = value;
            }
        }

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Projects";
            }
        }

        /// <summary>
        /// Gets or sets the website of the project as a string.
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
        /// Gets or sets the board-thread of the project as a string.
        /// </summary>
        [DataMember(Name = "Board")]
        private string BoardPath
        {
            get
            {
                return Board?.ToString();
            }

            set
            {
                Board = new Uri(value);
            }
        }
        
        /// <summary>
        /// Gets or sets the image-location of the banner of the project as a string.
        /// </summary>
        [DataMember(Name = "Banner")]
        private string BannerPath
        {
            get
            {
                return Banner?.ToString();
            }

            set
            {
                Banner = new Uri(value);
            }
        }
        
        /// <summary>
        /// Gets or sets the image-location of the cover of the project as a string.
        /// </summary>
        [DataMember(Name = "Cover")]
        private string CoverPath
        {
            get
            {
                return Cover?.ToString();
            }

            set
            {
                Cover = new Uri(value);
            }
        }

        /// <summary>
        /// Gets or sets a design color to draw the project nicely as an integer.
        /// </summary>
        [DataMember(Name = "Color1")]
        private int Color1Argb
        {
            get
            {
                return Color1.ToArgb();
            }

            set
            {
                Color1 = Color.FromArgb(value);
            }
        }

        /// <summary>
        /// Gets or sets a design color to draw the project nicely as an integer.
        /// </summary>
        [DataMember(Name = "Color2")]
        private int Color2Argb
        {
            get
            {
                return Color2.ToArgb();
            }

            set
            {
                Color2 = Color.FromArgb(value);
            }
        }

        /// <summary>
        /// Gets or sets the date of the creation of the project as a string.
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
        /// Gets or sets the versions of the project.
        /// </summary>
        [DataMember(Name = "Versions")]
        private List<Version> VersionList
        {
            get
            {
                return Versions?.ToList() ?? new List<Version>();
            }

            set
            {
                if (Versions == null)
                {
                    Versions = new VersionCollection(this);
                }

                Versions.Clear();
                foreach (Version version in value)
                {
                    Versions.Add(version);
                }
            }
        }
    }
}
