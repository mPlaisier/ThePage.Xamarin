using System;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Genre
{
    public class CellGenreSelectTests
    {
        [Fact]
        public void CreateValidAuthorSelectObjectNotSelected()
        {
            //Setup
            var cell = new CellGenreSelect(GenreDataFactory.GetSingleGenre());

            //Check
            Assert.NotNull(cell.Item);
            Assert.False(cell.IsSelected);
        }

        [Fact]
        public void CreateValidAuthorSelectObjectIsSelected()
        {
            //Setup
            var cell = new CellGenreSelect(GenreDataFactory.GetSingleGenre(), true);

            //Check
            Assert.NotNull(cell.Item);
            Assert.True(cell.IsSelected);
        }

        [Fact]
        public void CreateNullAuthorSelectObjectNotSelected()
        {
            //Setup
            var cell = new CellGenreSelect(null);

            //Check
            Assert.Null(cell.Item);
            Assert.False(cell.IsSelected);
        }

        [Fact]
        public void CreateNullAuthorSelectObjectIsSelectedButShouldBeFalse()
        {
            //Setup
            var cell = new CellGenreSelect(null, true);

            //Check
            Assert.Null(cell.Item);
            Assert.False(cell.IsSelected);
        }
    }
}