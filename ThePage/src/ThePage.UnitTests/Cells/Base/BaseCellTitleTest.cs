using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Base
{
    public class BaseCellTitleTest
    {
        [Fact]
        public void CreateBaseCellTitle()
        {
            //Execute
            var cell = new BaseCellTitle("");

            //Assert
            cell.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("title")]
        [InlineData(null)]
        public void CheckTitleValueIsCorrect(string value)
        {
            //Execute
            var cell = new BaseCellTitle(value);

            cell.Title.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void BaseCellTitleIsICellObject()
        {
            //Execute
            var cell = new BaseCellTitle("");

            //Assert
            cell.Should().BeAssignableTo<ICell>();
        }
    }
}
