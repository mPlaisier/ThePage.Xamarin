using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.BookShelf
{
    public class BookShelfDetailViewModelTests : BaseServicesTests
    {
        readonly BookShelfDetailViewModel _vm;

        #region Constructor

        public BookShelfDetailViewModelTests()
        {
            Setup();
            _vm = Ioc.IoCConstruct<BookShelfDetailViewModel>();
        }

        #endregion

        void LoadViewModel(Bookshelf parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        [Fact]
        public void CheckBookShelfItemsCountWithBooks()
        {
            //Arrange
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            //Assert
            Assert.Equal(7, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfItemsCountWithoutBooks()
        {
            //Arrange
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithoutBooks()));

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
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));

            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Execute
            _vm.Initialize();

            _vm.EditBookShelfCommand.Execute();

            //Assert
            Assert.Equal(8, _vm.Items.Count);
        }

        [Fact]
        public void CheckBookShelfItemsCountWithoutBooksAfterEdit()
        {
            //Arrange
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithoutBooks()));

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
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));
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
            MockBookShelfService
                  .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithoutBooks()));
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
            MockBookShelfService
                  .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));
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
            MockBookShelfService
                  .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithoutBooks()));
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
            MockBookShelfService
                  .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                 .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));
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
            MockBookShelfService
                .Setup(x => x.FetchBookShelf(It.IsAny<string>()))
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetBookShelfDetailWithBooks()));
            LoadViewModel(BookShelfDataFactory.GetSingleBookfShelfWithBooks());

            //Excute
            _vm.Initialize();
            _vm.EditBookShelfCommand.Execute();

            //Assert
            Assert.True(_vm.IsEditing);
        }
    }
}