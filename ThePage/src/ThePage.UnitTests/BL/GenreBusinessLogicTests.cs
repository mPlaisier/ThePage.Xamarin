using System.Collections.Generic;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.BusinessLogic
{
    public partial class GenreBusinessLogicTests
    {
        [Fact]
        public void CheckFilledListGenreConvertedToCellGenres()
        {
            //Create List<Genre>
            var genres = GenreDataFactory.GetListGenre4ElementsComplete();

            //Execute
            var cellGenres = GenreBusinessLogic.GenresToCellGenres(genres);

            //Check
            Assert.NotNull(cellGenres);
            Assert.NotEmpty(cellGenres);
            Assert.Equal(genres.Count, cellGenres.Count);
        }

        [Fact]
        public void CheckEmptyListGenreConvertedToCellGenres()
        {
            //Create List<Genre>
            var genres = GenreDataFactory.GetListGenreEmpty();

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
