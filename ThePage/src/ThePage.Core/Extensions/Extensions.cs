using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using ThePage.Api;

namespace ThePage.Core
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Ignores all exceptions the task, or only the ones as specified
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="acceptableExceptions">Acceptable exceptions.</param>
        public static async void Forget(this Task task, params Type[] acceptableExceptions)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Consider whether derived types are also acceptable.
                if (!acceptableExceptions.Contains(ex.GetType()))
                    throw;
            }
        }

        public static void Forget(this Task task)
        {
        }

        public static List<string> GetIdAsStringList(this List<Genre> genres)
        {
            return genres == null ? new List<string>() : genres.Select(g => g.Id).ToList();
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

            var item = collection.Where(predicate).First();
            return collection.IndexOf(item);
        }

        /// <summary>
        /// Determines whether is collection is null or contains no elements.
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
    }
}
