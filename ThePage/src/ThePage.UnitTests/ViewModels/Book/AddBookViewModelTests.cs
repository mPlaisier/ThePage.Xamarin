using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

        void PrepareAuthorData()
        {
            MockAuthorService
               .Setup(x => x.GetAuthors())
               .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
        }

        #endregion


        [Fact]
        public void CheckBookCellItemsCountWithoutVMParameter()
        {
            //Setup
            PrepareAuthorData();

            //Execute
            _vm.Initialize();

            //Assert
            Assert.Equal(8, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookCellItemsCountWithVMParameter()
        {
            //Setup
            PrepareAuthorData();

            var isbn = "5456554412";
            var olObject = BookDataFactory.GetSingleOLObject();
            var param = new AddBookParameter(isbn, olObject);

            //Execute
            LoadViewModel(param);

            //Assert
            Assert.Equal(8, _vm.Items.Count);
        }

        [Fact]
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithoutVmParameter()
        {
            //Setup
            PrepareAuthorData();

            //Execute
            _vm.Initialize();

            //Assert
            _vm.Items[0].Should().BeOfType<CellBasicBook>();
            _vm.Items[1].Should().BeOfType<CellBookTitle>();
            _vm.Items[2].Should().BeOfType<CellBookAddGenre>();
            _vm.Items[3].Should().BeOfType<CellBookNumberTextView>();
            _vm.Items[4].Should().BeOfType<CellBookNumberTextView>();
            _vm.Items[5].Should().BeOfType<CellBookSwitch>();
            _vm.Items[6].Should().BeOfType<CellBookSwitch>();
            _vm.Items[7].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithVmParameter()
        {
            //Setup
            PrepareAuthorData();

            var isbn = "5456554412";
            var olObject = BookDataFactory.GetSingleOLObject();
            var param = new AddBookParameter(isbn, olObject);

            //Execute
            LoadViewModel(param);

            //Assert
            _vm.Items[0].Should().BeOfType<CellBasicBook>();
            _vm.Items[1].Should().BeOfType<CellBookTitle>();
            _vm.Items[2].Should().BeOfType<CellBookAddGenre>();
            _vm.Items[3].Should().BeOfType<CellBookNumberTextView>();
            _vm.Items[4].Should().BeOfType<CellBookNumberTextView>();
            _vm.Items[5].Should().BeOfType<CellBookSwitch>();
            _vm.Items[6].Should().BeOfType<CellBookSwitch>();
            _vm.Items[7].Should().BeOfType<CellBookButton>();
        }

        [Fact]
        public void ISBNFilledInIfViewModelStartedWithParameter()
        {
            //Setup
            var isbn = "5456554412";
            var param = new AddBookParameter(isbn, null);

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
            PrepareAuthorData();

            //Execute
            _vm.Initialize();

            //Assert
            Assert.False(_vm.IsLoading);
        }

        [Fact]
        public void ButtonsAreDisabledAfterInitialize()
        {
            //Setup
            PrepareAuthorData();

            //Execute
            _vm.Initialize();

            //Assert
            var buttons = _vm.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            foreach (var item in buttons)
                Assert.False(item.IsValid);
        }

        [Theory, MemberData(nameof(InputDataForBooks))]
        public void CheckIfButtonsHaveCorrectValidationAfterInput(string title, Core.Author author, string pages, bool isValid)
        {
            //Setup
            PrepareAuthorData();
            _vm.Initialize();

            //Execute
            var cellBasicInfo = _vm.Items.OfType<CellBasicBook>().Where(x => x.InputType == EBookInputType.BasicInfo).First();
            cellBasicInfo.TxtTitle = title;

            cellBasicInfo.Author = author;

            var cellPages = _vm.Items.OfType<CellBookNumberTextView>().Where(x => x.InputType == EBookInputType.Pages).First();
            cellPages.TxtInput = pages;

            //Assert
            var buttons = _vm.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
            foreach (var item in buttons)
                item.IsValid.Should().Be(isValid, $"title is {title}, author is {author} and pages is {pages}");
        }

        public static IEnumerable<object[]> InputDataForBooks =>
               new[]
               {
                new object[] {"Valid name",  AuthorDataFactory.GetSingleAuthor(), "100", true },
                new object[] { "", null, "", false },
                new object[] { null, null, null, false },
                new object[] { "Valid name", null, "", false },
                new object[] { "", null, "100", false },
                new object[] { null, null, "100", false },
                new object[] { "Valid name", null, null, false },
                new object[] { null, AuthorDataFactory.GetSingleAuthor(), null, false },
                new object[] { "Valid name", AuthorDataFactory.GetSingleAuthor(), null, true },
                new object[] { null, AuthorDataFactory.GetSingleAuthor(), "100", false }
        };
    }
}