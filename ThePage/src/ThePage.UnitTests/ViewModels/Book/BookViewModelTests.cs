using System.Threading.Tasks;
using FluentAssertions;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class BookViewModelTests : BaseServicesTests
    {
        readonly BookViewModel _vm;

        #region Constructor

        public BookViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<BookViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel()
        {
            _vm.Initialize();
        }

        #endregion

        [Fact]
        public void ShowBooksWhenDataAvailable()
        {
            //Arrange
            MockBookService
               .Setup(x => x.FetchBooks())
               .Returns(() => Task.FromResult(BookDataFactory.GetListBook4ElementsComplete()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Books);
            Assert.NotEmpty(_vm.Books);
            Assert.Equal(4, _vm.Books.Count);
        }

        [Fact]
        public void ShowEmptyBooksNoDataAvailable()
        {
            //Arrange
            MockBookService
                .Setup(x => x.FetchBooks())
                .Returns(() => Task.FromResult(BookDataFactory.GetListBookEmpty()));

            //Setup
            LoadViewModel();

            //Check
            _vm.Books.Should().NotBeNull();
            _vm.Books.Should().BeEmpty();
        }
    }
}
