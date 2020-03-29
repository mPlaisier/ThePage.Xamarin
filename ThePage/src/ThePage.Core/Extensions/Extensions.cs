using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
