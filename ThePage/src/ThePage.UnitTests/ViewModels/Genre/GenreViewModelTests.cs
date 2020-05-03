using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class GenreViewModelTests : BaseViewModelTests
    {
        GenreViewModel _vm;

        #region Constructor

        public GenreViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<GenreViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel()
        {
            _vm.Initialize();
        }

        #endregion

        #region Genre check Genre items

        [Fact]
        public void ShowGenresWhenDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetListGenre4ElementsComplete()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Genres);
            Assert.NotEmpty(_vm.Genres);
            Assert.Equal(4, _vm.Genres.Count);
        }

        [Fact]
        public void ShowEmptyGenresNoDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetListGenreEmpty()));

            //Setup
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Genres);
            Assert.Empty(_vm.Genres);
        }

        [Fact]
        public void ShowNullGenresNoDataAvailable()
        {
            //Setup
            LoadViewModel();

            //Check
            Assert.Null(_vm.Genres);
        }

        [Fact]
        public void StopLoadingAfterRefresh()
        {
            //Setup
            LoadViewModel();

            Assert.False(_vm.IsLoading);
        }

        #endregion
    }
}