using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class GenreDataFactory
    {
        public static ApiGenreResponse GetListGenre4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<ApiGenreResponse>(ListGenre4ElementsComplete);
        }

        public static ApiGenreResponse GetListGenreEmpty()
        {
            return JsonConvert.DeserializeObject<ApiGenreResponse>(ListGenreDataEmpty);
        }

        public static ApiGenre GetSingleGenre()
        {
            return JsonConvert.DeserializeObject<ApiGenre>(SingleGenre);
        }

        public static CellGenreSelect GetSingleCellGenre()
        {
            var genre = GetSingleGenre();
            return new CellGenreSelect(genre);
        }
    }
}