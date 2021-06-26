using System.Threading.Tasks;
using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.BookShelf
{
    public class BookShelfViewModelTests : BaseViewModelTests
    {
        readonly BookShelfViewModel _vm;

        #region Constructor

        public BookShelfViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<BookShelfViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel()
        {
            _vm.Initialize();
        }

        #endregion

        [Fact]
        public void ShowBookShelvesWhenDataAvailable()
        {
            //Arrange
            MockBookShelfService
                .Setup(x => x.FetchBookshelves())
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetListBookShelf2Elements()));

            //Setup
            LoadViewModel();

            //Check
            _vm.BookShelves.Should().NotBeNullOrEmpty();
            _vm.BookShelves.Should().HaveCount(2);
        }

        [Fact]
        public void ShowEmptyBookShelfNoDataAvailable()
        {
            //Arrange
            MockBookShelfService
                .Setup(x => x.FetchBookshelves())
                .Returns(() => Task.FromResult(BookShelfDataFactory.GetListBookShelfEmpty()));

            //Setup
            LoadViewModel();

            //Check
            _vm.BookShelves.Should().BeEmpty();
            _vm.BookShelves.Should().NotBeNull();
        }
    }
}
