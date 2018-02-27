using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a version of a project.
    /// </summary>
    [DataContract]
    public class Version : APIObject, IIdentifiableObject, IEquatable<Version>
    {
        /// <summary>
        /// The translations of this version.
        /// </summary>
        private TranslationCollection translations = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        public Version()
        {
            Translations = new TranslationCollection(this);
        }

        /// <summary>
        /// Gets or sets the identification-number of the version.
        /// </summary>
        [DataMember]
        public int? ID { get; set; } = null;

        /// <summary>
        /// Gets or sets the major-number of the version.
        /// </summary>
        [DataMember]
        public int Major { get; set; } = 1;

        /// <summary>
        /// Gets or sets the minor-number of the version.
        /// </summary>
        [DataMember]
        public int Minor { get; set; } = 0;

        /// <summary>
        /// Gets or sets the build-number of the version.
        /// </summary>
        [DataMember]
        public int Build { get; set; } = 0;

        /// <summary>
        /// Gets or sets the type of the version.
        /// </summary>
        [DataMember]
        public VersionType Type { get; set; } = new VersionType();

        /// <summary>
        /// Gets or sets the project to which this version belongs to.
        /// </summary>
        public Project Project { get; set; } = new Project();

        /// <summary>
        /// Gets or sets the location of the version.
        /// </summary>
        public Uri Location { get; set; } = null;

        /// <summary>
        /// Gets or sets the translations of the version.
        /// </summary>
        public TranslationCollection Translations
        {
            get
            {
                return translations;
            }

            set
            {
                value.Version = this;
                translations = value;
            }
        }

        /// <summary>
        /// Gets the page-name of the related API.
        /// </summary>
        public override string Page
        {
            get
            {
                return "API/Versions";
            }
        }

        /// <summary>
        /// Gets or sets the actual version.
        /// </summary>
        private System.Version VersionNumber
        {
            get
            {
                return new System.Version(Major, Minor, Build);
            }

            set
            {
                Major = Math.Max(0, value.Major);
                Minor = Math.Max(0, value.Minor);
                Build = Math.Max(0, value.Build);
            }
        }

        /// <summary>
        /// Gets or sets the location of the version as a string.
        /// </summary>
        [DataMember(Name = "Location")]
        private string LocationPath
        {
            get
            {
                return Location?.ToString();
            }

            set
            {
                Location = new Uri(value);
            }
        }

        /// <summary>
        /// Gets or sets the id of the project.
        /// </summary>
        [DataMember(Name = "ProjectID")]
        private int? ProjectID
        {
            get
            {
                return Project?.ID;
            }

            set
            {
                Project = Project ?? new Project();
                Project.ID = value;
            }
        }

        /// <summary>
        /// Gets or sets the versions of the project.
        /// </summary>
        [DataMember(Name = "Translations")]
        private List<Translation> TranslationList
        {
            get
            {
                return Translations?.ToList() ?? new List<Translation>();
            }

            set
            {
                if (Translations == null)
                {
                    Translations = new TranslationCollection(this);
                }

                Translations.Clear();
                foreach (Translation translation in value)
                {
                    Translations.Add(translation);
                }
            }
        }

        /// <summary>
        /// Determines whether two specified <see cref="Version"/> objects are equal.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> equals <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator ==(Version v1, Version v2)
        {
            return v1.VersionNumber == v2.VersionNumber;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="Version"/> object is greater than the second specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> is greater than <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator >(Version v1, Version v2)
        {
            return v1.VersionNumber > v2.VersionNumber;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="Version"/> object is greater than or equal to the second specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> is greater than or equal to <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator >=(Version v1, Version v2)
        {
            return v1.VersionNumber >= v2.VersionNumber;
        }

        /// <summary>
        /// Determines whether two specified <see cref="Version"/> objects are not equal.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> does not equal <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator !=(Version v1, Version v2)
        {
            return v1.VersionNumber != v2.VersionNumber;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="Version"/> object is less than the second specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> is less than <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator <(Version v1, Version v2)
        {
            return v1.VersionNumber < v2.VersionNumber;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="Version"/> object is less than or equal to the second <see cref="Version"/> object.
        /// </summary>
        /// <param name="v1">The first <see cref="Version"/> object.</param>
        /// <param name="v2">The second <see cref="Version"/> object.</param>
        /// <returns><see cref="true"/> if <paramref name="v1"/> is less than or equal to <paramref name="v2"/>; otherwise, <see cref="false"/>.</returns>
        public static bool operator <=(Version v1, Version v2)
        {
            return v1.VersionNumber <= v2.VersionNumber;
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Version"/>.
        /// </summary>
        /// <returns>The <see cref="Version"/> this method creates, cast as an <see cref="Object"/>.</returns>
        public object Clone()
        {
            Version version = MemberwiseClone() as Version;
            version.VersionNumber = version.VersionNumber.Clone() as System.Version;
            return version;
        }

        /// <summary>
        /// Compares the current <see cref="Version"/> object to a specified <see cref="Version"/> object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">A <see cref="Version"/> object to compare to the current <see cref="Version"/> object, or <see cref="null"/>.</param>
        /// <returns>A signed integer that indicates the relative values of the two objects, as shown in the following table.</returns>
        public int CompareTo(Version value)
        {
            return VersionNumber.CompareTo(value.VersionNumber);
        }

        /// <summary>
        /// Compares the current <see cref="Version"/> object to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="version">An object to compare, or <see cref="null"/>.</param>
        /// <returns>A signed integer that indicates the relative values of the two objects, as shown in the following table.</returns>
        public int CompareTo(object version)
        {
            return VersionNumber.CompareTo(version);
        }

        /// <summary>
        /// Returns a value indicating whether the current <see cref="Version"/> object is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with the current <see cref="Version"/> object, or <see cref="null"/>.</param>
        /// <returns><see cref="true"/> if the current <see cref="Version"/> object and <paramref name="obj"/> are both <see cref="Version"/> objects, and every component of the current <see cref="Version"/> object matches the corresponding component of <paramref name="obj"/>; otherwise, <see cref="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Version)
            {
                return Equals(obj as Version);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a value indicating whether the current <see cref="Version"/> object and a specified <see cref="Version"/> object represent the same value.
        /// </summary>
        /// <param name="obj">A <see cref="Version"/> object to compare to the current <see cref="Version"/> object, or <see cref="null"/>.</param>
        /// <returns><see cref="true"/> if every component of the current <see cref="Version"/> object matches the corresponding component of the <paramref name="obj"/> parameter; otherwise, <see cref="false"/>.</returns>
        public bool Equals(Version obj)
        {
            return
                VersionNumber.Equals(obj.VersionNumber) &&
                ID == obj.ID &&
                Type.ID == obj.Type.ID &&
                Project.ID == obj.Project.ID &&
                Location == obj.Location;
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Version"/> object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Tuple.Create(ID, VersionNumber, Type, Project, Location).GetHashCode();
        }
    }
}
