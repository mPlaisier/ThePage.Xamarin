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

        #endregion
    }
}