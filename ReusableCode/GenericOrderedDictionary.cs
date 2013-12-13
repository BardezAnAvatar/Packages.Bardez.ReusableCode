using System;
using System.Collections;
using System.Collections.Specialized;

namespace Bardez.Projects.ReusableCode
{
    public class GenericOrderedDictionary<TKey, TValue> : OrderedDictionary where TKey : class
    {
        /// <summary>Hides exposure of Int32 indexer for one that casts back to TValue</summary>
        /// <param name="index">Int32 index into the dictionary</param>
        /// <returns>The TValue object at the specified index</returns>
        new public TValue this[int index]
        {
            get { return (TValue)base[index]; }
            set { base[index] = value; }
        }

        /// <summary>Hides exposure of Object indexer for one that casts back to TValue</summary>
        /// <param name="key">TKey Key into the dictionary</param>
        /// <returns>The TValue object associated with the specified key</returns>
        public TValue this[TKey key]
        {
            get { return (TValue)base[key]; }
            set { base[key] = value; }
        }

        /// <summary>
        ///     Adds an entry with the specified key and value into the ordered dictionary
        ///     collection with the lowest available index.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. The value can be null.</param>
        public void Add(TKey key, TValue value)
        {
            base.Add(key, value);
        }

        /// <summary>
        ///     Inserts a new entry into the ordered dictionary
        ///     collection with the specified key and value at the specified index.
        /// </summary>
        /// <param name="index">Int32 index to insert the pair at</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. The value can be null.</param>
        public void Insert(Int32 index, TKey key, TValue value)
        {
            base.Insert(index, key, value);
        }

        /// <summary>Removes the entry with the ordered dictionary collection.</summary>
        /// <param name="key">The key of the entry to remove.</param>
        public void Remove(TKey key)
        {
            base.Remove(key);
        }
    }
}