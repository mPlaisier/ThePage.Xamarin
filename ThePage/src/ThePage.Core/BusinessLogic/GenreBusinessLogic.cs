using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public class GenreBusinessLogic
    {
        #region Public

        public static List<CellGenreSelect> GenresToCellGenres(List<ApiGenre> genreApi)
        {
            return genreApi?.Select(genre => new CellGenreSelect(genre)).ToList();
        }

        public static IEnumerable<Genre> GetGenresFromString(IEnumerable<string> bookGenres, IEnumerable<Genre> genres)
        {
            return genres?.Where(g => bookGenres.Contains(g.Id));
        }

        #endregion
    }
}