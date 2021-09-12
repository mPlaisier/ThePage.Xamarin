using System.Collections.Generic;
using System.Linq;
using ThePage.Api;

namespace ThePage.Core
{
    public static class GenreBusinessLogic
    {
        #region Public static

        public static IEnumerable<Genre> MapGenres(IEnumerable<ApiGenre> apiGenres)
        {
            return apiGenres.Select(author => MapGenre(author));
        }

        public static Genre MapGenre(ApiGenre genre)
        {
            return new Genre()
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        #endregion
    }
}