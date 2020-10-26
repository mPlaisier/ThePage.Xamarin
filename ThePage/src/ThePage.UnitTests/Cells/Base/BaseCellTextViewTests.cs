using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Base
{
    public class BaseCellTextViewTests
    {
        [Fact]
        public void CreateBaseCellTitle()
        {
            //Execute
            var cell = new BaseCellTextView(null);

            //Assert
            cell.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("value")]
        [InlineData(null)]
        public void CheckValueIsEquivalentToInput(string value)
        {
            //Execute
            var cell = new BaseCellTextView(value, null);

            cell.TxtInput.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void CellIsIsEditFalseByDefault()
        {
            //Execute
            var cell = new BaseCellTextView(null);

            //Assert
            cell.IsEdit.Should().BeFalse();
        }

        [Theory]
        [InlineData(true, "", false)]
        [InlineData(true, "value", true)]
        [InlineData(false, "value", true)]
        [InlineData(false, "", true)]
        public void CheckCellIsValid(bool isRequired, string value, bool isValid)
        {
            //Execute
            var cell = new BaseCellTextView(value, null, isRequired);

            cell.IsValid.Should().Be(isValid);
        }
    }
}