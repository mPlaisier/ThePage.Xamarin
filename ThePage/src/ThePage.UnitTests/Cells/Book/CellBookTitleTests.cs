using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Book
{
    public class CellBookTitleTests
    {
        [Fact]
        public void CreateValidTitleCell()
        {
            //Setup
            var title = "Title";
            var cell = new CellBookTitle(title);

            //Check
            Assert.Equal(title, cell.LblTitle);
        }

        [Fact]
        public void CreateEmptyTitleCell()
        {
            //Setup
            var title = string.Empty;
            var cell = new CellBookTitle(title);

            //Check
            Assert.Empty(cell.LblTitle);
        }

        [Fact]
        public void CreateNullTitleCell()
        {
            //Setup
            var cell = new CellBookTitle(null);

            //Check
            Assert.Null(cell.LblTitle);
        }
    }
}