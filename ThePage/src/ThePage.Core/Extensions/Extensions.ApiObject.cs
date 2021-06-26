using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class ExtensionsApiOBjects
    {
        public static List<Author> SortByName(this List<Author> authors)
        {
            return authors?.OrderBy(x => x.Name).ToList();
        }

        public static List<Genre> SortByName(this List<Genre> genres)
        {
            return genres?.OrderBy(x => x.Name).ToList();
        }

        public static List<string> GetIdStrings(this IEnumerable<Genre> genres)
        {
            return genres == null ? new List<string>() : genres.Select(g => g.Id).ToList();
        }

        public static List<string> GetIdStrings(this IEnumerable<ApiGenre> genres, bool nullAllowed = false)
        {
            if (genres == null && nullAllowed)
                return null;

            return genres == null
                ? new List<string>()
                : genres.Select(g => g.Id).ToList();
        }

        public static List<string> GetIdList(this IEnumerable<ApiBook> books, bool nullAllowed = false)
        {
            if (books == null && nullAllowed)
                return null;

            return books == null
                    ? new List<string>()
                    : books.Select(b => b.Id).ToList();
        }

        public static List<string> GetIdList(this IEnumerable<Book> books, bool nullAllowed = false)
        {
            if (books == null && nullAllowed)
                return null;

            return books == null
                    ? new List<string>()
                    : books.Select(b => b.Id).ToList();
        }
    }
}
