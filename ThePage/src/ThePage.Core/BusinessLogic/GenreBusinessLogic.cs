using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public class GenreBusinessLogic
    {
        #region Public

        public static List<CellGenre> GenresToCellGenres(List<Genre> genreApi)
        {
            return genreApi?.Select(genre => new CellGenre(genre)).ToList();
        }

        public static IEnumerable<Genre> GetGenresFromString(IEnumerable<string> bookGenres, IEnumerable<Genre> genres)
        {
            return genres?.Where(g => bookGenres.Contains(g.Id));
        }

        #endregion
    }
}