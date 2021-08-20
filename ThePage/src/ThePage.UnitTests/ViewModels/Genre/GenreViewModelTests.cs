using System.Linq;
using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels
{
    public class GenreViewModelTests : BaseServicesTests
    {
        readonly GenreViewModel _vm;

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
            MockGenreService
                .Setup(x => x.GetGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetGenre4ElementsComplete()));

            //Execute
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
            MockGenreService
                .Setup(x => x.GetGenres())
                .Returns(() => Task.FromResult(Enumerable.Empty<Genre>()));

            //Execute
            LoadViewModel();

            //Check
            Assert.NotNull(_vm.Genres);
            Assert.Empty(_vm.Genres);
        }

        [Fact]
        public void StopLoadingAfterRefresh()
        {
            //Arrange
            MockGenreService
                .Setup(x => x.GetGenres())
                .Returns(() => Task.FromResult(Enumerable.Empty<Genre>()));

            //Execute
            LoadViewModel();

            //Assert
            Assert.False(_vm.IsLoading);
        }

        #endregion
    }
}