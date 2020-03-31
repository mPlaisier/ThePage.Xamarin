using System;
using System.Collections.Generic;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests
{
    public class GenreBusinessLogicTests
    {
        public GenreBusinessLogicTests()
        {
        }

        [Fact]
        public void CheckFilledListGenreConvertedToCellGenres()
        {
            //Create List<Genre>
            var genres = new List<Genre>
            {
                new Genre("1","Fiction"),
                new Genre("2","Fantasy")
            };

            //Execute
            var cellGenres = GenreBusinessLogic.GenresToCellGenres(genres);

            //Check
            Assert.NotNull(cellGenres);
            Assert.NotEmpty(cellGenres);
        }

        [Fact]
        public void CheckEmptyListGenreConvertedToCellGenres()
        {
            //Create List<Genre>
            var genres = new List<Genre>();

            //Execute
            var cellGenres = GenreBusinessLogic.GenresToCellGenres(genres);

            //Assert
            Assert.NotNull(cellGenres);
            Assert.Empty(cellGenres);
        }

        [Fact]
        public void CheckNullListGenreConvertedToCellGenres()
        {
            //Create List<Genre>
            List<Genre> genres = null;

            //Execute
            var cellGenres = GenreBusinessLogic.GenresToCellGenres(genres);

            //Assert
            Assert.Null(cellGenres);
        }
    }
}
