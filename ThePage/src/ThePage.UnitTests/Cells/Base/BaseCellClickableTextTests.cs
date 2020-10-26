using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.Cells.Base
{
    public class BaseCellClickableTextTests
    {
        [Fact]
        public void CreateBBaseCellClickableText()
        {
            //Execute
            var cell = new BaseCellClickableText("label", null);

            //Assert
            cell.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("value")]
        [InlineData(null)]
        public void CheckLabelIsEquivalentToInput(string value)
        {
            //Execute
            var cell = new BaseCellClickableText(value, null);

            cell.Label.Should().BeEquivalentTo(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ic_add")]
        [InlineData("ic_delete")]
        public void CheckInconIsEquivalentToInput(string icon)
        {
            //Execute
            var cell = new BaseCellClickableText("", null, icon);

            cell.Icon.Should().BeEquivalentTo(icon);
        }

        [Fact]
        public void IconIsCorrectAddIconByDefault()
        {
            //Execute
            var cell = new BaseCellClickableText("label", null);

            //Assert
            cell.Icon.Should().BeEquivalentTo("ic_add");
        }
    }
}
