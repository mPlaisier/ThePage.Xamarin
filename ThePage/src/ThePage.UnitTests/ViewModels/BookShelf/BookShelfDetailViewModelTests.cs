using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ThePage.Api;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.BookShelf
{
    public class BookShelfDetailViewModelTests : BaseViewModelTests
    {
        readonly BookShelfDetailViewModel _vm;

        #region Constructor

        public BookShelfDetailViewModelTests()
        {
            Setup();
            _vm = Ioc.IoCConstruct<BookShelfDetailViewModel>();
        }

        #endregion

        void LoadViewModel(ApiBookShelf parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        [Fact]
        public void CheckBookShelfItemsCountWithBooks()
        {
            //Arrange
            MockThePageService
                   .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                  .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            //Assert
            Assert.Equal(6, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfItemsCountWithoutBooks()
        {
            //Arrange
            MockThePageService
                   .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                  .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithoutBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithoutBooks());

            //Execute
            _vm.Initialize();

            //Assert
            Assert.Equal(3, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfItemsCountWithBooksAfterEdit()
        {
            //Arrange
            MockThePageService
                   .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                  .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            _vm.EditBookShelfCommand.Execute();

            //Assert
            Assert.Equal(7, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfItemsCountWithoutBooksAfterEdit()
        {
            //Arrange
            MockThePageService
                   .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                  .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithoutBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithoutBooks());

            //Execute
            _vm.Initialize();

            _vm.EditBookShelfCommand.Execute();

            //Assert
            Assert.Equal(4, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfWithBooksCellItemsTypesAndOrder()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            //Assert
            _vm.Items[0].Should().BeOfType<BaseCellTitle>();
            _vm.Items[1].Should().BeOfType<CellBookShelfTextView>();
            _vm.Items[2].Should().BeOfType<BaseCellTitle>();
            _vm.Items[3].Should().BeOfType<CellBookShelfBookItem>();
            _vm.Items[4].Should().BeOfType<CellBookShelfBookItem>();
            _vm.Items[5].Should().BeOfType<CellBookShelfBookItem>();
        }

        [Fact]
        public void CheckBookShelfWithoutBooksCellItemsTypesAndOrder()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithoutBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithoutBooks());

            //Execute
            _vm.Initialize();

            //Assert
            _vm.Items[0].Should().BeOfType<BaseCellTitle>();
            _vm.Items[1].Should().BeOfType<CellBookShelfTextView>();
            _vm.Items[2].Should().BeOfType<BaseCellTitle>();
        }

        [Fact]
        public void CheckBookShelfWithBooksCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();
            _vm.EditBookShelfCommand.Execute();

            //Assert
            _vm.Items[0].Should().BeOfType<BaseCellTitle>();
            _vm.Items[1].Should().BeOfType<CellBookShelfTextView>();
            _vm.Items[2].Should().BeOfType<BaseCellTitle>();
            _vm.Items[3].Should().BeOfType<BaseCellClickableText>();
            _vm.Items[4].Should().BeOfType<CellBookShelfBookItem>();
            _vm.Items[5].Should().BeOfType<CellBookShelfBookItem>();
            _vm.Items[6].Should().BeOfType<CellBookShelfBookItem>();
        }

        [Fact]
        public void CheckBookShelfWithoutBooksCellItemsTypesAndOrderAfterEditCommand()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithoutBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithoutBooks());

            //Execute
            _vm.Initialize();
            _vm.EditBookShelfCommand.Execute();

            //Assert
            _vm.Items[0].Should().BeOfType<BaseCellTitle>();
            _vm.Items[1].Should().BeOfType<CellBookShelfTextView>();
            _vm.Items[2].Should().BeOfType<BaseCellTitle>();
            _vm.Items[3].Should().BeOfType<BaseCellClickableText>();
        }

        [Fact]
        public void BookDetailEditIsDisabledAStart()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            //Assert
            Assert.False(_vm.IsEditing);
        }

        [Fact]
        public void BookShelfDetailEditIsEnabledAferEditCommand()
        {
            //Arrange
            MockThePageService
                  .Setup(x => x.GetBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetApiBookShelfDetailResponseWithBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Excute
            _vm.Initialize();
            _vm.EditBookShelfCommand.Execute();

            //Assert
            Assert.True(_vm.IsEditing);
        }
    }
}