using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ThePage.Api;

namespace ThePage.UnitTests
{
    public static partial class GenreDataFactory
    {
        public static List<Genre> GetGenre4ElementsComplete()
        {
            return JsonConvert.DeserializeObject<List<Genre>>(Genre4ElementsComplete);
        }

        public static List<Genre> GetGenreEmpty()
        {
            return JsonConvert.DeserializeObject<List<Genre>>(GenreDataEmpty);
        }
    }
}