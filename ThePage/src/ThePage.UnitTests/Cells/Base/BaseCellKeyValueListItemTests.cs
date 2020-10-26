using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Base
{
    public class BaseCellKeyValueListItemTests
    {
        [Fact]
        public void CreateBaseCellKeyValueListItem()
        {
            //Execute
            var cell = new BaseCellKeyValueListItem("key", "value", null);

            //Assert
            cell.Should().NotBeNull();
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("key", "value")]
        [InlineData(null, null)]
        public void CheckKeyAndValueIsCorrect(string key, string value)
        {
            //Execute
            var cell = new BaseCellKeyValueListItem(key, value, null);

            cell.Key.Should().BeEquivalentTo(key);
            cell.Value.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void BaseCellKeyValueListItemIsBaseCellInput()
        {
            //Execute
            var cell = new BaseCellTitle("");

            //Assert
            cell.Should().BeAssignableTo<ICell>();
        }

        [Fact]
        public void IconIsCorrectDeleteIconByDefault()
        {
            //Execute
            var cell = new BaseCellKeyValueListItem("key", "value", null);

            //Assert
            cell.Icon.Should().BeEquivalentTo("ic_delete");
        }

        [Fact]
        public void CellIsIsEditFalseByDefault()
        {
            //Execute
            var cell = new BaseCellKeyValueListItem("key", "value", null);

            //Assert
            cell.IsEdit.Should().BeFalse();
        }
    }
}
