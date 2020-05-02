using System;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Tests;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels.Genre
{
    public class GenreViewModelTests : BaseViewModelTests
    {
        #region Constructor

        public GenreViewModelTests()
        {
            Setup();
        }

        #endregion

        #region Setup

        GenreViewModel LoadViewModel()
        {
            var vm = Ioc.IoCConstruct<GenreViewModel>();
            vm.Refresh();

            return vm;
        }

        #endregion

        #region Genre check Genre items

        [Fact]
        public void ShowGenresWhenDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetGenre4ElementsComplete()));

            //Setup
            var vm = LoadViewModel();

            //Check
            Assert.NotNull(vm.Genres);
            Assert.NotEmpty(vm.Genres);
            Assert.Equal(4, vm.Genres.Count);
        }

        [Fact]
        public void ShowEmptyGenresNoDataAvailable()
        {
            //Arrange
            MockThePageService
                .Setup(x => x.GetAllGenres())
                .Returns(() => Task.FromResult(GenreDataFactory.GetGenreEmpty()));

            //Setup
            var vm = LoadViewModel();

            //Check
            Assert.NotNull(vm.Genres);
            Assert.Empty(vm.Genres);
        }

        [Fact]
        public void ShowNullGenresNoDataAvailable()
        {
            //Setup
            var vm = LoadViewModel();

            //Check
            Assert.Null(vm.Genres);
        }

        [Fact]
        public void StopLoadingAfterRefresh()
        {
            //Setup
            var vm = LoadViewModel();

            Assert.False(vm.IsLoading);
        }

        #endregion
    }
}
