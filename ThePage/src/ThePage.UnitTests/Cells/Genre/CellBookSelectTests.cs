using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookSelectTests
    {
        [Fact]
        public void CreateValidCellBookSelectNotSelected()
        {
            //Setup
            var cell = new CellBookSelect(BookDataFactory.GetSingleApiBook());

            //Check
            cell.Item.Should().NotBeNull();
            cell.IsSelected.Should().BeFalse();
        }

        [Fact]
        public void CreateValidBookSelectIsSelected()
        {
            //Setup
            var cell = new CellBookSelect(BookDataFactory.GetSingleApiBook(), true);

            //Check
            cell.Item.Should().NotBeNull();
            cell.IsSelected.Should().BeTrue();
        }

        [Fact]
        public void CreateNullCellBookSelectNotSelected()
        {
            //Setup
            var cell = new CellGenreSelect(null);

            //Check
            cell.Item.Should().BeNull();
            cell.IsSelected.Should().BeFalse();
        }

        [Fact]
        public void CreateNullCellBookSelectIsSelectedButShouldBeFalse()
        {
            //Setup
            var cell = new CellGenreSelect(null, true);

            //Check
            cell.Item.Should().BeNull();
            cell.IsSelected.Should().BeFalse();
        }
    }
}
