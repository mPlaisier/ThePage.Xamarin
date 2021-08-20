using System;
using System.Collections.Generic;
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
    public class AddBookScreenManagerServiceTests : BaseServicesTests
    {
        readonly AddBookScreenManagerService _screenManager;

        #region Constructor

        public AddBookScreenManagerServiceTests()
        {
            Setup();

            _screenManager = Ioc.IoCConstruct<AddBookScreenManagerService>();
        }

        #endregion

        #region Setup

        void LoadData(AddBookParameter parameter)
        {
            _screenManager.Init(parameter, It.IsAny<Action<string>>());
            _screenManager.FetchData();
        }

        void PrepareAuthorData(bool containsGenres = true)
        {
            MockAuthorService
             .Setup(x => x.GetAuthors())
             .Returns(() => Task.FromResult(AuthorDataFactory.GetListAuthor4ElementsComplete()));
        }

        #endregion

        [Fact]
        public void CheckBookCellItemsCountWithoutVMParameter()
        {
            //Execute
            LoadData(null);

            //Assert
            _screenManager.Items.Should().HaveCount(8);
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
            LoadData(param);

            //Assert
            _screenManager.Items.Should().HaveCount(8);
        }

        [Fact]
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithoutVmParameter()
        {
            //Setup

            //Execute
            LoadData(null);

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
        public void CheckCellBookTypesAreCreatedInCorrectPositionWithVmParameter()
        {
            //Setup
            PrepareAuthorData();

            var isbn = "5456554412";
            var olObject = BookDataFactory.GetSingleOLObject();
            var param = new AddBookParameter(isbn, olObject);

            //Execute
            LoadData(param);

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
        public void ISBNFilledInIfViewModelStartedWithParameter()
        {
            //Setup
            var isbn = "5456554412";
            var param = new AddBookParameter(isbn, null);

            //Execute
            LoadData(param);

            //Assert
            var item = _screenManager.Items.OfType<CellBookNumberTextView>()
                                           .Where(x => x.InputType == EBookInputType.ISBN)
                                           .First();
            item.TxtInput.Should().Be(isbn);
        }

        [Fact]
        public void IsLoadingSetToFalseAfterInitialize()
        {
            //Setup

            //Execute
            LoadData(null);

            //Assert
            _screenManager.IsLoading.Should().BeFalse();
        }

        [Fact]
        public void ButtonsAreDisabledAfterInitialize()
        {
            //Setup

            //Execute
            LoadData(null);

            //Assert
            var buttons = _screenManager.Items.Where(b => b is CellBookButton)
                                              .OfType<CellBookButton>();

            foreach (var item in buttons)
                item.IsValid.Should().BeFalse();
        }

        [Theory, MemberData(nameof(InputDataForBooks))]
        public void CheckIfButtonsHaveCorrectValidationAfterInput(string title, Author author, string pages, bool isValid)
        {
            //Setup
            PrepareAuthorData();
            LoadData(null);

            //Execute
            var cellBasicInfo = _screenManager.Items.OfType<CellBasicBook>()
                                                    .Where(x => x.InputType == EBookInputType.BasicInfo)
                                                    .First();
            cellBasicInfo.TxtTitle = title;
            cellBasicInfo.Author = author;

            var cellPages = _screenManager.Items.OfType<CellBookNumberTextView>()
                                                .Where(x => x.InputType == EBookInputType.Pages)
                                                .First();
            cellPages.TxtInput = pages;

            //Assert
            var buttons = _screenManager.Items.Where(b => b is CellBookButton).OfType<CellBookButton>();
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
