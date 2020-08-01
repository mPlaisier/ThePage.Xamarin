using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class ExtensionsApiOBjects
    {
        #region Book

        public static List<Book> SortByTitle(this List<Book> books)
        {
            return books?.OrderBy(x => x.Title).ToList();
        }

        #endregion

        #region Author

        public static List<Author> SortByName(this List<Author> authors)
        {
            return authors?.OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region Genre

        public static List<string> GetIdStrings(this IEnumerable<Genre> genres)
        {
            return genres == null ? new List<string>() : genres.Select(g => g.Id).ToList();
        }

        public static List<string> GetIdStrings(this IEnumerable<ApiGenre> genres)
        {
            return genres == null ? new List<string>() : genres.Select(g => g.Id).ToList();
        }

        public static List<Genre> SortByName(this List<Genre> genres)
        {
            return genres?.OrderBy(x => x.Name).ToList();
        }

        #endregion
    }
}
