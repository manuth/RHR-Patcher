using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManuTh.ProjectDBClient
{
    /// <summary>
    /// Represents a set of translations which belong to a specified version.
    /// </summary>
    public class TranslationCollection : Collection<Translation>, ICollection<Translation>
    {
        /// <summary>
        /// The version to which the translations belong to.
        /// </summary>
        private Version version;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationCollection"/> class.
        /// </summary>
        /// <param name="version">The version to which the translations belong to.</param>
        public TranslationCollection(Version version)
        {
            this.version = version;
        }

        /// <summary>
        /// Gets or sets the version to which the translations belong to.
        /// </summary>
        public Version Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
                for (int i = 0; i < Count; i++)
                {
                    this[i] = this[i];
                }
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="TranslationCollection"/>.
        /// </summary>
        protected override void ClearItems()
        {
            foreach (Translation translation in Items)
            {
                translation.Version = null;
            }

            base.ClearItems();
        }

        /// <summary>
        /// Inserts an element into the <see cref="TranslationCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be <see cref="null"/> for reference types.</param>
        protected override void InsertItem(int index, Translation item)
        {
            item.Version = version;
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="TranslationCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            Items[index].Version = null;
            base.RemoveItem(index);
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be <see cref="null"/> for reference types.</param>
        protected override void SetItem(int index, Translation item)
        {
            if (!item.Equals(Items[index]))
            {
                Items[index].Version = null;
                item.Version = version;
            }

            base.SetItem(index, item);
        }
    }
}
