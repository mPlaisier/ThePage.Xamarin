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
            return genreApi.Select(x => new CellGenre(x.Id, x.Name)).ToList();
        }

        public static Genre CellGenreToGenre(CellGenre cellGenre)
        {
            return new Genre(cellGenre.Id, cellGenre.Name);
        }

        #endregion
    }
}
