using System;
using System.Linq;
using System.Threading.Tasks;
using ThePage.Core;
using Xunit;
using static ThePage.Core.CellBookInput;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class BookDetailViewModelTests : BaseViewModelTests
    {
        readonly BookDetailViewModel _vm;

        #region Constructor

        public BookDetailViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<BookDetailViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(BookDetailParameter parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        void PrepareAuthorAndGenreData()
        {
            MockThePageService
               .Setup(x => x.GetAllGenres())
               .Returns(() => Task.FromResult(GenreDataFactory.GetListGenre4ElementsComplete()));

            MockThePageService
               .Setup(x => x.GetAllAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
        }

        #endregion

        [Fact]
        public void CheckBookCellItemsCountWithGenres()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            //Assert
            Assert.Equal(10, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookCellItemsCountWithoutGenres()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWithoutGenres()));
            _vm.Initialize();

            //Assert
            Assert.Equal(8, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookCellItemsCountWithGenresAfterEdit()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Assert
            Assert.Equal(11, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookCellItemsCountWithoutGenresAfterEdit()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWithoutGenres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Assert
            Assert.Equal(9, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookWithGenresCellItemsTypesAndOrder()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            //Assert
            Assert.IsType<CellBookTextView>(_vm.Items[0]);
            Assert.IsType<CellBookAuthor>(_vm.Items[1]);
            Assert.IsType<CellBookTitle>(_vm.Items[2]);
            Assert.IsType<CellBookGenreItem>(_vm.Items[3]);
            Assert.IsType<CellBookGenreItem>(_vm.Items[4]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[5]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[6]);
            Assert.IsType<CellBookSwitch>(_vm.Items[7]);
            Assert.IsType<CellBookSwitch>(_vm.Items[8]);
            Assert.IsType<CellBookButton>(_vm.Items[9]);
        }

        [Fact]
        public void CheckBookWithoutGenresCellItemsTypesAndOrder()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWithoutGenres()));
            _vm.Initialize();

            //Assert
            Assert.IsType<CellBookTextView>(_vm.Items[0]);
            Assert.IsType<CellBookAuthor>(_vm.Items[1]);
            Assert.IsType<CellBookTitle>(_vm.Items[2]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[3]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[4]);
            Assert.IsType<CellBookSwitch>(_vm.Items[5]);
            Assert.IsType<CellBookSwitch>(_vm.Items[6]);
            Assert.IsType<CellBookButton>(_vm.Items[7]);
        }

        [Fact]
        public void CheckBookWithGenresCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Assert
            Assert.IsType<CellBookTextView>(_vm.Items[0]);
            Assert.IsType<CellBookAuthor>(_vm.Items[1]);
            Assert.IsType<CellBookTitle>(_vm.Items[2]);
            Assert.IsType<CellBookGenreItem>(_vm.Items[3]);
            Assert.IsType<CellBookGenreItem>(_vm.Items[4]);
            Assert.IsType<CellBookAddGenre>(_vm.Items[5]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[6]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[7]);
            Assert.IsType<CellBookSwitch>(_vm.Items[8]);
            Assert.IsType<CellBookSwitch>(_vm.Items[9]);
            Assert.IsType<CellBookButton>(_vm.Items[10]);
        }

        [Fact]
        public void CheckBookWithoutGenresCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWithoutGenres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Assert
            Assert.IsType<CellBookTextView>(_vm.Items[0]);
            Assert.IsType<CellBookAuthor>(_vm.Items[1]);
            Assert.IsType<CellBookTitle>(_vm.Items[2]);
            Assert.IsType<CellBookAddGenre>(_vm.Items[3]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[4]);
            Assert.IsType<CellBookNumberTextView>(_vm.Items[5]);
            Assert.IsType<CellBookSwitch>(_vm.Items[6]);
            Assert.IsType<CellBookSwitch>(_vm.Items[7]);
            Assert.IsType<CellBookButton>(_vm.Items[8]);
        }

        [Fact]
        public void BookDetailEditIsDisabledAStart()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            //Assert
            Assert.False(_vm.IsEditing);
        }

        [Fact]
        public void BookDetailEditIsEnabledAferEditCommand()
        {
            //Arrange
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Assert
            Assert.True(_vm.IsEditing);
        }

        [Theory]
        [InlineData("Valid name", "100", true)]
        [InlineData("", "", false)]
        [InlineData(null, null, false)]
        [InlineData("Valid name", "", false)]
        [InlineData("", "100", false)]
        [InlineData(null, "100", false)]
        [InlineData("Valid name", null, false)]
        public void CheckIfButtonsHaveCorrectValidationAfterInput(string title, string pages, bool isValid)
        {
            //Setup
            PrepareAuthorAndGenreData();
            LoadViewModel(new BookDetailParameter(BookDataFactory.GetSingleCellBookWith2Genres()));
            _vm.Initialize();

            _vm.EditBookCommand.Execute();

            //Execute
            var cellTitle = _vm.Items.OfType<CellBookTextView>().Where(x => x.InputType == EBookInputType.Title).First();
            cellTitle.TxtInput = title;

            var cellPages = _vm.Items.OfType<CellBookNumberTextView>().Where(x => x.InputType == EBookInputType.Pages).First();
            cellPages.TxtInput = pages;

            //Assert
            var buttons = _vm.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            foreach (var item in buttons)
                Assert.Equal(isValid, item.IsValid);
        }

    }
}
