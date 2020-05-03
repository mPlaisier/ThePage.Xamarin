using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Book
{
    public class BookViewModelTests : BaseViewModelTests
    {
        BookViewModel _vm;

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
            MockThePageService
                .Setup(x => x.GetAllBooks())
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
            MockThePageService
                .Setup(x => x.GetAllBooks())
                .Returns(() => Task.FromResult(BookDataFactory.GetListBookEmpty()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Books);
            Assert.Empty(_vm.Books);
        }

        [Fact]
        public void ShowNullBooksNoDataAvailable()
        {
            //Setup
            LoadViewModel();

            //Check
            Assert.Null(_vm.Books);
        }

        [Fact]
        public void StopLoadingAfterRefresh()
        {
            //Setup
            LoadViewModel();

            Assert.False(_vm.IsLoading);
        }
    }
}
