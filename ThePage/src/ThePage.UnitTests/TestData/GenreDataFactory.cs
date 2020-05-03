using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class GenreDataFactory
    {
        public static List<Genre> GetListGenre4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<List<Genre>>(ListGenre4ElementsComplete);
        }

        public static List<Genre> GetListGenreEmpty()
        {
            return JsonConvert.DeserializeObject<List<Genre>>(ListGenreDataEmpty);
        }

        public static Genre GetSingleGenre()
        {
            return JsonConvert.DeserializeObject<Genre>(SingleGenre);
        }

        public static CellGenre GetSingleCellGenre()
        {
            var genre = GetSingleGenre();
            return new CellGenre(genre);
        }
    }
}