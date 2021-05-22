using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ThePage.Core
{
    public static class ExtensionsIEnumerable
    {
        /// <summary>
        /// Determines whether the <paramref name="enumerable"/> is null or contains no elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N). */
            return enumerable is ICollection<T> collection ? collection.Count < 1 : !enumerable.Any();
        }

        /// <summary>
        /// Determines whether the collection is not null and contains elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNotNullAndHasItems<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return false;

            /* If this is a list, use the Count property for efficiency. 
            * The Count property is O(1) while IEnumerable.Count() is O(N). */
            return enumerable is ICollection<T> collection ? collection.Count > 0 : enumerable.Any();
        }

        public static void ForEach<T>(this IEnumerable<T> @enumerable, Action<T> mapFunction)
        {
            if (enumerable.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(enumerable));

            if (enumerable is List<T> collection)
                collection.ForEach(mapFunction);
            else
            {
                foreach (var item in @enumerable)
                    mapFunction(item);
            }
        }

        public static void ForEachType<T, S>(this IEnumerable<T> @enumerable, Action<S> mapFunction) where S : T
        {
            enumerable.OfType<S>().ForEach(mapFunction);
        }

        /// <summary>
        /// Find the Index of a certain item with a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this Collection<T> collection, Func<T, bool> predicate)
        {
            if (collection.IsNullOrEmpty())
                return -1;

            var item = collection.FirstOrDefault(predicate);
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Adds the elements at index of the collection. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index"></param>
        /// <param name="enumerable"></param>
        public static void InsertRange<T>(this Collection<T> collection, int index, IEnumerable<T> enumerable)
        {
            int currentIndex = index;
            var changedItems = collection is List<T> ? (List<T>)enumerable : new List<T>(enumerable);
            foreach (var i in changedItems)
            {
                collection.Insert(currentIndex, i);
                currentIndex++;
            }
        }

        /// <summary>
        /// Adds the elements at index of the collection. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index"></param>
        /// <param name="enumerable"></param>
        public static void InsertRange<T>(this ObservableCollection<T> collection, int index, IEnumerable<T> enumerable)
        {
            int currentIndex = index;
            var changedItems = collection is List<T> ? (List<T>)enumerable : new List<T>(enumerable);
            foreach (var i in changedItems)
            {
                collection.Insert(currentIndex, i);
                currentIndex++;
            }
        }

        /// <summary>
        /// Removes the first occurence of each item in the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="enumerable"></param>
        public static void RemoveRange<T>(this Collection<T> collection, IEnumerable<T> enumerable)
        {
            if (enumerable.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(enumerable));

            //fix error when enumerable is from the Collection
            //Error: Collection was modified; enumeration operation may not execute
            foreach (var item in enumerable.ToList())
                collection.Remove(item);
        }
    }
}
