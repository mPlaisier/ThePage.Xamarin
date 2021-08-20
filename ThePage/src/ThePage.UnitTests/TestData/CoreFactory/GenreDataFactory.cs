using System;
using System.Collections.Generic;
using Bogus;
using ThePage.Core;

namespace ThePage.UnitTests
{
    public static partial class GenreDataFactory
    {
        public static Genre GetSingleGenre()
        {
            return GetSingleFakeGenreObject().Generate();
        }

        public static IEnumerable<Genre> GetGenre4ElementsComplete()
        {
            return GetSingleFakeGenreObject().Generate(4);
        }

        public static CellGenreSelect GetSingleCellGenre()
        {
            var genre = GetSingleGenre();
            return new CellGenreSelect(genre);
        }

        #region Private

        static Faker<Genre> GetSingleFakeGenreObject()
        {
            return new Faker<Genre>()
               .RuleFor(g => g.Id, f => Guid.NewGuid().ToString())
               .RuleFor(g => g.Name, f => f.Name.FirstName());
        }

        #endregion
    }
}