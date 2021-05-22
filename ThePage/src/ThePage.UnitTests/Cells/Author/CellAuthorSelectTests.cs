using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Author
{
    public class CellAuthorSelectTests
    {
        [Fact]
        public void CreateValidAuthorSelectObjectNotSelected()
        {
            //Setup
            var cell = new CellAuthorSelect(AuthorDataFactory.GetSingleAuthor());

            //Check
            Assert.NotNull(cell.Item);
            Assert.True(cell.IsSelected);
        }

        [Fact]
        public void CreateValidAuthorSelectObjectIsSelected()
        {
            //Setup
            var cell = new CellAuthorSelect(AuthorDataFactory.GetSingleAuthor(), true);

            //Check
            Assert.NotNull(cell.Item);
            Assert.True(cell.IsSelected);
        }

        [Fact]
        public void CreateNullAuthorSelectObjectNotSelected()
        {
            //Setup
            var cell = new CellAuthorSelect(null);

            //Check
            Assert.Null(cell.Item);
            Assert.False(cell.IsSelected);
        }

        [Fact]
        public void CreateNullAuthorSelectObjectIsSelectedButShouldBeFalse()
        {
            //Setup
            var cell = new CellAuthorSelect(null, true);

            //Check
            Assert.Null(cell.Item);
            Assert.False(cell.IsSelected);
        }
    }
}