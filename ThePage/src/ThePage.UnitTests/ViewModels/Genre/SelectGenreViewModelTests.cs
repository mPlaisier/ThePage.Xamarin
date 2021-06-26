using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThePage.Core;
using Xunit;

namespace ThePage.UnitTests.ViewModels
{
    public class SelectGenreViewModelTests : BaseViewModelTests
    {
        readonly SelectGenreViewModel _vm;

        #region Constructor

        public SelectGenreViewModelTests()
        {
            Setup();

            _vm = Ioc.IoCConstruct<SelectGenreViewModel>();
        }

        #endregion

        #region Setup

        void LoadViewModel(SelectedGenreParameters parameter)
        {
            _vm.Prepare(parameter);
            _vm.Initialize();
        }

        void SetupViewModelWithOneSelectedGenre()
        {
            var genres = GenreDataFactory.GetGenre4ElementsComplete();
            MockGenreService
                .Setup(x => x.GetGenres())
                .Returns(() => Task.FromResult(genres));

            var selectedGenre = new List<Genre> { genres.First() };
            LoadViewModel(new SelectedGenreParameters(selectedGenre));
        }

        #endregion

        [Fact]
        public void CountSelectedItemsCount()
        {
            //Arrange
            SetupViewModelWithOneSelectedGenre();

            Assert.Single(_vm.SelectedItems);
            Assert.Equal(4, _vm.Items.Count);
        }


        [Fact]
        public void CommandSelectItemGenreRemovedTest()
        {
            //Arrange
            SetupViewModelWithOneSelectedGenre();

            var selectedGenre = _vm.Items.Where(x => x.IsSelected == true).FirstOrDefault();
            _vm.CommandSelectItem.Execute(selectedGenre);

            var selectedGenres = _vm.Items.Where(x => x.IsSelected == true).ToList();
            Assert.Empty(selectedGenres);
            Assert.Empty(_vm.SelectedItems);
        }


        [Fact]
        public void CommandSelectItemGenreAddTest()
        {
            //Arrange
            SetupViewModelWithOneSelectedGenre();

            var selectedGenre = _vm.Items.Where(x => x.IsSelected == false).FirstOrDefault();
            _vm.CommandSelectItem.Execute(selectedGenre);

            var selectedGenres = _vm.Items.Where(x => x.IsSelected == true).ToList();
            Assert.Equal(2, selectedGenres.Count);
            Assert.Equal(2, _vm.SelectedItems.Count);
        }


        [Fact]
        public void NoCrashWhenNoDataAvailable()
        {
            //Arrange
            MockGenreService
                .Setup(x => x.GetGenres())
                .Returns(() => Task.FromResult<IEnumerable<Genre>>(null));
            LoadViewModel(new SelectedGenreParameters(new List<Genre>()));

            Assert.Empty(_vm.Items);
        }
    }
}
