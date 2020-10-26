using System.Linq;
using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.BusinessLogic
{
    public class BookBusinessLogicTests
    {
        [Fact]
        public void CellsBookDetailWithGenresContainsEnoughItemsTest()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseWithGenres();

            //Execute
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null);

            //Assert
            items.Should().NotBeNullOrEmpty();
            var genreCount = response.Genres.Count;
            items.Should().HaveCount(genreCount + 8);
        }

        [Fact]
        public void CellsBookDetailWithGenresContainsCorrectCellsTest()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseWithGenres();

            //Execute
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null).ToList();

            //Assert
            items.Should().NotBeNullOrEmpty();
            items[0].Should().BeOfType<CellBookTextView>();
            items[1].Should().BeOfType<CellBookAuthor>();
            items[2].Should().BeOfType<CellBookTitle>();
            items[3].Should().BeOfType<CellBookGenreItem>();
            items[4].Should().BeOfType<CellBookGenreItem>();
            items[5].Should().BeOfType<CellBookNumberTextView>();
            items[6].Should().BeOfType<CellBookNumberTextView>();
            items[7].Should().BeOfType<CellBookSwitch>();
            items[8].Should().BeOfType<CellBookSwitch>();
            items[9].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void CellsBookDetailWithoutGenresContainsEnoughItemsTest()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseNoGenres();

            //Execute
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null);

            //Assert
            items.Should().NotBeNullOrEmpty();
            items.Should().HaveCount(8);
        }

        [Fact]
        public void CellsBookDetailWithoutGenresContainsCorrectCellsTest()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseNoGenres();

            //Execute
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null).ToList();

            //Assert
            items.Should().NotBeNullOrEmpty();
            items[0].Should().BeOfType<CellBookTextView>();
            items[1].Should().BeOfType<CellBookAuthor>();
            items[2].Should().BeOfType<CellBookTitle>();
            items[3].Should().BeOfType<CellBookNumberTextView>();
            items[4].Should().BeOfType<CellBookNumberTextView>();
            items[5].Should().BeOfType<CellBookSwitch>();
            items[6].Should().BeOfType<CellBookSwitch>();
            items[7].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void RequestShouldBeNullWithNoChanges()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseWithGenres();
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null);

            //Execute
            var result = BookBusinessLogic.CreateBookDetailRequestFromInput(items, null, response);

            //Assert
            result.request.Should().BeNull();
        }

        [Fact]
        public void RequestShouldNotBeNullWithChanges()
        {
            //Setup
            var response = BookDataFactory.GetApiBookDetailResponseWithGenres();
            var items = BookBusinessLogic.CreateCellsBookDetail(response, null, null, null, null, null).ToList();

            ((CellBookTextView)items[0]).TxtInput = "Change title";

            //Execute
            var result = BookBusinessLogic.CreateBookDetailRequestFromInput(items, null, response);

            //Assert
            result.request.Should().NotBeNull();
        }
    }
}