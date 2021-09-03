using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ThePage.Core;
using ThePage.Core.Cells;
using Xunit;
using static ThePage.Core.Enums;

namespace ThePage.UnitTests.Services.Book.ScreenManager
{
    public class BookDetailScreenManagerServiceTests : BaseServicesTests
    {
        readonly BookDetailScreenManagerService _screenManager;

        #region Constructor

        public BookDetailScreenManagerServiceTests()
        {
            Setup();

            _screenManager = Ioc.IoCConstruct<BookDetailScreenManagerService>();
        }

        #endregion

        #region Setup

        void LoadData()
        {
            _screenManager.Init(new BookDetailParameter(BookDataFactory.GetSingleBook()), It.IsAny<Action>());
            _screenManager.FetchData();
        }

        void PrepareThePageServiceResults(bool containsGenres = true)
        {
            MockGenreService
               .Setup(x => x.GetGenres())
               .Returns(() => Task.FromResult(GenreDataFactory.GetGenre4ElementsComplete()));

            MockAuthorService
               .Setup(x => x.GetAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));

            if (containsGenres)
                MockBookService
                    .Setup(x => x.FetchBook(It.IsAny<string>()))
                    .Returns(() => Task.FromResult(BookDataFactory.GetBookDetailWithGenres()));
            else
                MockBookService
                    .Setup(x => x.FetchBook(It.IsAny<string>()))
                    .Returns(() => Task.FromResult(BookDataFactory.GetBookDetailNoGenres()));
        }

        #endregion

        [Fact]
        public void CheckBookCellItemsCountWithGenres()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();

            //Assert
            _screenManager.Items.Should().HaveCount(11);
        }

        [Fact]
        public void CheckBookCellItemsCountWithoutGenres()
        {
            //Arrange
            PrepareThePageServiceResults(false);

            //Execute
            LoadData();

            //Assert
            _screenManager.Items.Should().HaveCount(7);
        }

        [Fact]
        public void CheckBookCellItemsCountWithGenresAfterEdit()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();
            _screenManager.ToggleEditValue();

            //Assert
            _screenManager.Items.Should().HaveCount(12);
        }

        [Fact]
        public void CheckBookCellItemsCountWithoutGenresAfterEdit()
        {
            //Arrange
            PrepareThePageServiceResults(false);

            //Execute
            LoadData();
            _screenManager.ToggleEditValue();

            //Assert
            _screenManager.Items.Should().HaveCount(8);
        }

        [Fact]
        public void CheckBookWithGenresCellItemsTypesAndOrder()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();

            //Assert
            var items = _screenManager.Items;
            items[0].Should().BeOfType<CellBasicBook>();
            items[1].Should().BeOfType<CellBookTitle>();
            items[2].Should().BeOfType<CellBookGenreItem>();
            items[3].Should().BeOfType<CellBookGenreItem>();
            items[4].Should().BeOfType<CellBookGenreItem>();
            items[5].Should().BeOfType<CellBookGenreItem>();
            items[6].Should().BeOfType<CellBookNumberTextView>();
            items[7].Should().BeOfType<CellBookNumberTextView>();
            items[8].Should().BeOfType<CellBookSwitch>();
            items[9].Should().BeOfType<CellBookSwitch>();
            items[10].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void CheckBookWithGenresCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();
            _screenManager.ToggleEditValue();

            //Assert
            var items = _screenManager.Items;
            items[0].Should().BeOfType<CellBasicBook>();
            items[1].Should().BeOfType<CellBookTitle>();
            items[2].Should().BeOfType<CellBookGenreItem>();
            items[3].Should().BeOfType<CellBookGenreItem>();
            items[4].Should().BeOfType<CellBookGenreItem>();
            items[5].Should().BeOfType<CellBookGenreItem>();
            items[6].Should().BeOfType<CellBookAddGenre>();
            items[7].Should().BeOfType<CellBookNumberTextView>();
            items[8].Should().BeOfType<CellBookNumberTextView>();
            items[9].Should().BeOfType<CellBookSwitch>();
            items[10].Should().BeOfType<CellBookSwitch>();
            items[11].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void CheckBookWithoutGenresCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            PrepareThePageServiceResults(false);

            //Execute
            //Execute
            LoadData();
            _screenManager.ToggleEditValue();

            //Assert
            var items = _screenManager.Items;
            items[0].Should().BeOfType<CellBasicBook>();
            items[1].Should().BeOfType<CellBookTitle>();
            items[2].Should().BeOfType<CellBookAddGenre>();
            items[3].Should().BeOfType<CellBookNumberTextView>();
            items[4].Should().BeOfType<CellBookNumberTextView>();
            items[5].Should().BeOfType<CellBookSwitch>();
            items[6].Should().BeOfType<CellBookSwitch>();
            items[7].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void BookDetailEditIsDisabledAStart()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();

            //Assert
            _screenManager.IsEditing.Should().BeFalse();
        }

        [Fact]
        public void BookDetailEditIsEnabledAferEditToggle()
        {
            //Arrange
            PrepareThePageServiceResults();

            //Execute
            LoadData();
            _screenManager.ToggleEditValue();

            //Assert
            _screenManager.IsEditing.Should().BeTrue();
        }

        [Theory]
        [InlineData("Valid name", "100", true)]
        [InlineData("", "", false)]
        [InlineData(null, null, false)]
        [InlineData("Valid name", "", true)]
        [InlineData("", "100", false)]
        [InlineData(null, "100", false)]
        [InlineData("Valid name", null, true)]
        public void CheckIfButtonsHaveCorrectValidationAfterInput(string title, string pages, bool isValid)
        {
            //Setup
            PrepareThePageServiceResults();
            LoadData();
            _screenManager.ToggleEditValue();

            //Execute
            var cellBasicBook = _screenManager.Items.OfType<CellBasicBook>()
                                                .Where(x => x.InputType == EBookInputType.BasicInfo)
                                                .First();
            cellBasicBook.TxtTitle = title;

            var cellPages = _screenManager.Items.OfType<CellBookNumberTextView>()
                                                .Where(x => x.InputType == EBookInputType.Pages)
                                                .First();
            cellPages.TxtInput = pages;

            //Assert
            var buttons = _screenManager.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            foreach (var item in buttons)
                item.IsValid.Should().Be(isValid);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadingAndEditingToFalseAfterSuccessConfirm(bool result)
        {
            //Arrange
            PrepareThePageServiceResults();
            MockBookService
               .Setup(x => x.UpdateBook(It.IsAny<Api.ApiBookDetailRequest>()))
               .Returns(() => Task.FromResult(result));
            LoadData();

            //Set view as Edit so we get the Update button
            _screenManager.ToggleEditValue();

            var cellBtn = _screenManager.Items.OfType<CellBookButton>().First();
            cellBtn.ClickCommand.Execute();

            //Assert
            _screenManager.IsEditing.Should().BeFalse();
            _screenManager.IsLoading.Should().BeFalse();
        }
    }
}
