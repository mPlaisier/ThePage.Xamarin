using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ThePage.Core;
using Xunit;
using static ThePage.Core.CellBookInput;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class AddBookViewModelTests : BaseViewModelTests
    {
        readonly AddBookViewModel _vm;

        #region Constructor

        public AddBookViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<AddBookViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(AddBookParameter parameter)
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
        public void CheckBookCellItemsCountWithoutVMParameter()
        {
            //Setup
            PrepareAuthorAndGenreData();

            //Execute
            _vm.Initialize();

            //Assert
            Assert.Equal(9, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookCellItemsCountWithVMParameter()
        {
            //Setup
            PrepareAuthorAndGenreData();

            var isbn = "5456554412";
            var olObject = BookDataFactory.GetSingleOLObject();
            var param = new AddBookParameter(isbn, olObject);

            //Execute
            LoadViewModel(param);

            //Assert
            Assert.Equal(9, _vm.Items.Count);
        }

        [Fact]
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithoutVmParameter()
        {
            //Setup
            PrepareAuthorAndGenreData();

            //Execute
            _vm.Initialize();

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
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithVmParameter()
        {
            //Setup
            PrepareAuthorAndGenreData();

            var isbn = "5456554412";
            var olObject = BookDataFactory.GetSingleOLObject();
            var param = new AddBookParameter(isbn, olObject);

            //Execute
            LoadViewModel(param);

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
        public void ISBNFilledInIfViewModelStartedWithParameter()
        {
            //Setup
            var isbn = "5456554412";
            AddBookParameter param = new AddBookParameter(isbn, null);

            //Execute
            LoadViewModel(param);

            //Assert
            var item = _vm.Items.OfType<CellBookNumberTextView>().Where(x => x.InputType == EBookInputType.ISBN).First();
            Assert.Equal(isbn, item.TxtInput);
        }

        [Fact]
        public void IsLoadingSetToFalseAfterInitialize()
        {
            //Setup
            PrepareAuthorAndGenreData();

            //Execute
            _vm.Initialize();

            //Assert
            Assert.False(_vm.IsLoading);
        }

        [Fact]
        public void ButtonsAreDisabledAfterInitialize()
        {
            //Setup
            PrepareAuthorAndGenreData();

            //Execute
            _vm.Initialize();

            //Assert
            var buttons = _vm.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            foreach (var item in buttons)
                Assert.False(item.IsValid);
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
            _vm.Initialize();

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LoadingSetCorrectAfterAddBook(bool result)
        {
            //Arrange
            MockThePageService
               .Setup(x => x.AddBook(It.IsAny<Api.Book>()))
               .Returns(() => Task.FromResult(result));

            PrepareAuthorAndGenreData();
            _vm.Initialize();

            //Execute
            var cellTitle = _vm.Items.OfType<CellBookTextView>().Where(x => x.InputType == EBookInputType.Title).First();
            cellTitle.TxtInput = "Valid Name";

            var cellPages = _vm.Items.OfType<CellBookNumberTextView>().Where(x => x.InputType == EBookInputType.Pages).First();
            cellPages.TxtInput = "500";


            //Execute
            _vm.AddBookCommand.Execute();

            //Assert
            Assert.Equal(result, _vm.IsLoading);
        }
    }
}