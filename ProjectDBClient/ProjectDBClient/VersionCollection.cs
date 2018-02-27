using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a set of versions which belong to a specified project.
    /// </summary>
    public class VersionCollection : Collection<Version>, ICollection<Version>
    {
        /// <summary>
        /// The project to which the versions belong to.
        /// </summary>
        private Project project;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCollection"/> class.
        /// </summary>
        /// <param name="project">The project to which the versions belong to.</param>
        public VersionCollection(Project project)
        {
            this.project = project;
        }

        /// <summary>
        /// Gets or sets the project to which the versions belong to.
        /// </summary>
        public Project Project
        {
            get
            {
                return project;
            }

            set
            {
                project = value;
                for (int i = 0; i < Count; i++)
                {
                    this[i] = this[i];
                }
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="VersionCollection"/>.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (Version version in Items)
            {
                version.Project = null;
            }

            base.ClearItems();
        }

        /// <summary>
        /// Inserts an element into the <see cref="VersionCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be <see cref="null"/> for reference types.</param>
        protected override void InsertItem(int index, Version item)
        {
            item.Project = project;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="VersionCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            Items[index].Project = null;
            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be <see cref="null"/> for reference types.</param>
        protected override void SetItem(int index, Version item)
        {
            if (!item.Equals(Items[index]))
            {
                Items[index].Project = null;
                item.Project = project;
            }

            base.SetItem(index, item);
        }
    }
}
